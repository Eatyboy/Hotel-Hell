using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


public class TitleScene : MonoBehaviour
{
    private InputSystem_Actions ctrl;

    private void Awake()
    {
        ctrl = new();
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
        SceneManager.LoadScene("Game");
    }

    private void Credits(InputAction.CallbackContext ctx)
    {
       // SceneManagement.loadScene():
    }
}
