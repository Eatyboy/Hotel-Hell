using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class CreditsManager : MonoBehaviour
{
    private InputSystem_Actions ctrl;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        ctrl = new();
        StartCoroutine(ScreenFader.FadeIn(GameManager.instance.transitionDuration / 2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        ctrl.Enable();
        ctrl.Player._0.performed += RestartGame;
    }

    private void OnDisable()
    {
        ctrl.Player._0.performed -= RestartGame;
        ctrl.Disable();
    }

    public void RestartGame(InputAction.CallbackContext ctx)
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
