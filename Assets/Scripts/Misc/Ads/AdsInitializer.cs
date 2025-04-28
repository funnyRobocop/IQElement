using UnityEngine;

public class AdsInitializer : MonoBehaviour//, IUnityAdsInitializationListener
{

    /*public Action OnInit;
    [SerializeField] private string _gameId;
    [SerializeField] bool _testMode;

    void Start()
    {
        Advertisement.Initialize(_gameId, _testMode, this);
    }
 
    public void CheckInit()
    {
        if (!Advertisement.isSupported)
            return;

        if (Advertisement.isInitialized)
        {
            OnInit?.Invoke();
        }
        else
        {
            Advertisement.Initialize(_gameId, _testMode, this);
        }
    }
 
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        OnInit?.Invoke();
    }
 
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.Log($"Unity Ads Initialization Failed: {error.ToString()} - {message}");
    }*/
}