using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

[Serializable]
public class ApplovinId
{
    public string APPLOVIN_SDK_KEY;
    public string APPLOVIN_INTERTITIAL_AD_ID, APPLOVIN_REWARDED_AD_ID, APPLOVIN_BANNER_AD_ID;
}
public class AdsManager : MonoBehaviour
{
    private string applovinSdkKey;
    private string interstitialID;
    private string bannerID;
    private string rewardedID;

    public ApplovinId Android_Applovin_ID = new ApplovinId();
    public ApplovinId Ios_Applovin_ID = new ApplovinId();
    [HideInInspector]
    public ApplovinId APPLOVIN_ID = new ApplovinId();
    int retryAttempt;
    public delegate void MaxRewardUserDelegate();
    private static MaxRewardUserDelegate NotifyReward;
    MediationHandler admobAdsManager;
    // Start is called before the first frame update
    void Awake()
    {
        #if UNITY_ANDROID
                APPLOVIN_ID = Android_Applovin_ID;
        #elif UNITY_IOS
                APPLOVIN_ID = Ios_Applovin_ID;
        #endif
        

        applovinSdkKey= APPLOVIN_ID.APPLOVIN_SDK_KEY;
        interstitialID = APPLOVIN_ID.APPLOVIN_INTERTITIAL_AD_ID;
        bannerID = APPLOVIN_ID.APPLOVIN_BANNER_AD_ID;
        
        rewardedID = APPLOVIN_ID.APPLOVIN_REWARDED_AD_ID;
        //Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task => {
        //    var dependencyStatus = task.Result;
        //    if (dependencyStatus == Firebase.DependencyStatus.Available)
        //    {
        //        // Create and hold a reference to your FirebaseApp,
        //        // where app is a Firebase.FirebaseApp property of your application class.
        //        Firebase.FirebaseApp app = Firebase.FirebaseApp.DefaultInstance;

        //        // Set a flag here to indicate whether Firebase is ready to use by your app.
        //        ShowDebugLogs("Firebase Ready!");
        //    }
        //    else
        //    {
        //        UnityEngine.Debug.LogError(System.String.Format(
        //          "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
        //        // Firebase Unity SDK is not safe to use here.
        //    }
        //});


        MaxSdkCallbacks.OnSdkInitializedEvent += (MaxSdkBase.SdkConfiguration sdkConfiguration) => {
            // AppLovin SDK is initialized, start loading ads
            //Debug.LogError("Max Initialized");
            //InitializeInterstitialAds();
            InitializeRewardedAds();
            InitializeBannerAds();
           
        };

        MaxSdk.SetSdkKey(applovinSdkKey);
        //MaxSdk.SetUserId("USER_ID");
        MaxSdk.InitializeSdk();
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        admobAdsManager = FindObjectOfType<MediationHandler>();
        Constants.timeBwIA = 0;
    }

    private void Update()
    {
        if (Constants.timeBwIA > 0)
            Constants.timeBwIA -= Time.deltaTime;
    }

    #region InterstitialAds
    public bool IsInterstitialReady()
    {
        //return false;
        if (MaxSdk.IsInterstitialReady(interstitialID))
            return true;
        else
            return false;
    }
    public void InitializeInterstitialAds()
    {
        if (PreferenceManager.GetAdStatus() == 1)
        {
            // Attach callback
            

            // Load the first interstitial
            LoadInterstitial();
        }
    }

