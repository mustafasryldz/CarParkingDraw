using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class UIManager : MonoBehaviour
{
    [SerializeField] LinesDrawer linesDrawer;

    [Space]
    [SerializeField] private CanvasGroup availableLineCanvasGroup;
    [SerializeField] private GameObject availableLineHolder;
    [SerializeField] private Image availableLineFill;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreTextEnd;
    [SerializeField] private GameObject reloadButton;
    [SerializeField] private GameObject endMenu;
    [SerializeField] private GameObject adWatchedText;
    [SerializeField] private GameObject goMenuButton;

    private bool isAvaiableLineUIActive = false;

    [Space]
    [SerializeField] Image fadePanel;
    [SerializeField] float fadeDuration;

    private Route activeRoute;
    private int score;
    private int totalScore = 0;
    private void Start()
    {
        fadePanel.DOFade(0f, fadeDuration).From(1f);


        availableLineCanvasGroup.alpha = 0f;



        linesDrawer.OnBeginDraw += OnBeginDrawHandler;
        linesDrawer.OnDraw += OnDrawHandler;
        linesDrawer.OnEndDraw += OnEndDrawHandler;


        Game.Instance.ShowScore += ScoreSuccessful;
        Game.Instance.GameFinished += GameFinished;

        if (AdManager.Instance != null)
        {
            Debug.Log("ad heere");
            AdManager.Instance.AdWatched += AdWatchedTextActivate;
        }
        if (AdManager.Instance == null)
        {
            Debug.Log("ad not heere");

        }

    }

    private void OnBeginDrawHandler(Route route)
    {
        activeRoute = route;

        availableLineFill.color = activeRoute.carColor;
        availableLineFill.fillAmount = 1f;
        availableLineCanvasGroup.DOFade(1f, .3f).From(0f);
        isAvaiableLineUIActive = true;
    }

    private void OnDrawHandler()
    {
        if (isAvaiableLineUIActive)
        {
            float maxLineLength = activeRoute.maxLineLength;
            float lineLength = activeRoute.line.length;

            availableLineFill.fillAmount = 1 - (lineLength / maxLineLength);
            score = Mathf.RoundToInt((maxLineLength / lineLength) * 100);
            scoreText.text = score.ToString();

        }
    }

    private void OnEndDrawHandler()
    {
        if (isAvaiableLineUIActive)
        {
            isAvaiableLineUIActive = false;
            activeRoute = null;

            availableLineCanvasGroup.DOFade(0f, .3f).From(1f);
            totalScore += score;
            score = 0;
        }
    }
    public void ScoreSuccessful()
    {
        scoreTextEnd.gameObject.SetActive(true);
        scoreTextEnd.text = totalScore.ToString() + " !";
        reloadButton.gameObject.SetActive(false);

        if (goMenuButton != null)
        {
            goMenuButton.gameObject.SetActive(false);
        }
    }
    public void GameFinished()
    {
        if (endMenu != null)
        {
            endMenu.gameObject.SetActive(true);
        }

    }

    public void AdWatchedTextActivate()
    {
        if (adWatchedText != null)
        {
            adWatchedText.SetActive(true);
        }
        Debug.Log("adwatched");
    }
}
