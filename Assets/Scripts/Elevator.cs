using UnityEngine;

public class Elevator : MonoBehaviour
{
    public Animator animator;
    public bool isOpened = false;

    public void CloseElevator()
    {
        animator.SetTrigger("Close");
        AudioManager.instance.PlayOneShot(AudioManager.instance.elevatorClose);
    }

    public void OpenElevator()
    {
        animator.SetTrigger("Open");
        AudioManager.instance.PlayOneShot(AudioManager.instance.elevatorOpen);
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
