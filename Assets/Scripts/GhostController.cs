using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostController : MonoBehaviour
{
    public Transform[] ghosts;
    public Vector3[] inaccesiblePath;
    public Transform player;
    public float movementSpeed = 30.0f;

    private Tweener tweener;

    private Tilemap tiles;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        tweener = GameObject.FindWithTag("MainGameController").GetComponent<Tweener>();
        tiles = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
    }

    // Update is called once per frame
    void Update()
    {
        GhostOneSelectTile(ghosts[0]);
    }

    void MoveGhost(Transform target, Vector3 direction)
    {
        //Debug.Log("lastInput: " + IsWalkable(lastInput));
        //Debug.Log("currentInput: " + IsWalkable(currentInput));\
        target.GetComponent<GhostStatusController>().previousPosition = target.position;
        Vector3 moveTargetPosition = target.position + direction;
        Animator animator = target.GetComponent<Animator>();

        if(direction == Vector3.up) animator.SetInteger("direction", (int)Directions.GhostAnimDirections.up);
        if(direction == Vector3.down) animator.SetInteger("direction", (int)Directions.GhostAnimDirections.down);
        if(direction == Vector3.left || direction == Vector3.right) animator.SetInteger("direction", (int)Directions.GhostAnimDirections.side);
        tweener.AddTween(target, 
                                target.position, 
                                moveTargetPosition,
                                10.0f/movementSpeed 
                                );
    }

    bool IsWalkable(Transform ghost, Vector3 direction, Vector3 previousPosition)
    {
        // Check if adjacent tile is walkable, then override currentInput <70%>
        
        //Debug.Log(playerPositionInTilemap);

        Tile adjacentTile = null;
        bool isInaccesible = false;
        Vector3 target = ghost.position + direction;
        adjacentTile = tiles.GetTile<Tile>(tiles.WorldToCell(target));

        foreach(Vector3 notAccessible in inaccesiblePath)
        {
            if(target == notAccessible)
            {
                isInaccesible = true;
            }
        }

        // Also check if target is in list of inaccesible spots
        if(adjacentTile != null || isInaccesible || target == previousPosition) return false;
        return true; // Check if there is no obstacle, true if obstacle is found
    }

    // GHOST AIS
    void GhostOneSelectTile(Transform ghost)
    {
        if(!tweener.TweenExists(ghost))
        {
            Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
            // Book keeping
            Vector3 selectedDirection = Vector3.zero;
            float selectedDistance = 0.0f;

            for(int i = 0; i < directions.Length; i++)
            {
                if(IsWalkable(ghost, directions[i], ghost.GetComponent<GhostStatusController>().previousPosition))
                {
                    if(Vector3.Distance((ghost.position + directions[i]), player.position) > selectedDistance)
                    {
                        selectedDirection = directions[i];
                        selectedDistance = Vector3.Distance((ghost.position + directions[i]), player.position);
                    }
                    else if(Vector3.Distance((ghost.position + directions[i]), player.position) == selectedDistance)
                    {
                        // decide to overwrite or not
                        if(Random.value > 0.5)
                        {
                            selectedDirection = directions[i];
                            selectedDistance = Vector3.Distance((ghost.position + directions[i]), player.position);
                        }
                    }
                }
            }
            if(selectedDirection == Vector3.zero) // IF MUST BACKTRACK
            {
                selectedDirection = ghost.GetComponent<GhostStatusController>().previousPosition - ghost.position;
            }
            Debug.Log(ghost.GetComponent<GhostStatusController>().previousPosition);
            MoveGhost(ghost, selectedDirection);
        }
    }
}
