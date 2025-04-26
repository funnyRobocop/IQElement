/*
 * This file is a part of the Yandex Advertising Network
 *
 * Version for Android (C) 2023 YANDEX
 *
 * You may not use this file except in compliance with the License.
 * You may obtain a copy of the License at https://legal.yandex.com/partner_ch/
 */

using System;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

public class AdsRewarded : MonoBehaviour
{

    public Action OnLoaded;
    public Action OnSuccess;
    public Action OnFail;

    private String message = "";

    private RewardedAdLoader rewardedAdLoader;

    private RewardedAd rewardedAd;

    public static AdsRewarded Instance;


    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        rewardedAdLoader = new RewardedAdLoader();
        rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
        rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    public void Start()
    {
        //RequestRewardedAd();
    }

    public void RequestRewardedAd()
    {
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        if (rewardedAd != null)
        {
            rewardedAd.Destroy();
        }

        string adUnitId = "R-M-15275443-2";
        //adUnitId = "demo-rewarded-yandex";
        
        rewardedAdLoader.LoadAd(CreateAdRequest(adUnitId));
        DisplayMessage("Rewarded Ad is requested");
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd == null)
        {
            DisplayMessage("RewardedAd is not ready yet");
            return;
        }

        rewardedAd.OnAdClicked += HandleAdClicked;
        rewardedAd.OnAdShown += HandleAdShown;
        rewardedAd.OnAdFailedToShow += HandleAdFailedToShow;
        rewardedAd.OnAdImpression += HandleImpression;
        rewardedAd.OnAdDismissed += HandleAdDismissed;
        rewardedAd.OnRewarded += HandleRewarded;

        rewardedAd.Show();
    }

    private AdRequestConfiguration CreateAdRequest(string adUnitId)
    {
        return new AdRequestConfiguration.Builder(adUnitId).Build();
    }

    private void DisplayMessage(String message)
    {
        this.message = message + (this.message.Length == 0 ? "" : "\n--------\n" + this.message);
        MonoBehaviour.print(message);
    }

    #region Rewarded Ad callback handlers

    public void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
    {
        DisplayMessage("HandleAdLoaded event received");
        rewardedAd = args.RewardedAd;

        if (OnLoaded != null)
            OnLoaded();
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DisplayMessage($"HandleAdFailedToLoad event received with message: {args.Message}");
    }

    public void HandleAdClicked(object sender, EventArgs args)
    {
        DisplayMessage("HandleAdClicked event received");
    }

    public void HandleAdShown(object sender, EventArgs args)
    {
        DisplayMessage("HandleAdShown event received");
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        DisplayMessage("HandleAdDismissed event received");

        if (rewardedAd!= null)
            rewardedAd.Destroy();
        rewardedAd = null;
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        DisplayMessage($"HandleImpression event received with data: {data}");
    }

    public void HandleRewarded(object sender, Reward args)
    {
        DisplayMessage($"HandleRewarded event received: amout = {args.amount}, type = {args.type}");

        if (rewardedAd!= null)
            rewardedAd.Destroy();
        rewardedAd = null;

        if (OnSuccess != null)
            OnSuccess();
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        DisplayMessage($"HandleAdFailedToShow event received with message: {args.Message}");
            
        if (rewardedAd!= null)
            rewardedAd.Destroy();
        rewardedAd = null;

        if (OnFail != null)
            OnFail();
    }

    #endregion
}
