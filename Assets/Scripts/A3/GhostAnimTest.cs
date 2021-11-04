using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostAnimTest : MonoBehaviour
{
    [SerializeField] private float timer = 0;
    [SerializeField] private int direction = -1;
    [SerializeField] private bool isScared = false;
    [SerializeField] private bool isRecovering = false;
    [SerializeField] private bool isDead = false;

    private Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ChangeAnimation(); // Start playing initial animation
    }

    // Update is called once per frame
    void Update()
    {
        if(timer > 3.0f)
        {
            timer = 0.0f;
            ChangeAnimation();
        }
        timer = timer + Time.deltaTime;
    }

    private void ChangeAnimation()
    {
        if(direction >= 2 || isDead) // Change state once all direction animations have been passed trough or "ghost" is dead
        {
            if(!isScared) 
            {
                if(isDead)
                {
                    isDead = false;
                }
                else
                {
                    isScared = true;
                }  
            }
            else
            {
                if(!isRecovering)
                {
                    isRecovering = true;
                }
                else
                {
                    if(!isDead)
                    {
                        isDead = true;
                        isScared = false;
                        isRecovering = false;
                    }
                }
            }

            

            direction = 0;
        }
        else
        {
            direction++; // Change direction animation
        }
        animator.SetInteger("direction", direction);
        animator.SetBool("isScared", isScared);
        animator.SetBool("isRecovering", isRecovering);
        animator.SetBool("isDead", isDead);
    }
}
