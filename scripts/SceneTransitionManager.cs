using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneTransitionManager : MonoBehaviour
{
    private static SceneTransitionManager instance;
    public static SceneTransitionManager Instance => instance;

    [Header("UI Settings")]
    [SerializeField] private CanvasGroup fadeCanvasGroup;

    private bool isTransitioning = false;

    private void Awake()
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

    private void Start()
    {
        if (fadeCanvasGroup != null)
        {
            fadeCanvasGroup.alpha = 0f;
            fadeCanvasGroup.blocksRaycasts = false;
        }
    }

    public void TransitionToScene(
        string sceneName, 
        float fadeDuration = 1f, 
        Animator optionalAnimator = null, 
        string optionalAnimTrigger = null, 
        float animWaitTime = 0f)
    {
        // Impede que a transição seja chamada mais de uma vez ao mesmo tempo
        if (isTransitioning) return;

        StartCoroutine(TransitionCoroutine(sceneName, fadeDuration, optionalAnimator, optionalAnimTrigger, animWaitTime));
    }

    private IEnumerator TransitionCoroutine(
        string sceneName, 
        float fadeDuration, 
        Animator optionalAnimator, 
        string optionalAnimTrigger, 
        float animWaitTime)
    {
        if (fadeCanvasGroup == null)
        {
            Debug.LogError("[SceneTransitionManager] CanvasGroup não atribuído!");
            yield break;
        }

        isTransitioning = true;
        fadeCanvasGroup.blocksRaycasts = true;

        // 1. Executa Animação Opcional (se houver)
        if (optionalAnimator != null && !string.IsNullOrEmpty(optionalAnimTrigger))
        {
            optionalAnimator.SetTrigger(optionalAnimTrigger);
            if (animWaitTime > 0f)
                yield return new WaitForSecondsRealtime(animWaitTime); // Garante funcionamento em pause
        }

        // 2. Fade Out (Tela fica preta)
        yield return StartCoroutine(FadeRoutine(0f, 1f, fadeDuration));

        // 3. Carregamento Assíncrono da Cena
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(sceneName);
        while (!loadOperation.isDone)
            yield return null;

        // Espera 1 frame para garantir que os scripts Awake/Start da nova cena inicializem
        yield return null; 

        // 4. Fade In (Tela volta a ficar visível)
        yield return StartCoroutine(FadeRoutine(1f, 0f, fadeDuration));

        fadeCanvasGroup.blocksRaycasts = false;
        isTransitioning = false;
    }

    private IEnumerator FadeRoutine(float startAlpha, float targetAlpha, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            // Time.unscaledDeltaTime permite transição mesmo com Time.timeScale = 0 (jogo pausado)
            elapsedTime += Time.unscaledDeltaTime; 
            fadeCanvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            yield return null;
        }
        fadeCanvasGroup.alpha = targetAlpha;
    }

    private void CreateFadePanel()
    {
        GameObject canvasObj = new GameObject("FadePanel");
        canvasObj.transform.SetParent(transform);

        Canvas canvas = canvasObj.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 999;

        canvasObj.AddComponent<CanvasScaler>();
        canvasObj.AddComponent<GraphicRaycaster>(); // Importante para gerenciar os Raycasts do Canvas

        RectTransform canvasRect = canvasObj.GetComponent<RectTransform>();
        canvasRect.anchorMin = Vector2.zero;
        canvasRect.anchorMax = Vector2.one;
        canvasRect.offsetMin = Vector2.zero;
        canvasRect.offsetMax = Vector2.zero;

        GameObject imageObj = new GameObject("Image");
        imageObj.transform.SetParent(canvasObj.transform);

        Image image = imageObj.AddComponent<Image>();
        image.color = Color.black;

        RectTransform imageRect = imageObj.GetComponent<RectTransform>();
        imageRect.anchorMin = Vector2.zero;
        imageRect.anchorMax = Vector2.one;
        imageRect.offsetMin = Vector2.zero;
        imageRect.offsetMax = Vector2.zero;

        fadeCanvasGroup = imageObj.AddComponent<CanvasGroup>();
    }
}