using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using System;

//[RequireComponent(typeof(AdsInitializer))]
public class RewardedAds : MonoBehaviour//, IUnityAdsLoadListener, IUnityAdsShowListener
{

    /*public Action OnUnityAdsShowCompleted;

    private AdsInitializer adsInitializer;
    [SerializeField] private Button button;
    [SerializeField] private string adUnitId = "Rewarded_Android";

    void Awake()
    {
        adsInitializer = gameObject.GetComponent<AdsInitializer>();
        adsInitializer.OnInit += LoadAd;
    }
    void OnDestroy()
    {
        adsInitializer.OnInit -= LoadAd;
        OnUnityAdsShowCompleted = null;

        if (button != null)
            button.onClick.RemoveAllListeners();
    }

    public void Init()
    {
        if (button != null)
        {
            button.gameObject.SetActive(false);
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(ShowAd);
        }

        adsInitializer.CheckInit();
    }

    public void LoadAd()
    {
        if (MainGameScript.currentLevel != MainGameScript.openedLevel)
            return;

        if (button != null)
            button.gameObject.SetActive(true);
 
        Advertisement.Load(adUnitId, this);
        Debug.Log("Loading Ad: " + adUnitId);
    }

    public void ShowAd()
    {
        if (button != null)
            button.gameObject.SetActive(false);
            
        Advertisement.Show(adUnitId, this);
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (button != null)
            button.gameObject.SetActive(true);
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message) 
    { 
        Debug.Log($"Error loading Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }

    public void OnUnityAdsShowClick(string placementId) {  }
    public void OnUnityAdsShowStart(string placementId) { }

    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (adUnitId.Equals(adUnitId) && showCompletionState.Equals(UnityAdsShowCompletionState.COMPLETED))
        {
            OnUnityAdsShowCompleted?.Invoke();
            Debug.Log("Unity Ads Rewarded Ad Completed");
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message) 
    {
        Debug.Log($"Error showing Ad Unit {adUnitId}: {error.ToString()} - {message}");
    }*/
}
