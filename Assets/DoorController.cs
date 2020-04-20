using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void ActivateDoor()
    {
        animator.SetBool("active", true);
    }

    public void DeactivateDoor()
    {
        animator.SetBool("active", false);
    }
}
