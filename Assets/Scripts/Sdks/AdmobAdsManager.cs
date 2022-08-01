using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using GameAnalyticsSDK;

//using Facebook.Unity;

[Serializable]
public class AdmobId
{
    public string ADMOB_APP_ID;
    public string ADMOB_INTERTITIAL_AD_ID, ADMOB_VIDEO_AD_ID, ADMOB_BANNER_AD_ID, ADMOB_REWARDED_AD_ID , ADMOB_Medium_BANNER_AD_ID;
}
public class AdmobAdsManager : MediationHandler
{
    public bool enableTestAds;
    
    private static RewardUserDelegate AbmobNotifyReward;

    public AdmobId AndroidAdmob_ID = new AdmobId();
    public AdmobId IosAndroid_ID = new AdmobId();
    public AdmobId TestAdmob_ID = new AdmobId();
    [HideInInspector]
    public AdmobId ADMOB_ID = new AdmobId();

    [HideInInspector]
    public InterstitialAd interstitial;
    [HideInInspector]
    public InterstitialAd videoAd;
    [HideInInspector]
    public BannerView SmallBanner;
    [HideInInspector]
    public BannerView MediumBanner;
    [HideInInspector]
    public RewardedAd rewardBasedVideo;
    

    private bool isShowingAppOpenAd = false;
    bool isAdmobInitialized = false;



    public static bool isInterstialAdPresent;
#if UNITY_ANDROID
    private const string AD_UNIT_ID = "ca-app-pub-6662467219300949/2147635045";
#elif UNITY_IOS
    private const string AD_UNIT_ID = "ca-app-pub-3940256099942544/3419835294";
#else
    private const string AD_UNIT_ID = "unexpected_platform";
#endif


    private AppOpenAd ad;

    public static bool isShowingAd = false;
    private DateTime loadTime;


    private bool IsAdAvailable
    {
        get
        {
            return ad != null && (System.DateTime.UtcNow - loadTime).TotalHours < 4;
            ;
        }
    }

    public void LoadAppOpen()
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

    public override void ShowAppOpenAd()
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
            LoadAppOpen();
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

                LoadAppOpen();
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
                LoadAppOpen();
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


    void Awake()
    {

        DontDestroyOnLoad(this.gameObject);
        //Debug.Log("GN:GA not initialized");
       
        
    }

    
    // Start is called before the first frame update
    void Start()
    {
        //GameAnalytics.Initialize();
        if (enableTestAds)
        {
            ADMOB_ID = TestAdmob_ID;
        }
        else
        {
#if UNITY_ANDROID
            ADMOB_ID = AndroidAdmob_ID;
#elif UNITY_IOS
        ADMOB_ID = IosAndroid_ID;
#endif
        }

        Init();

    }

    private void Init()
    {
        AdmobGA_Helper.GA_Log(AdmobGAEvents.Initializing);
        
        MobileAds.Initialize((initStatus) =>
        {
            //Debug.Log("GG >> Admob:Initialized");
            AdmobGA_Helper.GA_Log(AdmobGAEvents.Initialized);


            isAdmobInitialized = true;
            LoadSmallBanner();
            LoadAppOpen();
            //CreateAdsObjects();
            //BindAdsEvent();

            //LoadInterstitial();
            //LoadVideo();
        });
       
      
           
#if UNITY_IOS
        MobileAds.SetiOSAppPauseOnBackground(true);    
#endif
    }

    //void MediationAdapterConsent(string AdapterClassname)
    //{  
    //    if (AdapterClassname.Contains("Unity"))
    //    {
    //        UnityAds.SetGDPRConsentMetaData(true);
    //        Debug.Log("GG >> UnityAds consent is send");
    //        Constant.LogDesignEvent("Admob:Consent:UnityAds");
    //    }
    //}

   

    #region Ads Events Bind
    private void BindSmallBannerEvents()
    {
        // INTERSTITIAL EVENTS...//
        this.SmallBanner.OnAdLoaded += SmallBanner_HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.SmallBanner.OnAdFailedToLoad += SmallBanner_HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.SmallBanner.OnAdOpening += SmallBanner_HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.SmallBanner.OnAdClosed += SmallBanner_HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.SmallBanner.OnAdLeavingApplication += SmallBanner_HandleOnAdLeavingApplication;
    }

