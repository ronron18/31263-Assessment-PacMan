using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    private GameObject player;
    private AudioSource playerAudioSource;
    private ParticleSystem playerParticleSystem;
    private Animator playerAnimator;
    private Tilemap tiles;
    [SerializeField] private GameObject bumpParticleGameObject;
    private ScoreManager scoreManager;

    public AudioClip[] audioClips;
    private enum AudioClips
    {
        walk, pellet, bump
    }

    [SerializeField] float movementSpeed;
    private KeyCode lastInput;
    private KeyCode currentInput;
    private Vector3 moveTargetPosition;
    private bool wallHit = false;   // Checks if object have hit a wall
    private bool inTeleporter = false; // Checks if the player is in a teleporter

    // Start is called before the first frame update
    void Start()
    {
        //tweener = GameObject.FindWithTag("Manager").GetComponent<Tweener>();
        tiles = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        tweener = GameObject.FindWithTag("MainGameController").GetComponent<Tweener>();
        playerAudioSource = GetComponent<AudioSource>();
        playerAnimator = GetComponent<Animator>();
        playerParticleSystem = transform.Find("Walk Particles").GetComponent<ParticleSystem>();
        moveTargetPosition = transform.position;
        scoreManager = GameObject.FindWithTag("MainGameController").GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.W))
            lastInput = KeyCode.W;
        if(Input.GetKeyDown(KeyCode.A))
            lastInput = KeyCode.A;
        if(Input.GetKeyDown(KeyCode.S))
            lastInput = KeyCode.S;
        if(Input.GetKeyDown(KeyCode.D))
            lastInput = KeyCode.D;

        if(!tweener.TweenExists(transform) && !inTeleporter)
            MovePlayer();
        
        //Debug.Log("Last Input" + lastInput);
    }

    void MovePlayer()
    {
        //Debug.Log("lastInput: " + IsWalkable(lastInput));
        //Debug.Log("currentInput: " + IsWalkable(currentInput));

        if(IsWalkable(lastInput))
        { // IF WALKABLE
            currentInput = lastInput;
            wallHit = false;
            playerAnimator.speed = 1.0f; // Resumes the animation
            switch(currentInput)
            {
                case KeyCode.W:
                    playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.up);
                    moveTargetPosition = transform.position + Vector3.up;
                    break;
                case KeyCode.A:
                    playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.left);
                    moveTargetPosition = transform.position + Vector3.left;
                    break;
                case KeyCode.S:
                    playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.down);
                    moveTargetPosition = transform.position + Vector3.down;
                    break;
                case KeyCode.D:
                    playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.right);
                    moveTargetPosition = transform.position + Vector3.right;
                    break;
            }
            tweener.AddTween(transform, 
                                transform.position, 
                                moveTargetPosition,
                                10.0f/movementSpeed 
                                );
            PlayMovementEffects();
        }
        else if(IsWalkable(currentInput))
        { // IF NOT WALKABLE but CURRENT INPUT IS WALKABLE
            switch(currentInput)
            {
                    case KeyCode.W:
                        playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.up);
                        moveTargetPosition = transform.position + Vector3.up;
                        break;
                    case KeyCode.A:
                        playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.left);
                        moveTargetPosition = transform.position + Vector3.left;
                        break;
                    case KeyCode.S:
                        playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.down);
                        moveTargetPosition = transform.position + Vector3.down;
                        break;
                    case KeyCode.D:
                        playerAnimator.SetInteger("direction", (int)Directions.PlayerAnimDirections.right);
                        moveTargetPosition = transform.position + Vector3.right;
                        break;
            }
            tweener.AddTween(transform, 
                            transform.position, 
                            moveTargetPosition,
                            10.0f/movementSpeed 
                            );
            PlayMovementEffects();
        }
        //Debug.Log(moveTargetPosition);
    }

    bool IsWalkable(KeyCode input)
    {
        // Check if adjacent tile is walkable, then override currentInput <70%>
        
        //Debug.Log(playerPositionInTilemap);

        Tile adjacentTile = null;
        Vector3 target = transform.position;

        switch(input)
        {
            case KeyCode.W:
                target = transform.position + Vector3.up;
                break;
            case KeyCode.A:
                target = transform.position + Vector3.left;
                break;
            case KeyCode.S:
                target = transform.position + Vector3.down;
                break;
            case KeyCode.D:
                target = transform.position + Vector3.right;
                break;
        }
        adjacentTile = tiles.GetTile<Tile>(tiles.WorldToCell(target));

        // Play bump particle effect
        if(input == currentInput && adjacentTile != null && !tweener.TweenExists(transform) && wallHit == false)
        {
            // play bump sound
            playerAudioSource.PlayOneShot(audioClips[(int)AudioClips.bump], 1.5f);
            Debug.Log("hit wall");

            // play bump particle effect
            // move particle effect to position between player and wall
            bumpParticleGameObject.transform.position = (target + transform.position)/2;
            // Play the particle effect ONCE
            bumpParticleGameObject.GetComponent<ParticleSystem>().Play();

            // Pauses the animation
            playerAnimator.speed = 0.0f;

            // Set wallHit to true as player had collided with a wall
            wallHit = true;
        }

        if(adjacentTile != null) return false;
        return true; // Check if there is no obstacle, true if obstacle is found
    }

    void PlayMovementEffects()
    {
        if(moveTargetPosition != transform.position)     // This is so that audio does not play when the game starts
        {
            // Audio
            playerAudioSource.PlayOneShot(audioClips[(int)AudioClips.walk], 0.65f);
            // Particle Effects
            playerParticleSystem.Play();
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if(collider.gameObject.CompareTag("Pellet"))
        {
            playerAudioSource.PlayOneShot(audioClips[(int)AudioClips.pellet], 0.65f);
            Destroy(collider.gameObject);
            scoreManager.currentScore += 10;
        }
        Debug.Log("Triggered!");
    }

    void OnTriggerStay(Collider collider)
    {
        if(collider.gameObject.CompareTag("Teleporter"))
        {
            inTeleporter = true;    // Indicates that player is in the teleporter, telling component not to tween player
            if(!tweener.TweenExists(transform)) // Teleport when player has done tweening
            {
                Vector2 targetPosition = collider.gameObject.GetComponent<TeleporterController>().targetTeleporter.exitPosition;
                transform.position = targetPosition;
                Debug.Log("Teleported player");
                inTeleporter = false;   // PLAYER CAN NOW MOVE AGAIN!!!
            }
        }
    }
}
