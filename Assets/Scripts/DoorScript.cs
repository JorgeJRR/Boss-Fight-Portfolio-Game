using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Animator animator;

    [ContextMenu("abrir puerta")]
    public void OpenDoor()
    {
        animator.SetTrigger("openDoor");
    }

    [ContextMenu("cerrar puerta")]
    public void CloseDoor()
    {
        animator.SetTrigger("closeDoor");
    }
}
