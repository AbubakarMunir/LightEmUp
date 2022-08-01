using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
public enum AdLoadingStatus
{
    NotLoaded,
    Loading,
    Loaded,
    NoInventory
}

public abstract class MediationHandler : MonoBehaviour
{

    public delegate void RewardUserDelegate();

    public static AdLoadingStatus iAdStatus = AdLoadingStatus.NotLoaded;

    public static AdLoadingStatus vAdStatus = AdLoadingStatus.NotLoaded;

    public static AdLoadingStatus rAdStatus = AdLoadingStatus.NotLoaded, sbAdStatus, mbAdStatus;
    public abstract void LoadSmallBanner();
    public abstract void HideSmallBannerEvent();
    public abstract void ShowSmallBanner(AdPosition position);
    public abstract void LoadMediumBanner();
    public abstract void HideMediumBannerEvent();
    public abstract void ShowMediumBanner(AdPosition position);
    public abstract bool IsRewardedAdReady();
    public abstract bool IsVideoAdReady();
    public abstract bool IsInterstitialAdReady();
    public abstract void LoadInterstitial();
    public abstract void LoadVideo();

    public abstract bool isAdmobBannerDisplayed();
    public abstract bool IsSmallBannerReady();
    public void LoadAd()
    {
        if (Constants.showInterstitial)
        {
            LoadInterstitial();

        }
        else
        {
            LoadVideo();
        }
    }

    public void ShowAd()
    {
        if (Constants.showInterstitial)
        {
            Constants.showInterstitial = false;
            ShowInterstitial();
        }
        else
        {
            Constants.showInterstitial = true;
            ShowVideo();
        }
    }

    public bool isAdReady()
    {
        if (Constants.showInterstitial)
        {
            return IsInterstitialAdReady();
        }
        else
        {
            return IsVideoAdReady();
        }
    }
    public abstract void LoadRewardedVideo();

    public abstract void ShowInterstitial();
    public abstract void ShowVideo();
    public abstract void ShowRewardedVideo(RewardUserDelegate _delegate);

    public abstract void ShowAppOpenAd();


}