    private void BindMediumBannerEvents()
    {

        // INTERSTITIAL EVENTS...//

        this.MediumBanner.OnAdLoaded += MediumBanner_HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.MediumBanner.OnAdFailedToLoad += MediumBanner_HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this.MediumBanner.OnAdOpening += MediumBanner_HandleOnAdOpened;
        // Called when the user returned from the app after an ad click.
        this.SmallBanner.OnAdClosed += MediumBanner_HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.MediumBanner.OnAdLeavingApplication += MediumBanner_HandleOnAdLeavingApplication;
    }
    private void BindIntertitialEvents()
    {

        // INTERSTITIAL EVENTS...//

        this.interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;

        // Called when an ad is shown.
        this.interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this.interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;
    }

    private void BindVideoEvents()
    {
        // VIDEO AD EVENTS...//

        this.videoAd.OnAdLoaded += Video_HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this.videoAd.OnAdFailedToLoad += Video_HandleOnAdFailedToLoad;

        // Called when an ad is shown.
        this.videoAd.OnAdOpening += Video_HandleOnAdOpened;
        // Called when the ad is closed.
        this.videoAd.OnAdClosed += Video_HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        //this.videoAd.OnAdLeavingApplication += Video_HandleOnAdLeavingApplication;
    }

    private void BindRewardedEvents()
    {
        //.....REWARDED ADS EVENTS.......//
        //// Get singleton reward based video ad reference.
        //this.rewardBasedVideo = RewardBasedVideoAd.Instance;

        // Called when an ad request has successfully loaded.
        rewardBasedVideo.OnAdLoaded += HandleRewardBasedVideoLoaded;
        // Called when an ad request failed to load.
        rewardBasedVideo.OnAdFailedToLoad += HandleRewardBasedVideoFailedToLoad;
        // Called when an ad is shown.
        rewardBasedVideo.OnAdOpening += HandleRewardBasedVideoOpened;
        // Called when the ad starts to play.
        //rewardBasedVideo.OnAdStarted += HandleRewardBasedVideoStarted;

        rewardBasedVideo.OnAdFailedToShow += HandleRewardedAdFailedToShow;


        // Called when the user should be rewarded for watching a video.
        rewardBasedVideo.OnUserEarnedReward += HandleRewardBasedVideoRewarded;
        // Called when the ad is closed.
        rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        // Called when the ad click caused the user to leave the application.
        //rewardBasedVideo.OnAdLeavingApplication += HandleRewardBasedVideoLeftApplication;
    }

    
    #endregion

    #region Load Ads
    public override void LoadSmallBanner()
    {
        if (sbAdStatus == AdLoadingStatus.Loading || PreferenceManager.GetAdStatus() == 0 || IsSmallBannerReady() ||Constants.enableAds=="0" )
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:smallBanner:LoadRequest");
            GameAnalytics.NewDesignEvent("Admob:smallBanner:LoadRequest");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadSmallBanner);

            SmallBanner = new BannerView(ADMOB_ID.ADMOB_BANNER_AD_ID, AdSize.Banner, AdPosition.Bottom);
            BindSmallBannerEvents();
            AdRequest request = new AdRequest.Builder().Build();

