using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    #region Fields
    
    public Text coinText;
    public Text powerUp1Text;
    public Text powerUp2Text;
    public Text powerUp3Text;
    #endregion
    #region Function
    private void Start()
    {
        coinText.text = PlayerManager.Instance.coinCount.ToString();
        powerUp1Text.text = 2 + " Seconds";
        powerUp2Text.text = 1 + "";
        powerUp3Text.text = 30 + " Seconds";
    }
    
    #endregion
}

