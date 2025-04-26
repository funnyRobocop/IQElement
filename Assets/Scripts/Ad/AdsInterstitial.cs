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

public class AdsInterstitial : MonoBehaviour
{
    private String message = "";
    private float timer;
    private float timerInterval = 60f;

    private InterstitialAdLoader interstitialAdLoader;
    private Interstitial interstitial;

    public static AdsInterstitial Instance;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(this);

        interstitialAdLoader = new InterstitialAdLoader();
        interstitialAdLoader.OnAdLoaded += HandleAdLoaded;
        interstitialAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
    }

    public void Start()
    {
        //RequestInterstitial();
    }

    public void Update()
    {
        timer += Time.deltaTime;
        if (timer > timerInterval)
        {
            if (interstitial == null)
                RequestInterstitial();
            timer = 0f;
        }
    }

    private void RequestInterstitial()
    {
        //Sets COPPA restriction for user age under 13
        MobileAds.SetAgeRestrictedUser(true);

        string adUnitId = "R-M-15275443-1";
        //string adUnitId = "demo-interstitial-yandex";

        if (interstitial != null)
        {
            interstitial.Destroy();
        }

        interstitialAdLoader.LoadAd(CreateAdRequest(adUnitId));
        DisplayMessage("Interstitial is requested");
    }

    public void ShowInterstitial()
    {
        if (interstitial == null)
        {
            DisplayMessage("Interstitial is not ready yet");
            return;
        }

        interstitial.OnAdClicked += HandleAdClicked;
        interstitial.OnAdShown += HandleAdShown;
        interstitial.OnAdFailedToShow += HandleAdFailedToShow;
        interstitial.OnAdImpression += HandleImpression;
        interstitial.OnAdDismissed += HandleAdDismissed;

        interstitial.Show();
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

    #region Interstitial callback handlers

    public void HandleAdLoaded(object sender, InterstitialAdLoadedEventArgs args)
    {
        DisplayMessage("HandleAdLoaded event received");

        interstitial = args.Interstitial;
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        DisplayMessage($"HandleAdFailedToLoad event received with message: {args.Message}");
    }
    public void HandleAdClicked(object sender, EventArgs args)
    {
        DisplayMessage("HandleAdClicked event received");

        if (interstitial != null)
            interstitial.Destroy();
        interstitial = null;
    }

    public void HandleAdShown(object sender, EventArgs args)
    {
        DisplayMessage("HandleAdShown event received");

        if (interstitial != null)
            interstitial.Destroy();
        interstitial = null;
    }

    public void HandleAdDismissed(object sender, EventArgs args)
    {
        DisplayMessage("HandleAdDismissed event received");

        if (interstitial != null)
            interstitial.Destroy();
        interstitial = null;
    }

    public void HandleImpression(object sender, ImpressionData impressionData)
    {
        var data = impressionData == null ? "null" : impressionData.rawData;
        DisplayMessage($"HandleImpression event received with data: {data}");
    }

    public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
    {
        this.DisplayMessage($"HandleAdFailedToShow event received with message: {args.Message}");
        
        if (interstitial != null)
            interstitial.Destroy();
        interstitial = null;
    }

    #endregion
}
