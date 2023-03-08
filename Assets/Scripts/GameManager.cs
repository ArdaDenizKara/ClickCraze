using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;
public class GameManager : MonoBehaviour
{
    
    #region Fields
    //OBJECTS PREFABS
    [SerializeField]
    public List<GameObject> objectsPrefabs = new List<GameObject>();
    public List<GameObject> objectsPrefabsWith2Bombs = new List<GameObject>();
    public List<GameObject> objectsPrefabsWith5Bombs = new List<GameObject>(); 
    //OBJECTS TO INSTANTIATE
    [SerializeField]
    public List<GameObject> objectsToInstantiate = new List<GameObject>();
    public List<GameObject> objectsWith2Bombs = new List<GameObject>();
    public List<GameObject> objectsWith5Bombs = new List<GameObject>();
    // INSTANTIATED OBJECTS POSITIONS
    public Vector3[] instantiatedObjectsPositions;
    //OTHERS
    public float timeRemaining = 30f;
    public Text remainingTimeText;
    public Text scoreText;
    public Text highScoreText;
    public Text gameOverScoreText;
    public int score;
    public int scoreIncrement = 1;
    public float scoreMultiplier = 1f;
    [SerializeField]
    public float multiplierDuration = 2f;
    bool isGameOver = false;
    public GameObject gameOverPanel;
    public GameObject bronzeIcon;
    public GameObject goldIcon;
    public GameObject silverIcon;
    private InterstitialAd interstitialAd;
    bool isAdShowed = false;
    #endregion
    #region Functions
    private void Awake()
    {
       
        SpawnObjects();
        StartCoroutine(StartCountdown());
        highScoreText.text = "Best : " + PlayerPrefs.GetInt("BestScore", 0).ToString();
       
        
    }
    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
        //this.RequestIntersititial();
        isGameOver = false;
        
    }
    public void Update()
    {
        if (timeRemaining<=0)
        {
            isGameOver = true;
            GameOver();
        }
        ChangeScoreText();
        ChangeTimeText();
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.GetRayIntersection(ray);
            if (isGameOver == false)
            {
                if (hit.collider != null && objectsToInstantiate.Contains(hit.collider.gameObject) || objectsWith2Bombs.Contains(hit.collider.gameObject) || objectsWith5Bombs.Contains(hit.collider.gameObject)) 
                {
                    if (hit.collider.CompareTag("Objects"))
                    {
                        objectsToInstantiate.Remove(hit.collider.gameObject);
                        
                        score += (int)(scoreIncrement * scoreMultiplier);
                        PlayerManager.Instance.coinCount += (int)(score * scoreMultiplier);


                        Destroy(hit.collider.gameObject);
                    }
                    if (hit.collider.CompareTag("TimePowerUp"))
                    {
                        objectsToInstantiate.Remove(hit.collider.gameObject);
                        timeRemaining += 1;
                        Destroy(hit.collider.gameObject);
                    }
                    if (hit.collider.CompareTag("ScoreMultiplier"))
                    {
                        scoreMultiplier *= 2f;
                        objectsToInstantiate.Remove(hit.collider.gameObject);
                        Destroy(hit.collider.gameObject);
                        StartCoroutine(DecrementMultiplierDuration());
                    }
                    if (hit.collider.CompareTag("Bomb"))
                    {
                        timeRemaining = 0;
                        isGameOver = true;
                        if (score > PlayerPrefs.GetInt("BestScore", 0))
                        {
                            PlayerPrefs.SetInt("BestScore", score);
                            highScoreText.text = "Best : " + score.ToString();
                        }
                        GameOver();
                    }
                    if (objectsToInstantiate.Count <= 14)
                    {
                        foreach (GameObject obj in objectsToInstantiate)
                        {
                            Destroy(obj);
                        }
                        objectsToInstantiate.Clear();
                        foreach (GameObject obj in objectsWith2Bombs)
                        {
                            Destroy(obj);
                        }
                        objectsWith2Bombs.Clear();
                        foreach (GameObject obj in objectsWith5Bombs)
                        {
                            Destroy(obj);
                        }
                        objectsWith5Bombs.Clear();
                        if (score<=10)
                        {
                            SpawnObjects();
                        }
                        else if (score<=25)
                        {
                            SpawnObjectsWith2Bombs();
                        }
                        else if(score>=26)
                        {
                            SpawnObjectsWith5Bombs();
                        }
                        
                    }
                }
            }
        }
    }
    public void SpawnObjects()
    {
        List<int> usedRandomIndexes = new List<int>();
        foreach (GameObject objectsPrefab in objectsPrefabs)
        {
            int random = Random.Range(0, 15);
            while (usedRandomIndexes.Contains(random))
            {
                random = Random.Range(0, 15);
            }
            usedRandomIndexes.Add(random);
            var createdObject = GameObject.Instantiate(objectsPrefab, instantiatedObjectsPositions[random], Quaternion.identity);
            objectsToInstantiate.Add(createdObject);
        }
    }
    public void SpawnObjectsWith2Bombs()
    {
        List<int> usedRandomIndexes = new List<int>();
        foreach (GameObject objectsPrefab in objectsPrefabsWith2Bombs)
        {
            int random = Random.Range(0, 15);
            while (usedRandomIndexes.Contains(random))
            {
                random = Random.Range(0, 15);
            }
            usedRandomIndexes.Add(random);
            var createdObject = GameObject.Instantiate(objectsPrefab, instantiatedObjectsPositions[random], Quaternion.identity);
            objectsWith2Bombs.Add(createdObject);
        }
    }
    public void SpawnObjectsWith5Bombs()
    {
        List<int> usedRandomIndexes = new List<int>();
        foreach (GameObject objectsPrefab in objectsPrefabsWith5Bombs)
        {
            int random = Random.Range(0, 15);
            while (usedRandomIndexes.Contains(random))
            {
                random = Random.Range(0, 15);
            }
            usedRandomIndexes.Add(random);
            var createdObject = GameObject.Instantiate(objectsPrefab, instantiatedObjectsPositions[random], Quaternion.identity);
            objectsWith5Bombs.Add(createdObject);
        }
    }
    public void ChangeScoreText()
    {
        scoreText.text = "Score : " + score.ToString();
        gameOverScoreText.text = "Score : " + score.ToString();
    }
    private void ChangeTimeText()
    {
        remainingTimeText.text = "Remaining Time : " + timeRemaining.ToString();
    }
    IEnumerator StartCountdown()
    {
        while (timeRemaining > 0)
        {
            yield return new WaitForSeconds(1f);
            timeRemaining--;
        }
    }
    IEnumerator DecrementMultiplierDuration()
    {
        yield return new WaitForSeconds(multiplierDuration);
        scoreMultiplier /= 2f;
    }
    private void GameOver()
    {
        
        if (isGameOver == true)
        {
            PlayerPrefs.SetInt("coinCount", PlayerManager.Instance.coinCount);
            foreach (GameObject gameobJect in objectsToInstantiate)
            {
                Destroy(gameobJect);
            }
            foreach (GameObject gameObject in objectsWith2Bombs)
            {
                Destroy(gameObject);
            }
            foreach (GameObject gameObject1 in objectsWith5Bombs)
            {
                Destroy(gameObject1);
            }
            remainingTimeText.gameObject.SetActive(false);
            scoreText.gameObject.SetActive(false);
            gameOverPanel.SetActive(true);
            if (score <= 10)
            {
                bronzeIcon.SetActive(true);
            }
            else if (score <= 50){
                silverIcon.SetActive(true);
            }
            else
            {
                goldIcon.SetActive(true);
            }
            if (isAdShowed ==false)
            {
                if (this.interstitialAd.IsLoaded())
                {
                    this.interstitialAd.Show();
                    isAdShowed = true;
                }
            }
        }
    }
    public void GameStart()
    {
        SceneManager.LoadScene("GamePlay");
    }
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }
    public void RequestIntersititial()
    {
#if UNITY_ANDROID
        string adID = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IPHONE
  string adID = "ca-app-pub-3940256099942544/1033173712";
#else
string adID = "unexpected_platform";
#endif
        this.interstitialAd = new InterstitialAd(adID);
        AdRequest request = new AdRequest.Builder().Build();
        this.interstitialAd.LoadAd(request);
    }
    #endregion
}
