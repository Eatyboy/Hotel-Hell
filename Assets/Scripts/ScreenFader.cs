using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;

public class ScreenFader : MonoBehaviour
{
    private static ScreenFader instance;

    [SerializeField] private CanvasGroup fadeBackground;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(gameObject);

        instance.fadeBackground.alpha = 0.0f;
        instance.fadeBackground.blocksRaycasts = false;
    }

    public static async UniTask FadeIn(float duration)
    {
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            instance.fadeBackground.alpha = Mathf.Clamp01(1.0f - t);
            await UniTask.Yield();
        }
        instance.fadeBackground.alpha = 0.0f;
        instance.fadeBackground.blocksRaycasts = false;
    }

    public static async UniTask FadeOut(float duration)
    {
        instance.fadeBackground.blocksRaycasts = true;
        float elapsedTime = 0.0f;
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / duration;
            instance.fadeBackground.alpha = Mathf.Clamp01(t);
            await UniTask.Yield();
        }
        instance.fadeBackground.alpha = 1.0f;
    }
}
