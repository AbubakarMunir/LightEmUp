using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameAnalyticsSDK;

public class GameanalyticsInit : MonoBehaviour, IGameAnalyticsATTListener
{
    private void OnEnable()
    {
        GameAnalytics.OnRemoteConfigsUpdatedEvent += MyOnRemoteConfigsUpdateFunction;
    }
    // Start is called before the first frame update
    private void Start()
    {
        if (Application.platform == RuntimePlatform.IPhonePlayer)
        {
            GameAnalytics.RequestTrackingAuthorization(this);
        }
        else
        {
            Initialize();
        }
    }

    public void GameAnalyticsATTListenerNotDetermined()
    {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerRestricted()
    {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerDenied()
    {
        GameAnalytics.Initialize();
    }
    public void GameAnalyticsATTListenerAuthorized()
    {
        GameAnalytics.Initialize();
    }
    public static void Initialize()
    {
        //Need to get the conset flow on iOS before launch.
        if (Application.platform != RuntimePlatform.IPhonePlayer)
        {
            GameAnalytics.Initialize();
        }
       // LogFirstSessionEvent();
    }

    private static void MyOnRemoteConfigsUpdateFunction()
    {
        
        Constants._MrecEnabled = GameAnalytics.GetRemoteConfigsValueAsString("mrecEnabled", "0");
        Constants.enableAds= GameAnalytics.GetRemoteConfigsValueAsString("enableAds", "0");

    }
    private void OnDisable()
    {
        GameAnalytics.OnRemoteConfigsUpdatedEvent -= MyOnRemoteConfigsUpdateFunction;
    }
}
