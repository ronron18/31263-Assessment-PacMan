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

    public AudioClip[] audioClips;
    private enum AudioClips
    {
        walk, pellet
    }

    [SerializeField] float movementSpeed;
    private KeyCode lastInput;
    private KeyCode currentInput;
    private Vector3 moveTargetPosition;

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

        if(!tweener.TweenExists(transform))
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
        }
    }
}
