using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class TitleScene : MonoBehaviour
{
    private InputSystem_Actions ctrl;
    [SerializeField] private Animator animator;

    private void Awake()
    {
        ctrl = new();

        StartCoroutine(ScreenFader.FadeIn(GameManager.instance.transitionDuration / 2.0f));
    }

    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnEnable()
    {
        ctrl.Enable();
        ctrl.Player._0.performed += StartGame;
        ctrl.Player._9.performed += Credits;
    }

    private void OnDisable()
    {
        ctrl.Player._0.performed -= StartGame;
        ctrl.Player._9.performed -= Credits;
        ctrl.Disable();
    }

    private void StartGame(InputAction.CallbackContext ctx)
    {
        animator.SetTrigger("StartGame");
    }

    private void GoToGame()
    {
        StartCoroutine(GameManager.instance.StartGame());
    }

    private void Credits(InputAction.CallbackContext ctx)
    {
        StartCoroutine(GameManager.instance.GoCredits());
    }
}
