using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int playerSouls = 3;
    [SerializeField] private GameObject pause;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        pause.SetActive(false);
    }

    // Update is called once per frame
    private void Update()
    {
        
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