            SmallBanner.LoadAd(request);
            SmallBanner.Hide();
            sbAdStatus = AdLoadingStatus.Loading;
        }
    }

    public override void LoadMediumBanner()
    {
        if (mbAdStatus == AdLoadingStatus.Loading || PreferenceManager.GetAdStatus()==0 || IsMediumBannerReady() || Constants.enableAds == "0" )
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:mediumBanner:LoadRequest");
            GameAnalytics.NewDesignEvent("Admob:mediumBanner:LoadRequest");
           // AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadMediumBanner);
            AdRequest request = new AdRequest.Builder().Build();
            
            MediumBanner.LoadAd(request);
            MediumBanner.Hide();
            mbAdStatus = AdLoadingStatus.Loading;
        }
    }

    /// <summary>
    /// Load Interstitial Ad
    /// </summary>
    public override void LoadInterstitial()
    {
        if (!isAdmobInitialized || IsInterstitialAdReady() || iAdStatus == AdLoadingStatus.Loading|| PreferenceManager.GetAdStatus() == 0 || Constants.enableAds == "0")
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:iad:LoadRequest");
            GameAnalytics.NewDesignEvent("Admob:iad:LoadRequest");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadInterstitialAd);
            this.interstitial = new InterstitialAd(ADMOB_ID.ADMOB_INTERTITIAL_AD_ID);
            BindIntertitialEvents();
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.interstitial.LoadAd(request);
            iAdStatus = AdLoadingStatus.Loading;
        }
    }

    /// <summary>
    /// Load Video Ad
    /// </summary>
    public override void LoadVideo()
    {
        if (!isAdmobInitialized || IsVideoAdReady() || vAdStatus == AdLoadingStatus.Loading || PreferenceManager.GetAdStatus() == 0  || Constants.enableAds == "0")
        {
            return;
        }
        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {
            Debug.Log("GG >> Admob:vad:LoadRequest");
            GameAnalytics.NewDesignEvent("Admob:vad:LoadRequest");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadVideoAd);
            this.videoAd = new InterstitialAd(ADMOB_ID.ADMOB_VIDEO_AD_ID);
            BindVideoEvents();
            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the interstitial with the request.
            this.videoAd.LoadAd(request);
            vAdStatus = AdLoadingStatus.Loading;
        }
    }

    /// <summary>
    /// Load Rewarded Ad
    /// </summary>
    public override void LoadRewardedVideo()
    {
        if (!isAdmobInitialized || IsRewardedAdReady() || rAdStatus == AdLoadingStatus.Loading || Constants.enableAds == "0")
        {
            return;
        }

        if (Application.internetReachability == NetworkReachability.ReachableViaCarrierDataNetwork | Application.internetReachability == NetworkReachability.ReachableViaLocalAreaNetwork)
        {

            Debug.Log("GG >> Admob:rad:LoadRequest");
            GameAnalytics.NewDesignEvent("Admob:rad:LoadRequest");
            rewardBasedVideo = new RewardedAd(ADMOB_ID.ADMOB_REWARDED_AD_ID);
            BindRewardedEvents();
            //+--------AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadRewardedAd);

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded video ad with the request.
            this.rewardBasedVideo.LoadAd(request);
            rAdStatus = AdLoadingStatus.Loading;
        }

    }

    bool isSmallBannerLoaded = false;
    bool isMediumBannerLoaded = false;
    public override bool IsSmallBannerReady()
    {
        return isSmallBannerLoaded;
    }

    public bool IsMediumBannerReady()
    {
        return isMediumBannerLoaded;
    }

    public override bool IsRewardedAdReady()
    {
        if (this.rewardBasedVideo != null)
            return this.rewardBasedVideo.IsLoaded();
        return false;
    }


    #endregion

    #region Show Ads
    public override void HideSmallBannerEvent()
    {
        if (SmallBanner != null)
        {
            Debug.Log("GG >> Admob:smallBanner:Hide");
            GameAnalytics.NewDesignEvent("Admob:smallBanner:Hide");
            SmallBanner.Hide();
        }
        FindObjectOfType<AdsManager>().HideBanner();
    }
    public override void HideMediumBannerEvent()
    {
        if (MediumBanner != null)
        {
            Debug.Log("GG >> Admob:mediumBanner:Hide");
            GameAnalytics.NewDesignEvent("Admob:mediumBanner:Hide");
            MediumBanner.Hide();
        }
    }
    //public void ShowSmallBannerEvent()
    //{
    //    ShowSmallBanner(AdPosition.Top);
    //}
    bool admobBannerIsDisplayed = false;
    public override bool isAdmobBannerDisplayed()
    {
        return admobBannerIsDisplayed;
    }
    public void ShowMediumBannerEvent()
    {
        ShowMediumBanner(AdPosition.BottomLeft);
    }
    public override void ShowSmallBanner(AdPosition position)
    {
        
        if (PreferenceManager.GetAdStatus() == 0 || !isAdmobInitialized || Constants.enableAds == "0" )
        {
            return;
        }
        
        Debug.Log("GG >> Admob:smallBanner:ShowCall");
        GameAnalytics.NewDesignEvent("Admob:smallBanner:ShowCall");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowSmallBanner);
        if (SmallBanner != null && IsSmallBannerReady())
        {
            Debug.Log("GG >> Admob:smallBanner:WillDisplay");
            GameAnalytics.NewDesignEvent("Admob:smallBanner:WillDisplay");

            SmallBanner.Show();
            SmallBanner.SetPosition(position);
            admobBannerIsDisplayed = true;
        }
        else
        {
            Debug.Log("GG >> Admob:smallBanner:AdNotLoaded");
            GameAnalytics.NewDesignEvent("Admob:smallBanner:AdNotLoaded");
            LoadSmallBanner();
            admobBannerIsDisplayed = false;
        }


    }
    public override void ShowMediumBanner(AdPosition position)
    {
            
        if (PreferenceManager.GetAdStatus() == 0 || !isAdmobInitialized || Constants.enableAds == "0")
        {
            return;
        }
        Debug.Log("GG >> Admob:mediumBanner:ShowCall");
        GameAnalytics.NewDesignEvent("Admob:mediumBanner:ShowCall");
        if (MediumBanner != null)
        {
            Debug.Log("GG >> Admob:mediumBanner:WillDisplay");
            GameAnalytics.NewDesignEvent("Admob:mediumBanner:WillDisplay");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowMediumBanner);
            MediumBanner.Show();
            MediumBanner.SetPosition(position);
        }
        else
        {
            Debug.Log("GG >> Admob:mediumBanner:AdNotLoaded");
            GameAnalytics.NewDesignEvent("Admob:mediumBanner:AdNotLoaded");
            LoadMediumBanner();
        }
    }

    
    public override void ShowInterstitial()
    {
        if (Constants.enableAds == "0" || PreferenceManager.GetAttemptedLevelCount() < 2)
            return;
        Debug.Log("GG >> Admob:iad:ShowCall");
        GameAnalytics.NewDesignEvent("Admob:iad:ShowCall");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowInterstitialAd);
        if (interstitial != null)
        {
            if (interstitial.IsLoaded())
            {

                Debug.Log("GG >> Admob:iad:WillDisplay");
                GameAnalytics.NewDesignEvent("Admob:iad:WillDisplay");
                //AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdWillDisplay);
                isInterstialAdPresent = true;
                interstitial.Show();
            }
            else
            {
                LoadInterstitial();
            }
         
        }
    }
    public override bool IsInterstitialAdReady()
    {
        if (this.interstitial != null)
            return this.interstitial.IsLoaded();
        else
            return false;
    }

    public override void ShowVideo()
    {
        if (Constants.enableAds == "0")
            return;
        Debug.Log("GG >> Admob:vad:ShowCall");
        GameAnalytics.NewDesignEvent("Admob:vad:ShowCall");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowVideoAd);
        if (this.videoAd != null)
        {
            if (this.videoAd.IsLoaded())
            {

                Debug.Log("GG >> Admob:vad:WillDisplay");
                GameAnalytics.NewDesignEvent("Admob:vad:WillDisplay");
                //AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdWillDisplay);
                isInterstialAdPresent = true;
                this.videoAd.Show();


            }
            else
            {
                LoadVideo();
            }
            
        }
    }
    public override bool IsVideoAdReady()
    {
        if (this.videoAd !=null)
            return this.videoAd.IsLoaded();
        else
            return false;
    }
    
    public override void ShowRewardedVideo(RewardUserDelegate _delegate)
    {
        if (Constants.enableAds == "0" || PreferenceManager.GetAttemptedLevelCount()<2)
            return;
        Debug.Log("GG >> Admob:rad:ShowCall");
        GameAnalytics.NewDesignEvent("Admob:rad:ShowCall");
        AbmobNotifyReward = _delegate;
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.ShowRewardedAd);

        if (this.rewardBasedVideo != null && this.rewardBasedVideo.IsLoaded())
        {
            Debug.Log("GG >> Admob:rad:WillDisplay");
            GameAnalytics.NewDesignEvent("Admob:rad:WillDisplay");
            isInterstialAdPresent = true;
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdWillDisplay);
            this.rewardBasedVideo.Show();
        }      
        else
        {
            LoadRewardedVideo();
        }
    }
    public void ShowRewardedAdBtnEvent()
    {
        ShowRewardedVideo(RewardGiven);    
    }
    private void RewardGiven()
    {
        Debug.Log("Reward is Given");
    }
   