    private void LoadInterstitial()
    {
        if (Constants.enableAds == "0")
            return;
        if (!MaxSdk.IsInterstitialReady(interstitialID))
        {
            ShowDebugLogs("Load Interstitial Called");
            MaxSdkCallbacks.Interstitial.OnAdLoadedEvent += OnInterstitialLoadedEvent;
            MaxSdkCallbacks.Interstitial.OnAdLoadFailedEvent += OnInterstitialLoadFailedEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayedEvent += OnInterstitialDisplayedEvent;
            MaxSdkCallbacks.Interstitial.OnAdClickedEvent += OnInterstitialClickedEvent;
            MaxSdkCallbacks.Interstitial.OnAdHiddenEvent += OnInterstitialHiddenEvent;
            MaxSdkCallbacks.Interstitial.OnAdDisplayFailedEvent += OnInterstitialAdFailedToDisplayEvent;
            MaxSdk.LoadInterstitial(interstitialID);
        }
        else
        {
            ShowDebugLogs("Interstitial already Loaded!");
        }
    }
    public void ShowInterstitial()
    {
        if (Constants.enableAds == "0")
            return;
        if (PreferenceManager.GetAdStatus() == 1 && PreferenceManager.GetAttemptedLevelCount() > 2 && Constants.timeBwIA <= 0)
        {
            if (MaxSdk.IsInterstitialReady(interstitialID))
            {
                ShowDebugLogs("Show Interstitial Called");
                AdmobAdsManager.isInterstialAdPresent = true;
                Constants.timeBwIA = 40;
                MaxSdk.ShowInterstitial(interstitialID);
            }
            else
            {
                MaxSdk.LoadInterstitial(interstitialID);
            }
            //else
            //{
            //    ShowDebugLogs("Interstitial NOT Loaded!");
            //    admobAdsManager.ShowAd();
            //}
        }
    }
    private void OnInterstitialLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is ready for you to show. MaxSdk.IsInterstitialReady(adUnitId) now returns 'true'
        ShowDebugLogs("Interstitial Loaded");
        // Reset retry attempt
        //retryAttempt = 0;
    }

    private void OnInterstitialLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Interstitial ad failed to load
        ShowDebugLogs("Interstitial Failed to load: " + errorInfo);
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds)
        
        //retryAttempt++;
        //double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        //Invoke("LoadInterstitial", (float)retryDelay);
    }

    private void OnInterstitialDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        ShowDebugLogs("Interstitial Displayed");
    }

    private void OnInterstitialAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad failed to display. AppLovin recommends that you load the next ad.
        ShowDebugLogs("Interstitial Failed to Display: "+ errorInfo);
        LoadInterstitial();
    }

    private void OnInterstitialClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnInterstitialHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Interstitial ad is hidden. Pre-load the next ad.
        ShowDebugLogs("Interstitial Closed");
        LoadInterstitial();
    }
    #endregion

