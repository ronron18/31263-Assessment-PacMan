using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script manages individual ghost
*/

public class GhostStatusController : MonoBehaviour
{
    private Animator animator;
    private Collider ghostCollider;
    private AudioController audioController;
    public Vector3 previousPosition;
    private GhostsStatusController ghostsController;
    public Vector3 spawnPoint;

    public bool isScared = false;
    public bool isRecovering = false;
    public bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ghostCollider = GetComponent<Collider>();
        audioController = GameObject.FindWithTag("MainGameController").GetComponent<AudioController>();
        ghostsController = GameObject.FindWithTag("MainGameController").GetComponent<GhostsStatusController>();
        previousPosition = transform.position;
        spawnPoint = transform.position;
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
        // Change BGM
        audioController.ChangeClip(audioController.oneEatenBGM);
        // TURN OFF COLLIDER!!!
        ghostCollider.enabled = false;
    }

    public void Respawn()
    {
        isScared = ghostsController.inScaredState;
        isRecovering = ghostsController.inRecoveringState;
        isDead = false;
        ghostCollider.enabled = true;
    }
}
