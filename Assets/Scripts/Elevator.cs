using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator animator;
    public bool isOpened = false;

    public void CloseElevator()
    {
        animator.SetTrigger("Close");
    }

    public void OpenElevator()
    {
        animator.SetTrigger("Open");
    }

    public void OnElevatorOpened()
    {
        isOpened = true;
        animator.SetTrigger("OpenIdle");
    }

    public void OnElevatorClosed()
    {
        isOpened = false;
    }
}
