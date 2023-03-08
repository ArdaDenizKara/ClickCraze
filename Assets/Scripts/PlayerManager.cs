using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    #region Fields
    public int coinCount;
    public float multiplierDuration = 2f;
    private static PlayerManager _instance;
    public AudioSource AudioSource;
    public GameObject musicOn;
    public GameObject musicOff;
    

    public static PlayerManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = GameObject.FindObjectOfType<PlayerManager>();
            }


            return _instance;
        }
    }

    #endregion
    #region Functions
    private void Awake()
    {

        if (_instance != null)
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
        coinCount = PlayerPrefs.GetInt("coinCount", 0);
    }
    #endregion
    public void MusicOn()
    {
        AudioSource.Play();
        musicOff.SetActive(false);
        musicOn.SetActive(true);
    }
    public void MusicOff()
    {
        AudioSource.Stop();
        musicOn.SetActive(false);
        musicOff.SetActive(true);
    }
    
}


