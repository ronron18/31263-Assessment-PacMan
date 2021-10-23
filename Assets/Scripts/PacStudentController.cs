using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PacStudentController : MonoBehaviour
{
    private Tweener tweener;
    public GameObject player;
    private Tilemap tiles;

    [SerializeField] float movementSpeed;
    private KeyCode lastInput;
    private KeyCode currentInput;

    // Start is called before the first frame update
    void Start()
    {
        //tweener = GameObject.FindWithTag("Manager").GetComponent<Tweener>();
        tiles = GameObject.FindWithTag("Tilemap").GetComponent<Tilemap>();
        tweener = GetComponent<Tweener>();
        player = GameObject.FindWithTag("Player");
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

        if(!tweener.TweenExists(player.transform))
            MovePlayer();
        //Debug.Log("Last Input" + lastInput);
    }

    void MovePlayer() {
        Debug.Log(IsWalkable());
        if(IsWalkable()) { // IF WALKABLE
            currentInput = lastInput;
            Vector3 target = player.transform.position;
            switch(currentInput) {
                case KeyCode.W:
                    player.GetComponent<Animator>().SetInteger("direction", (int)Directions.PlayerAnimDirections.up);
                    target = player.transform.position + Vector3.up;
                    break;
                case KeyCode.A:
                    player.GetComponent<Animator>().SetInteger("direction", (int)Directions.PlayerAnimDirections.left);
                    target = player.transform.position + Vector3.left;
                    break;
                case KeyCode.S:
                    player.GetComponent<Animator>().SetInteger("direction", (int)Directions.PlayerAnimDirections.down);
                    target = player.transform.position + Vector3.down;
                    break;
                case KeyCode.D:
                    player.GetComponent<Animator>().SetInteger("direction", (int)Directions.PlayerAnimDirections.right);
                    target = player.transform.position + Vector3.right;
                    break;
            }
            tweener.AddTween(player.transform, 
                                player.transform.position, 
                                target,
                                10.0f/movementSpeed 
                                );
        }
        else { // IF NOT WALKABLE

        }
    }

    bool IsWalkable() {
        // Check if adjacent tile is walkable, then override currentInput <70%>
        
        //Debug.Log(playerPositionInTilemap);

        Tile adjacentTile = null;
        Vector3 target = player.transform.position;

        switch(lastInput) {
            case KeyCode.W:
                target = player.transform.position + Vector3.up;
                break;
            case KeyCode.A:
                target = player.transform.position + Vector3.left;
                break;
            case KeyCode.S:
                target = player.transform.position + Vector3.down;
                break;
            case KeyCode.D:
                target = player.transform.position + Vector3.right;
                break;
        }
        adjacentTile = tiles.GetTile<Tile>(tiles.WorldToCell(target));
        if(adjacentTile != null) return false;
        
        return true; // Check if there is no obstacle, true if obstacle is found
    }
}
