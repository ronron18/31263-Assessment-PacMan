using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script manages each ghosts separately
*/

public class GhostController : MonoBehaviour
{
    private Animator animator;
    private Collider ghostCollider;

    public bool isScared = false;
    public bool isRecovering = false;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ghostCollider = GetComponent<Collider>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isScared", isScared);
        animator.SetBool("isRecovering", isRecovering);
        animator.SetBool("isDead", isDead);
    }

    public void Death()
    {
        isScared = false;
        isRecovering = false;
        isDead = true;
        // TURN OFF COLLIDER!!!
        ghostCollider.enabled = false;
    }
}
