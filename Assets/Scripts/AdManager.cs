using GoogleMobileAds.Api;
using System;
using UnityEngine;
using UnityEngine.Events;

public class AdManager : MonoBehaviour
{
    //private static AdManager _instance;

    // Public property to access the singleton instance

    public static AdManager Instance;
    // These ad units are configured to always serve test ads.
#if UNITY_ANDROID
    private string _adUnitId = "ca-app-pub-3940256099942544/1033173712";
    //private string _adUnitId = "ca-app-pub-7068722663835083/6739917137";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string _adUnitId = "unused";
#endif
#if UNITY_ANDROID
    //private string _adUnitIdRewarded = "ca-app-pub-7068722663835083/1703450556"; //real ID
    private string _adUnitIdRewarded = "ca-app-pub-3940256099942544/5224354917";
#elif UNITY_IPHONE
  private string _adUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
    private string _adUnitIdRewarded = "unused";
#endif
    public UnityAction AdWatched;
    private RewardedAd _rewardedAd;
    private InterstitialAd _interstitialAd;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            LoadRewardedAd();
        });

        if (Game.Instance != null)
        {
            Game.Instance.ShowInterstitalAd += LoadAndShowInterstitialAd;
            Game.Instance.ShowRewardedAd += LoadAndShowRewardedAd;
        }
    }
    private void Awake()
    {
        Instance = this;
    }
    /*public static AdManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find the instance in the scene if it's not already set
                _instance = FindObjectOfType<AdManager>();

                if (_instance == null)
                {
                    // If no instance found, create a new one
                    GameObject go = new GameObject("AdManager");
                    _instance = go.AddComponent<AdManager>();
                }
            }
            return _instance;
        }
    }

    // Ensure only one instance exists by using Awake
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);  // Destroy duplicate
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);  // Optional: keep the singleton alive across scenes
        }
    }*/
    public void LoadInterstitialAd()
    {
        // Clean up the old ad before loading a new one.
        if (_interstitialAd != null)
        {
            _interstitialAd.Destroy();
            _interstitialAd = null;
        }

        Debug.Log("Loading the interstitial ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        InterstitialAd.Load(_adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;
            });
    }

    public void ShowInterstitialAd()
    {
        if (_interstitialAd != null && _interstitialAd.CanShowAd())
        {
            Debug.Log("Showing interstitial ad.");
            _interstitialAd.Show();
        }
        else
        {
            Debug.LogError("Interstitial ad is not ready yet.");
        }
    }

    public void LoadAndShowInterstitialAd()
    {
        LoadInterstitialAd();
        ShowInterstitialAd();
    }
    public void LoadRewardedAd()
    {
        // Clean up the old ad before loading a new one.
        if (_rewardedAd != null)
        {
            _rewardedAd.Destroy();
            _rewardedAd = null;
        }

        Debug.Log("Loading the rewarded ad.");

        // create our request used to load the ad.
        var adRequest = new AdRequest();

        // send the request to load the ad.
        RewardedAd.Load(_adUnitIdRewarded, adRequest,
            (RewardedAd ad, LoadAdError error) =>
            {
                // if error is not null, the load request failed.
                if (error != null || ad == null)
                {
                    Debug.LogError("Rewarded ad failed to load an ad " +
                                   "with error : " + error);
                    return;
                }

                Debug.Log("Rewarded ad loaded with response : "
                          + ad.GetResponseInfo());

                _rewardedAd = ad;
                RegisterEventHandlers(_rewardedAd);


            });
    }
    public void ShowRewardedAd()
    {
        if (_rewardedAd != null && _rewardedAd.CanShowAd())
        {
            _rewardedAd.Show((Reward reward) =>
            {
                Debug.Log("User earned reward: " + reward.Amount);
                // Ödülü ver
            });
        }
        else
        {
            Debug.Log("Rewarded ad is not ready yet.");
            // Reklam hazýr deðilse yeniden yükleyebilirsin
            LoadRewardedAd();
        }
    }
    public void LoadAndShowRewardedAd()
    {
        ShowRewardedAd();
    }
    private void RegisterEventHandlers(RewardedAd ad)
    {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) =>
        {
            Debug.Log(String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () =>
        {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () =>
        {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () =>
        {
            Debug.Log("Rewarded ad full screen content opened.");
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () =>
        {
            Debug.Log("Rewarded ad full screen content closed.");
            //SceneManager.LoadScene(0);
            AdWatched?.Invoke();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) =>
        {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);
        };
    }
}
