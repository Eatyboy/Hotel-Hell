using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("References")]
    public static Player instance;
    private InputSystem_Actions ctrl;
    private bool paused = false;

    [SerializeField] private Satan satan;
    [SerializeField] private Image mc;
    [SerializeField] private TextMeshProUGUI mcNumberTMP;
    [SerializeField] private Sprite mcDefault;
    [SerializeField] private Sprite mcHoldUp;

    public PauseController myPauseController;

    [Header("Parameters")]
    public int maxHp = 3;
    public float playerHoldUpSignDuration = 0.5f;
    public float nextSinnerDelay = 1.0f;

    [Header("State")]
    public int hp = 3;
    public int souls = 0;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        ctrl = new();

        mcNumberTMP.gameObject.SetActive(false);
        mc.sprite = mcDefault;
    }

    private void Start()
    {
        hp = maxHp;
    }

    private void OnEnable()
    {
        ctrl.Enable();
        ctrl.Player._0.performed += Zero;
        ctrl.Player._1.performed += One;
        ctrl.Player._2.performed += Two;
        ctrl.Player._3.performed += Three;
        ctrl.Player._4.performed += Four;
        ctrl.Player._5.performed += Five;
        ctrl.Player._6.performed += Six;
        ctrl.Player._7.performed += Seven;
        ctrl.Player._8.performed += Eight;
        ctrl.Player._9.performed += Nine;
    }

    private void OnDisable()
    {
        ctrl.Player._0.performed -= Zero;
        ctrl.Player._1.performed -= One;
        ctrl.Player._2.performed -= Two;
        ctrl.Player._3.performed -= Three;
        ctrl.Player._4.performed -= Four;
        ctrl.Player._5.performed -= Five;
        ctrl.Player._6.performed -= Six;
        ctrl.Player._7.performed -= Seven;
        ctrl.Player._8.performed -= Eight;
        ctrl.Player._9.performed -= Nine;
        ctrl.Disable();
    }

    private void Zero(InputAction.CallbackContext ctx)
    {
        AudioManager.instance.PlayOneShot(AudioManager.instance.elevatorButton);
        
        paused = !paused; 
        if (paused)
        {
            GameManager.instance.Pause();
        }
        else
        {
            GameManager.instance.UnPause();
        }
    }

    private void One(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(1);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Limbo));
        myPauseController.updateEachSin(0);
    }

    private void Two(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(2);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Lust));
        myPauseController.updateEachSin(1);
        
    }

    private void Three(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(3);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Gluttony));
        myPauseController.updateEachSin(2);
    }

    private void Four(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(4);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Greed));
        myPauseController.updateEachSin(3);
    }

    private void Five(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(5);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Anger));
        myPauseController.updateEachSin(4);
    }

    private void Six(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(6);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Heresy));
        myPauseController.updateEachSin(5);
    }

    private void Seven(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(7);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Violence));
        myPauseController.updateEachSin(6);
    }

    private void Eight(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(8);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Fraud));
        myPauseController.updateEachSin(7);
    }

    private void Nine(InputAction.CallbackContext ctx)
    {
        if (paused) {
            myPauseController.OnFloorButtonPressed(9);
            return;
        }
        StartCoroutine(SendToFloor(HellCircle.Treachery));
        myPauseController.updateEachSin(8);
    }

    public IEnumerator SendToFloor(HellCircle floor)
    {
        AudioManager.instance.PlayOneShot(AudioManager.instance.elevatorButton);

        HellCircle greatestSin = HellCircle.None; 
        foreach (var sin in SinnerManager.instance.currentSinner.data.sins)
        {
            if (sin.hellCircle > greatestSin) greatestSin = sin.hellCircle;
        }

        mc.sprite = mcHoldUp;
        mcNumberTMP.text = ((int)floor).ToString();
        mcNumberTMP.gameObject.SetActive(true);

        yield return new WaitForSeconds(playerHoldUpSignDuration);

        AudioManager.instance.PlayOneShot(AudioManager.instance.elevatorDing);

        yield return SinnerManager.instance.SendSinnerAway();

        mcNumberTMP.gameObject.SetActive(false);
        mc.sprite = mcDefault;

        if (greatestSin == floor)
        {
            souls++;
        }
        else
        {
            LoseHP();
        }

        yield return new WaitForSeconds(nextSinnerDelay);

        yield return SinnerManager.instance.NextSinner();
    }

    public void LoseHP()
    {
        hp--;
        satan.SetSatanLevel(maxHp - hp);
        AudioManager.instance.PlayOneShot(AudioManager.instance.satanLaugh);
        if (hp <= 0) GameManager.instance.GameOver();
    }
}