#region RewardedAds
    public void InitializeRewardedAds()
    {
        // Attach callback
        if (Constants.enableAds == "0")
            return;

        // Load the first rewarded ad
        LoadRewardedAd();
    }
    public bool IsRewardedAdReady()
    {
        //return false;
        if (MaxSdk.IsRewardedAdReady(rewardedID))
            return true;
        else
            return false;
    }
    public void ShowRewardedAdTest()
    {
        if(Constants.enableAds == "1")
            ShowRewardedAd(GiveRewardNow);
    }
    private void GiveRewardNow()
    {
        ShowDebugLogs("Reward Given Successfully!");
    }
    public void ShowRewardedAd(MaxRewardUserDelegate _delegate)
    {
        if (Constants.enableAds == "0")
            return;
        NotifyReward = _delegate;
        
        ShowDebugLogs("Show Rewarded Called");
        AdmobAdsManager.isInterstialAdPresent = true;
        MaxSdk.ShowRewardedAd(rewardedID);
        
    }
    private void LoadRewardedAd()
    {
        if (Constants.enableAds == "0")
            return;
        if (!MaxSdk.IsRewardedAdReady(rewardedID))
        {
            ShowDebugLogs("Load Rewarded Ad Called");
            MaxSdkCallbacks.Rewarded.OnAdLoadedEvent += OnRewardedAdLoadedEvent;
            MaxSdkCallbacks.Rewarded.OnAdLoadFailedEvent += OnRewardedAdLoadFailedEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayedEvent += OnRewardedAdDisplayedEvent;
            MaxSdkCallbacks.Rewarded.OnAdClickedEvent += OnRewardedAdClickedEvent;
            MaxSdkCallbacks.Rewarded.OnAdRevenuePaidEvent += OnRewardedAdRevenuePaidEvent;
            MaxSdkCallbacks.Rewarded.OnAdHiddenEvent += OnRewardedAdHiddenEvent;
            MaxSdkCallbacks.Rewarded.OnAdDisplayFailedEvent += OnRewardedAdFailedToDisplayEvent;
            MaxSdkCallbacks.Rewarded.OnAdReceivedRewardEvent += OnRewardedAdReceivedRewardEvent;
            MaxSdk.LoadRewardedAd(rewardedID);
        }
        else
        {
            ShowDebugLogs("Rewarded Ad Already Loaded");
        }
    }

    private void OnRewardedAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Rewarded ad is ready for you to show. MaxSdk.IsRewardedAdReady(adUnitId) now returns 'true'.
        ShowDebugLogs("Rewarded Ad Loaded");
        // Reset retry attempt
        retryAttempt = 0;
    }

    private void OnRewardedAdLoadFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        // Rewarded ad failed to load
        ShowDebugLogs("Rewarded Ad Failed to load: " + errorInfo);
        // AppLovin recommends that you retry with exponentially higher delays, up to a maximum delay (in this case 64 seconds).

        retryAttempt++;
        double retryDelay = Math.Pow(2, Math.Min(6, retryAttempt));

        Invoke("LoadRewardedAd", (float)retryDelay);
    }

    private void OnRewardedAdDisplayedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) {
        ShowDebugLogs("Rewarded Ad Dispalyed");
    }

    private void OnRewardedAdFailedToDisplayEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo, MaxSdkBase.AdInfo adInfo)
    {
        ShowDebugLogs("Rewarded Failed to display: " + errorInfo);
        // Rewarded ad failed to display. AppLovin recommends that you load the next ad.
        LoadRewardedAd();
    }

    private void OnRewardedAdClickedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo) { }

    private void OnRewardedAdHiddenEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        ShowDebugLogs("Rewarded Ad Closed");
        // Rewarded ad is hidden. Pre-load the next ad
        //if(Constants.isUnlockPanelClosed)
        //    EventManager.DoFireUnlockPanelClosed();
        LoadRewardedAd();
    }

    private void OnRewardedAdReceivedRewardEvent(string adUnitId, MaxSdk.Reward reward, MaxSdkBase.AdInfo adInfo)
    {
        // The rewarded ad displayed and the user should receive the reward.
        NotifyReward();
    }

    private void OnRewardedAdRevenuePaidEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        // Ad revenue paid. Use this callback to track user revenue.
    }
    #endregion

    #region SmallBanner
    public bool bannerAdLoaded = false;

    public void InitializeBannerAds()
    {
        if (PreferenceManager.GetAdStatus() == 1 && Constants.enableAds=="1")
        {
            MaxSdkCallbacks.Banner.OnAdLoadedEvent += BannerAdLoadedEvent;
            MaxSdkCallbacks.Banner.OnAdLoadFailedEvent += BannerAdFailedEvent;
            // Banners are automatically sized to 320×50 on phones and 728×90 on tablets
            // You may call the utility method MaxSdkUtils.isTablet() to help with view sizing adjustments
            MaxSdk.CreateBanner(bannerID, MaxSdkBase.BannerPosition.BottomCenter);
            //MaxSdk.SetBannerExtraParameter(bannerID, "adaptive_banner", "true");
            // Set background or background color for banners to be fully functional
            MaxSdk.SetBannerBackgroundColor(bannerID,Color.clear);
        }
    }
    void BannerAdLoadedEvent(string adUnitId, MaxSdkBase.AdInfo adInfo)
    {
        bannerAdLoaded = true;
    }
    void BannerAdFailedEvent(string adUnitId, MaxSdkBase.ErrorInfo errorInfo)
    {
        bannerAdLoaded = false;
    }
    public bool isMaxBannerDisplayed = false;
    public void ShowBanner()
    {
        if (Constants.enableAds=="1" && PreferenceManager.GetAdStatus() == 1 && bannerAdLoaded && Constants.enableAds == "1" && PreferenceManager.GetAttemptedLevelCount() > 2)
        { 
            MaxSdk.ShowBanner(bannerID); 
            isMaxBannerDisplayed = true;
        }
        else
        {
            isMaxBannerDisplayed = false;
        }
    }

    public void HideBanner()
    {
        MaxSdk.HideBanner(bannerID);
    }

    public void DestroyBanner()
    {
        MaxSdk.DestroyBanner(bannerID);
    }
    //public Text debugTxt;
    private void ShowDebugLogs(string log)
    {
        //debugTxt.text = log.ToString();
    }
    #endregion
  
}
