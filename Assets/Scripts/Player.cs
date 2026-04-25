using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputSystem_Actions ctrl;
    private int correctCount;
    private bool paused = false;

    public GameManager gameManager;

    private void Awake()
    {
        ctrl = new();
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
        paused = !paused; 
        if (paused)
        {
            gameManager.Pause();
        }
        else
        {
            gameManager.UnPause();
        }
    }

    private void One(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Limbo);
    }

    private void Two(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Lust);
    }

    private void Three(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Gluttony);
    }

    private void Four(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Greed);
    }

    private void Five(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Anger);
    }

    private void Six(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Heresy);
    }

    private void Seven(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Violence);
    }

    private void Eight(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Fraud);
    }

    private void Nine(InputAction.CallbackContext ctx)
    {
        if (paused) {return;}
        SendToFloor(HellCircle.Treachery);
    }

    public void SendToFloor(HellCircle floor)
    {
        HellCircle greatestSin = HellCircle.None; 
        foreach (var sin in SinnerManager.instance.currentSinner.data.sins)
        {
            if (sin.hellCircle > greatestSin) greatestSin = sin.hellCircle;
        }
        if (greatestSin == floor)
        {
            Debug.Log("Correct!");
            correctCount++;
        }
        else
        {
            Debug.Log("Wrong!");
        }

        SinnerManager.instance.SendSinnerAway();
    }
}
