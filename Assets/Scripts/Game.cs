using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;


public class Game : MonoBehaviour
{
    // singleton class:
    public static Game Instance;

    [HideInInspector] public List<Route> readyRoutes = new();

    private int totalRoutes;
    private int successfulParks;
    private int scoreForScene;
    //events:
    public UnityAction<Route> OnCarEntersPark;
    public UnityAction OnCarCollision;
    public UnityAction ShowScore;
    public UnityAction ShowInterstitalAd;
    public UnityAction ShowRewardedAd;
    public UnityAction GameFinished;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

        // double -> float dönüþümü yapýlýr
        float refreshRate = (float)Screen.currentResolution.refreshRateRatio.value;

        Application.targetFrameRate = Mathf.RoundToInt(refreshRate);



        totalRoutes = transform.GetComponentsInChildren<Route>().Length;
        successfulParks = 0;


        OnCarEntersPark += OnCarEntersParkHandler;
        OnCarCollision += OnCarCollisionHandler;
    }

    private void OnCarCollisionHandler()
    {
        Debug.Log("GAME OVER!");

        DOVirtual.DelayedCall(2F, () =>
        {
            int currentLevel = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevel);
        });
    }

    private void OnCarEntersParkHandler(Route route)
    {
        route.car.StopDancingAnim();
        successfulParks++;

        if (successfulParks == totalRoutes)
        {
            Debug.Log("GOOD JOB!");
            int nextLevel = SceneManager.GetActiveScene().buildIndex + 1;

            //FindObjectOfType<UIManager>().ScoreSuccessful();
            ShowScore?.Invoke();
            DOVirtual.DelayedCall(1.3f, () =>
            {

                if (nextLevel < SceneManager.sceneCountInBuildSettings)
                {
                    SceneManager.LoadScene(nextLevel);
                }
                else
                {
                    Debug.LogWarning("No Next Level Available!");
                    ShowRewardedAd?.Invoke();
                    DOVirtual.DelayedCall(3f, () =>
                    {
                        GameFinished?.Invoke();
                    });
                }
            });
        }
    }

    public void RegisterRoute(Route route)
    {
        readyRoutes.Add(route);

        if (readyRoutes.Count == totalRoutes)
        {
            MoveAllCars();
        }
    }

    private void MoveAllCars()
    {
        foreach (var route in readyRoutes)
        {
            {
                route.car.CarMove(route.linePoints);
            }
        }
    }
}