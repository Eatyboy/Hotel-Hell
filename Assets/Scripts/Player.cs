using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    private InputSystem_Actions ctrl;

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

    }

    private void One(InputAction.CallbackContext ctx)
    {

    }

    private void Two(InputAction.CallbackContext ctx)
    {

    }

    private void Three(InputAction.CallbackContext ctx)
    {

    }

    private void Four(InputAction.CallbackContext ctx)
    {

    }

    private void Five(InputAction.CallbackContext ctx)
    {

    }

    private void Six(InputAction.CallbackContext ctx)
    {

    }

    private void Seven(InputAction.CallbackContext ctx)
    {

    }

    private void Eight(InputAction.CallbackContext ctx)
    {

    }

    private void Nine(InputAction.CallbackContext ctx)
    {

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
        }
        else
        {
            Debug.Log("Wrong!");
        }
    }
}
