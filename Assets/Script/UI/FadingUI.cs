using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public enum FadingPhase
{
    FadeIn,
    FadeOut
}

public class FadingUI : MonoBehaviour
{
    private static FadingUI _instance;

    public static FadingUI Instance
    {
        get
        {
            if (_instance == null && Application.isPlaying)
            {
                GameObject obj = new GameObject("FadingUI");
                FadingUI comp = obj.AddComponent<FadingUI>();
                _instance = comp;
            }

            return _instance;
        }
    }

    [SerializeField]
    private Image fadeImage;

    [SerializeField]
    private float fadingSpeed;

    public UnityEvent OnStopFading;

    private bool fading;
    private FadingPhase currentFadingPhase;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(this.gameObject);
        OnActiveSceneChanged();
    }

    private void Update()
    {
        if (fading) 
        {
            if (currentFadingPhase == FadingPhase.FadeIn)
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a + Time.deltaTime * fadingSpeed);
                
                if (fadeImage.color.a >= 1) 
                {
                    StopFading();
                }
            }
            else
            {
                fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, fadeImage.color.a - Time.deltaTime * fadingSpeed);

                if (fadeImage.color.a <= 0)
                {
                    StopFading();
                }
            }
        }
    }

    public void StartFadeIn() 
    {
        fadeImage.gameObject.SetActive(true);
        fading = true;
        currentFadingPhase = FadingPhase.FadeIn;
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 0);
    }

    public void StartFadeOut()
    {
        fadeImage.gameObject.SetActive(true);
        fading = true;
        currentFadingPhase = FadingPhase.FadeOut;
        fadeImage.color = new Color(fadeImage.color.r, fadeImage.color.g, fadeImage.color.b, 1);
    }

    private void StopFading() 
    {
        fading = false;
        OnStopFading?.Invoke();
        OnStopFading.RemoveAllListeners();
    }

    private void OnActiveSceneChanged() 
    {
        SceneManager.activeSceneChanged += DisactiveFadeImage;
    }

    private void DisactiveFadeImage(Scene current, Scene next) 
    {
        StartFadeOut();
    }
}
