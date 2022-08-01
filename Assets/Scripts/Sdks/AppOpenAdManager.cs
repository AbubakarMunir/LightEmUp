using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using System;
using GoogleMobileAds.Common;
using UnityEngine.UI;

public class AppOpenAdManager : MonoBehaviour
{
    public static bool isInterstialAdPresent;

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        //MobileAds.Initialize(initStatus => { });
        //LoadAd();
        //Invoke("ShowAppOpenAd", 5f);
    }

#if UNITY_ANDROID
    private const string AD_UNIT_ID = "ca-app-pub-6662467219300949/2147635045";
#elif UNITY_IOS
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/3419835294";
#else
    private const string AD_UNIT_ID = "unexpected_platform";
#endif

    private static AppOpenAdManager instance;

    private AppOpenAd ad;

    public static bool isShowingAd = false;
    private DateTime loadTime;

    public static AppOpenAdManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new AppOpenAdManager();
            }

            return instance;
        }
    }

    private bool IsAdAvailable
    {
        get
        {
            return ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
            ;
        }
    }

    public void LoadAd()
    {
        AdRequest request = new AdRequest.Builder().Build();

        // Load an app open ad for portrait orientation
        AppOpenAd.LoadAd(AD_UNIT_ID, ScreenOrientation.Landscape, request, ((appOpenAd, error) =>
        {
            if (error != null)
            {
                // Handle the error.
                Debug.LogFormat("Failed to load the ad. (reason: {0})", error.LoadAdError.GetMessage());
                return;
            }

            // App open ad is loaded.
            ad = appOpenAd;
            loadTime = DateTime.UtcNow;
            Debug.Log("End Request  ");
        }));
    }

    public void ShowAppOpenAd()
    {
        print(isShowingAd);
        if (isShowingAd)
        {
            Debug.Log("IsShowingAd");
            return;
        }
        if (ad == null)
        {
            Debug.Log("ad == null");
            LoadAd();
            return;
        }

        // Register for ad events.
        this.ad.OnAdDidDismissFullScreenContent += (sender, args) =>
        {
            isShowingAd = false;
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("AppOpenAd dismissed.");
                if (this.ad != null)
                {
                    this.ad.Destroy();
                    this.ad = null;
                }

                LoadAd();
            });
        };
        this.ad.OnAdFailedToPresentFullScreenContent += (sender, args) =>
        {
            isShowingAd = false;
            var msg = args.AdError.GetMessage();
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                // statusText.text = "AppOpenAd present failed, error: " + msg;
                if (this.ad != null)
                {
                    this.ad.Destroy();
                    this.ad = null;
                }
            });
        };
        this.ad.OnAdDidPresentFullScreenContent += (sender, args) =>
        {
            isShowingAd = true;
            MobileAdsEventExecutor.ExecuteInUpdate(() => { Debug.Log("AppOpenAd presented."); });
        };
        this.ad.OnAdDidRecordImpression += (sender, args) =>
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                Debug.Log("AppOpenAd recorded an impression.");
                LoadAd();
            });
        };
        this.ad.OnPaidEvent += (sender, args) =>
        {
            string currencyCode = args.AdValue.CurrencyCode;
            long adValue = args.AdValue.Value;
            string suffix = "AppOpenAd received a paid event.";
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                string msg = string.Format("{0} (currency: {1}, value: {2}", suffix, currencyCode, adValue);
                //statusText.text = msg;
            });
        };
        if (toggle == false)
            return;
        ad.Show();
        toggle = false;
        Invoke(nameof(AppOpenToggle), 1);
    }

    private bool toggle = true;

    void AppOpenToggle()
    {
        toggle = true;
    }

    public void OnApplicationPause(bool paused)
    {
        if (Constants.enableAds == "0")
            return;
        // Display the app open ad when the app is foregrounded
        if (!paused)
        {
            if (isInterstialAdPresent)
            {
                isInterstialAdPresent = false;
                return;
            }
            ShowAppOpenAd();
        }
    }
}