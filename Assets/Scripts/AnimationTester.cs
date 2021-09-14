using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    Code for testing animations, just to test whether there are something wrong or not
    //////------NOT USED IN FINAL PRODUCT------//////
*/

public class AnimationTester : MonoBehaviour
{
    public Animator animator;
    public int direction = 0;           // Cardinal direction of the gameObject
    public bool isDead = false;         // Is GameObject Dead?

    // Update is called once per frame
    void Update()
    {
        // Rotate object in cardinal directions
        if(Input.GetKeyDown(KeyCode.UpArrow))
            direction = 0;
        if(Input.GetKeyDown(KeyCode.RightArrow))
            direction = 1;
        if(Input.GetKeyDown(KeyCode.DownArrow))
            direction = 2;
        if(Input.GetKeyDown(KeyCode.LeftArrow))
            direction = 3;

        // Change dead parameter
        if(Input.GetKeyDown(KeyCode.Escape))
            isDead = true;

        // UPDATE PARAMETERS
        animator.SetBool("isDead", isDead);
        animator.SetInteger("direction", direction);
    }
}
