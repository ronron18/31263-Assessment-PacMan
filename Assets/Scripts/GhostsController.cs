using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This script manages all ghosts at once. Ghost movement is managed here.
*/

public class GhostsController : MonoBehaviour
{
    private AudioController audioController;
    public float scaredTimer = 0.0f;
    private bool inRecoveringState = false;
    public bool inScaredState = false;

    public GhostController[] ghosts;

    // Start is called before the first frame update
    void Start()
    {
        audioController = GetComponent<AudioController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(scaredTimer > 0.0f)
        {
            if(scaredTimer < 3.0f && !inRecoveringState) 
            {
                SetRecovering();
                inRecoveringState = true;
            }
            scaredTimer -= Time.deltaTime;
        }
        else if(inScaredState)
        {
            scaredTimer = 0.0f;
            SetNormal();
        }
        Debug.Log("ScaredTimer: " + scaredTimer);
    }

    // Change ghost to scared state
    public void SetScared()
    {
        inScaredState = true;
        inRecoveringState = false;

        // Change Audio Clip
        audioController.ChangeClip(audioController.scaredBGM);

        // Set all ghosts to scared
        foreach(GhostController ghost in ghosts)
        {
            ghost.isScared = true;
        }

        // START TIMER
        scaredTimer = 10.0f; 
    }

    // Change ghost to recovering state
    void SetRecovering()
    {
        // Set each and every ghost's state to recovering if still scared
        foreach(GhostController ghost in ghosts)
        {
            if(ghost.isScared)
                ghost.isRecovering = true;
        }
    }

    // Change ghost to normal state
    void SetNormal()
    {
        inScaredState = false;
        inRecoveringState = false;
        // Set each and every ghost's state to normal
        foreach(GhostController ghost in ghosts)
        {
            if(!ghost.isDead)
            {
                ghost.isScared = false;
                ghost.isRecovering = false;
            }
        }

        // Change Audio Clip
        audioController.ChangeClip(audioController.normalBGM);
    }

    // Ghost Movement
    void TweenGhosts()
    {

    }
}
