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

    public IEnumerator StartGame()
    {
        AudioManager.instance.StopMusic();
        yield return ScreenFader.FadeOut(transitionDuration / 2.0f);
        yield return SceneManager.LoadSceneAsync("Game");
    }

    public IEnumerator GoCredits()
    {
        AudioManager.instance.StopMusic();
        yield return ScreenFader.FadeOut(transitionDuration / 2.0f);
        yield return SceneManager.LoadSceneAsync("Credits");
    }


    public IEnumerator QuitToMainMenu()
    {
        AudioManager.instance.StopMusic();
        yield return ScreenFader.FadeOut(transitionDuration / 2.0f);
        yield return SceneManager.LoadSceneAsync("MainMenu");
        yield return ScreenFader.FadeIn(transitionDuration / 2.0f);
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        StartCoroutine(QuitToMainMenu());
    }

}
