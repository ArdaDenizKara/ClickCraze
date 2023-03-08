using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class AdManager : MonoBehaviour
{
    private InterstitialAd interstitialAd;
    private void Start()
    {
        MobileAds.Initialize(initStatus => { });
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
}
