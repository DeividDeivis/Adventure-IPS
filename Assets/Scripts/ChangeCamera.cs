using UnityEngine;

public class ChangeCamera : MonoBehaviour
{
    [SerializeField] private bool camDollyState = false;
    [SerializeField] private Animator camerasAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        camDollyState = !camDollyState;
        camerasAnimator.SetBool("Dolly", camDollyState);
    }
}
