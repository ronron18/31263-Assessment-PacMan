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

    public bool isScared = false;
    public bool isRecovering = false;
    public bool isDead = false;
    [SerializeField] private float respawnTimer = 5.0f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        ghostCollider = GetComponent<Collider>();
        audioController = GameObject.FindWithTag("MainGameController").GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        animator.SetBool("isScared", isScared);
        animator.SetBool("isRecovering", isRecovering);
        animator.SetBool("isDead", isDead);

        if(isDead && respawnTimer > 0.0f)
        {
            respawnTimer -= Time.deltaTime;
        }
        if(isDead && respawnTimer <= 0.0f)
        {
            Respawn();
        }
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
        respawnTimer = 5.0f;
    }

    public void Respawn()
    {
        isScared = false;
        isRecovering = false;
        isDead = false;
        ghostCollider.enabled = true;
    }
}
