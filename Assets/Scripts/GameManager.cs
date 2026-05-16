using Cysharp.Threading.Tasks;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerSouls = 3;
    public static GameManager instance;

    public float transitionDuration = 1.0f;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public async UniTask StartGame()
    {
        AudioManager.instance.StopMusic();
        await ScreenFader.FadeOut(transitionDuration / 2.0f);
        await SceneManager.LoadSceneAsync("Game");
    }

    public async UniTask GoCredits()
    {
        AudioManager.instance.StopMusic();
        await ScreenFader.FadeOut(transitionDuration / 2.0f);
        await SceneManager.LoadSceneAsync("Credits");
    }


    public async UniTask QuitToMainMenu()
    {
        AudioManager.instance.StopMusic();
        await ScreenFader.FadeOut(transitionDuration / 2.0f);
        await SceneManager.LoadSceneAsync("Lose");
        await ScreenFader.FadeIn(transitionDuration / 2.0f);
    }

    public async UniTask StartMenu()
    {
        AudioManager.instance.StopMusic();
        await ScreenFader.FadeOut(transitionDuration / 2.0f);
        await SceneManager.LoadSceneAsync("MainMenu");
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        QuitToMainMenu().Forget();
    }

}
