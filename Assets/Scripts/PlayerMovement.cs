using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;
    public int direction = 1;
    [SerializeField] private GameObject player;

    private Tweener tweener;

    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[4];
        positions[0] = new Vector3(-9.0f, 5.0f, 0.0f);
        positions[1] = new Vector3(-4.0f, 5.0f, 0.0f);
        positions[2] = new Vector3(-4.0f, 1.0f, 0.0f);
        positions[3] = new Vector3(-9.0f, 1.0f, 0.0f);
        tweener = GetComponent<Tweener>();
    }

    // Update is called once per frame
    void Update()
    {
        if(tweener.AddTween(player.transform, player.transform.position, positions[direction], 1.5f))
        {
            player.GetComponent<Animator>().SetInteger("direction", direction);
            ChangeDirection();
        }
    }

    public void ChangeDirection()
    {
        direction++;
        if(direction >= 4)
        {
            direction = 0;
        }
        
    }
}
