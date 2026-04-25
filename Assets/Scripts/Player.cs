using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("References")]
    public static Player instance;
    private InputSystem_Actions ctrl;
    [SerializeField] private Satan satan;
    [SerializeField] private Image mc;
    [SerializeField] private Sprite mcDefault;
    [SerializeField] private Sprite mcHoldUp;

    [Header("Parameters")]
    public int maxHp = 3;
    public float playerHoldUpSignDuration = 0.5f;

    [Header("State")]
    public int hp = 3;
    public int souls = 0;

    private void Awake()
    {
        if (instance != null && instance != this) Destroy(gameObject);
        else instance = this;

        ctrl = new();
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
    }

    private void One(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Limbo));
    }

    private void Two(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Lust));
    }

    private void Three(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Gluttony));
    }

    private void Four(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Greed));
    }

    private void Five(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Anger));
    }

    private void Six(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Heresy));
    }

    private void Seven(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Violence));
    }

    private void Eight(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Fraud));
    }

    private void Nine(InputAction.CallbackContext ctx)
    {
        StartCoroutine(SendToFloor(HellCircle.Treachery));
    }

    public IEnumerator SendToFloor(HellCircle floor)
    {
        HellCircle greatestSin = HellCircle.None; 
        foreach (var sin in SinnerManager.instance.currentSinner.data.sins)
        {
            if (sin.hellCircle > greatestSin) greatestSin = sin.hellCircle;
        }

        yield return new WaitForSeconds(playerHoldUpSignDuration);

        mc.sprite = mcHoldUp;

        if (greatestSin == floor)
        {
            souls++;
        }
        else
        {
            LoseHP();
        }

        yield return SinnerManager.instance.SendSinnerAway();
    }

    public void LoseHP()
    {
        hp--;
        satan.SetSatanLevel(maxHp - hp);
        if (hp <= 0) GameManager.instance.GameOver();
    }
}
