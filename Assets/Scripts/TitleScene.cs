using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Cysharp.Threading.Tasks;


public class TitleScene : MonoBehaviour
{
    private InputSystem_Actions ctrl;
    [SerializeField] private Animator animator;
    private bool inStartCutscene = false;

    private void Awake()
    {
        ctrl = new();

        ScreenFader.FadeIn(GameManager.instance.transitionDuration / 2.0f).Forget();
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
        if (!inStartCutscene)
        {
            animator.SetTrigger("StartGame");
            inStartCutscene = true;
        }
    }

    private void GoToGame()
    {
        GameManager.instance.StartGame().Forget();
    }

    private void Credits(InputAction.CallbackContext ctx)
    {
        GameManager.instance.GoCredits().Forget();
    }
}
