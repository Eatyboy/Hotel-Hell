using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerSouls = 3;
    [SerializeField] private GameObject pause;
    public static GameManager instance;

    private void Awake()
    {
        pause.SetActive(false);
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;
    }

    public void GameOver()
    {
        Debug.Log("Game Over");
        AudioManager.instance.StopMusic();
        SceneManager.LoadScene("MainMenu");
    }

    public void Pause()
    {
        Time.timeScale = 0f;
        pause.SetActive(true);
    }

    public void UnPause()
    {
        Time.timeScale = 1f;
        pause.SetActive(false);
    }
}
