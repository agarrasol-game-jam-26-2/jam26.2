using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance => instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);

            if (fadeCanvasGroup == null)
                CreateFadePanel();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    private void CreateFadePanel()
    {
        GameObject canvasObj = new GameObject("FadePanel");
        canvasObj.transform.SetParent(transform);
        canvasObj.transform.localPosition = Vector3.zero;

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        canvasObj.AddComponent<CanvasScaler>();

        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.anchorMin = Vector2.zero;
        canvasRect.anchorMax = Vector2.one;
        canvasRect.offsetMin = Vector2.zero;
        canvasRect.offsetMax = Vector2.zero;

        GameObject imageObj = new GameObject("Image");
        imageObj.transform.SetParent(canvasObj.transform);
        imageObj.transform.localPosition = Vector3.zero;

        Image image = imageObj.AddComponent<Image>();
        image.color = new Color(0f, 0f, 0f, 1f);

        RectTransform imageRect = imageObj.GetComponent<RectTransform>();
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;

        fadeCanvasGroup = imageObj.AddComponent<CanvasGroup>();
    }

    public void TransitionToScene(string sceneName, float fadeDuration = 1f, Animator optionalAnimator = null, string optionalAnimTrigger = null, float animWaitTime = 0f)
    {
        StartCoroutine(TransitionCoroutine(sceneName, fadeDuration, optionalAnimator, optionalAnimTrigger, animWaitTime));
    }

    private IEnumerator TransitionCoroutine(string sceneName, float fadeDuration, Animator optionalAnimator, string optionalAnimTrigger, float animWaitTime)
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("[SceneTransitionManager] CanvasGroup não atribuído!");
            yield break;
        }

        if (optionalAnimator != null && !string.IsNullOrEmpty(optionalAnimTrigger))
        {
            optionalAnimator.SetTrigger(optionalAnimTrigger);
            if (animWaitTime > 0f)
                yield return new WaitForSeconds(animWaitTime);
        }

        fadeCanvasGroup.blocksRaycasts = true;

        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 1f;

        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
            yield return null;

        elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeCanvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            yield return null;
        }
        fadeCanvasGroup.alpha = 0f;

        fadeCanvasGroup.blocksRaycasts = false;
    }
}