#endregion

    #region Intertitial Add Handler


    //******Intertitial Ad Handlers**********//
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            iAdStatus = AdLoadingStatus.Loaded;
            Debug.Log("GG >> Admob:iad:Loaded");
            GameAnalytics.NewDesignEvent("Admob:iad:Loaded");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdLoaded);
        });
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            iAdStatus = AdLoadingStatus.NoInventory;
            Debug.Log("GG >> Admob:iad:NoInventory :: " + args.ToString()); 
            GameAnalytics.NewDesignEvent("Admob:iad:NoInventory:: " + args.ToString());
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdNoInventory);
        });
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            iAdStatus = AdLoadingStatus.NotLoaded;
            Debug.Log("GG >> Admob:iad:Displayed ");
            GameAnalytics.NewDesignEvent("Admob:iad:Displayed");
            Constants.isAdPlaying = true;
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdDisplayed);

        });
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        this.interstitial.Destroy();
        

        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            Debug.Log("GG >> Admob:iad:Closed");
            GameAnalytics.NewDesignEvent("Admob:iad:Closed");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdClosed);
            Constants.isAdPlaying = false;
            iAdStatus = AdLoadingStatus.NotLoaded;
            //OnInterstitialClosed();
            
        });
    }

    //public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    //{
    //    MobileAdsEventExecutor.ExecuteInUpdate(() =>
    //    {
    //        Debug.Log("GG >> Admob:iad:Clicked");
    //        AdmobGA_Helper.GA_Log(AdmobGAEvents.InterstitialAdClicked);
    //    });
    //}

    #endregion

    #region Small Banner Add Handler

        public void SmallBanner_HandleOnAdLoaded(object sender, EventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                sbAdStatus = AdLoadingStatus.Loaded;
                Debug.Log("GG >> Admob:smallBanner:Loaded");
                GameAnalytics.NewDesignEvent("Admob:smallBanner:Loaded");
                //AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerLoaded);
                isSmallBannerLoaded = true;
            });
        }

        public void SmallBanner_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                sbAdStatus = AdLoadingStatus.NoInventory;
                Debug.Log("GG >> Admob:smallBanner:NoInventory :: " + args.ToString());
                GameAnalytics.NewDesignEvent("Admob:smallBanner:NoInventory:: " + args.ToString());
                //bannerIsDisplayed = false;
                //AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerNoInventory);
                isSmallBannerLoaded = false;
            });
        }

        public void SmallBanner_HandleOnAdOpened(object sender, EventArgs args)
        {
            MobileAdsEventExecutor.ExecuteInUpdate(() =>
            {
                sbAdStatus = AdLoadingStatus.NotLoaded;
                Debug.Log("GG >> Admob:smallBanner:Displayed");
                //bannerIsDisplayed = true;
                GameAnalytics.NewDesignEvent("Admob:smallBanner:Displayed");
                //AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerDisplayed);
            });
        }
        public void SmallBanner_HandleOnAdClosed(object sender, EventArgs args)
        {
            Debug.Log("GG >> Admob:smallBanner:Closed");
            GameAnalytics.NewDesignEvent("Admob:smallBanner:Closed");
        //bannerIsDisplayed=false;
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerClosed);
    }
        public void SmallBanner_HandleOnAdLeavingApplication(object sender, EventArgs args)
        {
            Debug.Log("GG >> Admob:smallBanner:Clicked");
            GameAnalytics.NewDesignEvent("Admob:smallBanner:Clicked");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.SmallBannerClicked);
        }


    #endregion

    #region Medium Banner Handler
    public void MediumBanner_HandleOnAdLoaded (object sender, EventArgs args)
    {
        Debug.Log("GG >> Admob:mediumBanner:NoInventory :: " + args.ToString());
        GameAnalytics.NewDesignEvent("Admob:mediumBanner:NoInventory :: " + args.ToString());
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.LoadMediumBanner);
        isMediumBannerLoaded = false;
    }

    public void MediumBanner_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("GG >> Admob:mediumBanner:NoInventory :: " + args.ToString());
        GameAnalytics.NewDesignEvent("Admob:mediumBanner:NoInventory :: " + args.ToString());
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerNoInventory);
        isMediumBannerLoaded = false;
    }

    public void MediumBanner_HandleOnAdOpened(object sender, EventArgs args)
    {
        Debug.Log("GG >> Admob:mediumBanner:Displayed");
        GameAnalytics.NewDesignEvent("Admob:mediumBanner:Displayed");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerDisplayed);
    }

    public void MediumBanner_HandleOnAdClosed(object sender, EventArgs args)
    {
        Debug.Log("GG >> Admob:mediumBanner:Closed");
        GameAnalytics.NewDesignEvent("Admob:mediumBanner:Closed");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerClosed);
    }

    public void MediumBanner_HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        Debug.Log("GG >> Admob:mediumBanner:Clicked");
        GameAnalytics.NewDesignEvent("Admob:mediumBanner:Clicked");
        //AdmobGA_Helper.GA_Log(AdmobGAEvents.MediumBannerClicked);
    }

    #endregion

    #region Video Ad Handlers

    public void Video_HandleOnAdLoaded(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            vAdStatus = AdLoadingStatus.Loaded;
            Debug.Log("GG >> Admob:vad:Loaded");
            GameAnalytics.NewDesignEvent("Admob:vad:Loaded");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdLoaded);
        });
    }

    public void Video_HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            vAdStatus = AdLoadingStatus.NoInventory;
            Debug.Log("GG >> Admob:vad:NoInventory :: " + args.ToString());
            GameAnalytics.NewDesignEvent("Admob:vad:NoInventory :: " + args.ToString());
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdNoInventory);
        });
    }


    public void Video_HandleOnAdOpened(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            vAdStatus = AdLoadingStatus.NotLoaded;
            Debug.Log("GG >> Admob:vad:Displayed");
            GameAnalytics.NewDesignEvent("Admob:vad:Displayed");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdDisplayed);
            Constants.isAdPlaying = true;
        });
    }

    public void Video_HandleOnAdClosed(object sender, EventArgs args)
    {
        
        this.videoAd.Destroy();
        
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            vAdStatus = AdLoadingStatus.NotLoaded;
            Debug.Log("GG >> Admob:vad:Closed");
            GameAnalytics.NewDesignEvent("Admob:vad:Closed");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.VideoAdClosed);
            Constants.isAdPlaying = false;
            
        });
    }

    #endregion

    #region Rewarded Ad Handlers

    public void HandleRewardBasedVideoLoaded(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.Loaded;
            Debug.Log("GG >> Admob:rad:Loaded");
            GameAnalytics.NewDesignEvent("Admob:rad:Loaded");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdLoaded);
        });
    }

    public void HandleRewardBasedVideoFailedToLoad(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NoInventory;
            Debug.Log("GG >> Admob:rad:NoInventory :: " + args.ToString());
            GameAnalytics.NewDesignEvent("Admob:rad:NoInventory :: " + args.ToString());
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdNoInventory);
        });

    }
    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            Debug.Log("GG >> Admob:rad:FailedToShow");
            GameAnalytics.NewDesignEvent("Admob:rad:FailedToShow");
        });
    }
    public void HandleRewardBasedVideoOpened(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            Debug.Log("GG >> Admob:rad:Displayed");
            GameAnalytics.NewDesignEvent("Admob:rad:Displayed");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdDisplayed);
        });
        
    }

    public void HandleRewardBasedVideoStarted(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            Debug.Log("GG >> Admob:rad:Started");
            GameAnalytics.NewDesignEvent("Admob:rad:Started");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdStarted);
        });
    }

    public void HandleRewardBasedVideoClosed(object sender, EventArgs args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            rAdStatus = AdLoadingStatus.NotLoaded;
            Debug.Log("GG >> Admob:rad:Closed");
            GameAnalytics.NewDesignEvent("Admob:rad:Closed");
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdClosed);
            //if (Constants.isUnlockPanelClosed)
            //    EventManager.DoFireUnlockPanelClosed();
            //this.rewardBasedVideo = new RewardedAd(ADMOB_ID.ADMOB_REWARDED_AD_ID);
            //BindRewardedEvents();
            LoadRewardedVideo();
        });
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            Debug.Log("GG >> give reward to user after watching rAd");
            GameAnalytics.NewDesignEvent("Admob:rad:give reward to user after watching rAd");
            
            AbmobNotifyReward();
            //AdmobGA_Helper.GA_Log(AdmobGAEvents.RewardedAdReward);
        });
    }

    #endregion
    
}
