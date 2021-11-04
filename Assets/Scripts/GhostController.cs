using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GhostController : MonoBehaviour
{
    public Transform[] ghosts;
    public Vector3[] inaccesiblePath;
    public Vector3 spawnAreaMid;
    public Vector3 spawnAreaSize;
    private Bounds spawnArea;
    public Transform player;
    public float movementSpeed = 30.0f;

    // Ghost 4
    public Vector3[] ghostFourPaths;
    public Vector3 ghostFourTargetDestination;
    public int destinationIndex;
    private bool reachedTargetDestination;

    private Tweener tweener;

    private Tilemap tiles;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        tweener = GameObject.FindWithTag("MainGameController").GetComponent<Tweener>();
        tiles = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        spawnArea = new Bounds(spawnAreaMid, spawnAreaSize);
        destinationIndex = Random.Range(0, ghostFourPaths.Length);
        ghostFourTargetDestination = ghostFourPaths[destinationIndex];
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < ghosts.Length; i++)
        {
            if(spawnArea.Contains(ghosts[i].position) && ghosts[i].GetComponent<GhostStatusController>().isDead)
            {
                if(ghosts[i].position == ghosts[i].GetComponent<GhostStatusController>().spawnPoint)
                    ghosts[i].GetComponent<GhostStatusController>().Respawn();
            }
            else if(!spawnArea.Contains(ghosts[i].position) && ghosts[i].GetComponent<GhostStatusController>().isDead)
            {
                GhostBackToSpawn(ghosts[i]);
            }
            else if(spawnArea.Contains(ghosts[i].position) && !ghosts[i].GetComponent<GhostStatusController>().isDead)
            {
                GhostLeaveSpawn(ghosts[i]);
            }
            else if(!spawnArea.Contains(ghosts[i].position) && !ghosts[i].GetComponent<GhostStatusController>().isDead && 
                    ghosts[i].GetComponent<GhostStatusController>().isScared)
            {
                GhostOneSelectTile(ghosts[i]);
            }
            else if(!spawnArea.Contains(ghosts[i].position) && !ghosts[i].GetComponent<GhostStatusController>().isDead)
            {
                switch(i)
                {
                    case 0:
                        GhostOneSelectTile(ghosts[i]);
                    break;

                    case 1:
                        GhostTwoSelectTile(ghosts[i]);
                    break;
                
                    case 2:
                        GhostThreeSelectTile(ghosts[i]);
                    break;

                    case 3:
                        GhostFourSelectTile(ghosts[i]);
                    break;
                }
            }
        }
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
    void GhostLeaveSpawn(Transform ghost)
    {
        if(ghost != null)
        {
            if(!tweener.TweenExists(ghost))
            {
                Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
                Dictionary<Vector3,float> directionalDistance = new Dictionary<Vector3,float>();
                // Book keeping
                Vector3 selectedDirection = Vector3.zero;
                float selectedDistance = Mathf.Infinity;

                for(int i = 0; i < directions.Length; i++)
                {
                    if(IsWalkable(ghost, directions[i], ghost.GetComponent<GhostStatusController>().previousPosition))
                    {
                        if(Vector3.Distance((ghost.position + directions[i]), spawnAreaMid + Vector3.up*100) < selectedDistance && 
                        Vector3.Distance((ghost.position + directions[i]), spawnAreaMid + Vector3.up*100) < Vector3.Distance((ghost.position + directions[i]), spawnAreaMid + Vector3.down*100))
                        {
                            directionalDistance.Add(directions[i], Vector3.Distance((ghost.position + directions[i]), spawnAreaMid + Vector3.up*100));
                        }
                        else if(Vector3.Distance((ghost.position + directions[i]), spawnAreaMid + Vector3.down*100) < selectedDistance)
                        {
                            directionalDistance.Add(directions[i], Vector3.Distance((ghost.position + directions[i]), spawnAreaMid + Vector3.down*100));
                        }
                    }
                }

                foreach(KeyValuePair<Vector3, float> directionDistancePairs in directionalDistance)
                {
                    if(directionDistancePairs.Value < selectedDistance)
                    {
                        selectedDistance = directionDistancePairs.Value;
                        selectedDirection = directionDistancePairs.Key;
                    }
                    else if(directionDistancePairs.Value == selectedDistance)
                    {
                        if(Random.value > 0.5f)
                        {
                            selectedDistance = directionDistancePairs.Value;
                            selectedDirection = directionDistancePairs.Key;
                        }
                    }
                }

                if(selectedDirection == Vector3.zero) // IF MUST BACKTRACK
                {
                    selectedDirection = ghost.GetComponent<GhostStatusController>().previousPosition - ghost.position;
                }
                //Debug.Log("Leaving spawn: " + ghost.GetComponent<GhostStatusController>().previousPosition);
                MoveGhost(ghost, selectedDirection);
            }
        }
    }

    void GhostOneSelectTile(Transform ghost)
    {
        if(ghost != null)
        {
            if(!tweener.TweenExists(ghost))
            {
                Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
                // Book keeping
                Vector3 selectedDirection = Vector3.zero;
                float selectedDistance = -Mathf.Infinity;

                for(int i = 0; i < directions.Length; i++)
                {
                    if(IsWalkable(ghost, directions[i], ghost.GetComponent<GhostStatusController>().previousPosition) && 
                    !spawnArea.Contains(ghost.position + directions[i]))
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
                //Debug.Log("Ghost 1: " + ghost.GetComponent<GhostStatusController>().previousPosition);
                MoveGhost(ghost, selectedDirection);
            }
        }
    }

    void GhostTwoSelectTile(Transform ghost)
    {
        if(ghost != null)
        {
            if(!tweener.TweenExists(ghost))
            {
                Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
                // Book keeping
                Vector3 selectedDirection = Vector3.zero;
                float selectedDistance = Mathf.Infinity;

                for(int i = 0; i < directions.Length; i++)
                {
                    if(IsWalkable(ghost, directions[i], ghost.GetComponent<GhostStatusController>().previousPosition) && 
                    !spawnArea.Contains(ghost.position + directions[i]))
                    {
                        if(Vector3.Distance((ghost.position + directions[i]), player.position) < selectedDistance)
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
                //Debug.Log("Ghost 2: " + ghost.GetComponent<GhostStatusController>().previousPosition);
                MoveGhost(ghost, selectedDirection);
            }
        }
    }

    void GhostThreeSelectTile(Transform ghost)
    {
        if(ghost != null)
        {
            if(!tweener.TweenExists(ghost))
            {
                Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
                List<Vector3> validDirections = new List<Vector3>();
                // Book keeping
                Vector3 selectedDirection = Vector3.zero;

                for(int i = 0; i < directions.Length; i++)
                {
                    if(IsWalkable(ghost, directions[i], ghost.GetComponent<GhostStatusController>().previousPosition) && 
                    !spawnArea.Contains(ghost.position + directions[i]))
                    {
                        validDirections.Add(directions[i]);
                    }
                }

                selectedDirection = validDirections[Random.Range(0, validDirections.Count)];

                if(selectedDirection == Vector3.zero) // IF MUST BACKTRACK
                {
                    selectedDirection = ghost.GetComponent<GhostStatusController>().previousPosition - ghost.position;
                }
                //Debug.Log("Ghost 3: " + ghost.GetComponent<GhostStatusController>().previousPosition);
                MoveGhost(ghost, selectedDirection);
            }
        }
    }

    void GhostFourSelectTile(Transform ghost)
    {
        if(ghost != null)
        {
            if(!tweener.TweenExists(ghost))
            {
                if(ghost.position == ghostFourTargetDestination) 
                {
                    reachedTargetDestination = true;
                    ghost.GetComponent<GhostStatusController>().previousPosition = ghost.position;
                }
                if(!reachedTargetDestination)    // When ghost 4 hasn't reached first destination
                {
                    Vector3[] directions = {Vector3.up, Vector3.down, Vector3.left, Vector3.right};
                    // Book keeping
                    Vector3 selectedDirection = Vector3.zero;
                    float selectedDistance = Mathf.Infinity;

                    for(int i = 0; i < directions.Length; i++)
                    {
                        if(IsWalkable(ghost, directions[i], ghost.GetComponent<GhostStatusController>().previousPosition) && 
                        !spawnArea.Contains(ghost.position + directions[i]))
                        {
                            if(Vector3.Distance((ghost.position + directions[i]), ghostFourTargetDestination) < selectedDistance)
                            {
                                selectedDirection = directions[i];
                                selectedDistance = Vector3.Distance((ghost.position + directions[i]), ghostFourTargetDestination);
                            }
                            else if(Vector3.Distance((ghost.position + directions[i]), ghostFourTargetDestination) == selectedDistance)
                            {
                                // decide to overwrite or not
                                if(Random.value > 0.5)
                                {
                                    selectedDirection = directions[i];
                                    selectedDistance = Vector3.Distance((ghost.position + directions[i]), ghostFourTargetDestination);
                                }
                            }
                        }
                    }
                    if(selectedDirection == Vector3.zero) // IF MUST BACKTRACK
                    {
                        selectedDirection = ghost.GetComponent<GhostStatusController>().previousPosition - ghost.position;
                    }
                    //Debug.Log("Ghost 4: " + ghost.GetComponent<GhostStatusController>().previousPosition);
                    MoveGhost(ghost, selectedDirection);
                }
                else    // when it has reached the first destination
                {
                    destinationIndex++;
                    reachedTargetDestination = false;
                    if(destinationIndex == ghostFourPaths.Length) destinationIndex = 0;
                    ghostFourTargetDestination = ghostFourPaths[destinationIndex];
                }
            }
        }
    }

    void GhostBackToSpawn(Transform ghost)
    {
        if(ghost != null)
        {
            if(!tweener.TweenExists(ghost))
            {
                tweener.AddTween(ghost, 
                                ghost.position, 
                                ghost.GetComponent<GhostStatusController>().spawnPoint,
                                10.0f * Vector3.Distance(ghost.position, ghost.GetComponent<GhostStatusController>().spawnPoint)/movementSpeed 
                                );
            }
        }
    }
}
