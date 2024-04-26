using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChartboostSDK;
using UnityEngine.Advertisements;
using UnityEngine.Serialization;

public class AdsManagerAB : MonoBehaviour
{
    [Header("Admob ids")] [SerializeField] private string appIdD = "ca-app-pub-3940256099942544~1458002511";
    [SerializeField] private string bannerAdIdD = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField] private string interstitialAdId = "ca-app-pub-3940256099942544/1033173712";

    [FormerlySerializedAs("bHasBannerR")] [Header("Banner Settings")] [SerializeField]
    private bool isHasBannerR = false;

    [FormerlySerializedAs("bShowBannerAtStartT")] [SerializeField] private bool isShowBannerAtStartT = false;

    [Header("Unity Id")] [SerializeField] private string unityAdIdD = "";

    public static AdsManagerAB instanceE = null;

    private void Awake()
    {
        if (instanceE == null)
        {
            instanceE = this;
            DontDestroyOnLoad(this);
        }
        else
        {
            Destroy(this);
        }
    }

    public void Start()
    {
        


        if (isShowBannerAtStartT)
            this.RequestBannerR();

        RequestInterstitialL();
        
        Chartboost.cacheInterstitial(CBLocation.Default);
    }


    private void RequestInterstitialL()
    {
        string adUnitId = interstitialAdId;

        
    }


    private void RequestBannerR()
    {
        if (!isHasBannerR)
            return;

        string adUnitId = bannerAdIdD;
    }


    public void ShowBannerR()
    {
        RequestBannerR();
    }

    public void ShowInterR()
    {
    }

    public void ShowUnityY()
    {
       
    }

    public void ShowCbB()
    {
        if (Chartboost.hasInterstitial(CBLocation.Default))
            Chartboost.showInterstitial(CBLocation.Default);
        else
            Chartboost.cacheInterstitial(CBLocation.Default);
    }

    public void CloseBannerR()
    {
    }
}