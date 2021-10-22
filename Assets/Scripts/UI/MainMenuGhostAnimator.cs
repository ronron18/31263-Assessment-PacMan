using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    The reason main menu ghost animator is separated from the player counterpart is because of the fact that
    the animation is handled differently
*/

public class MainMenuGhostAnimator : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;
    public int direction = 2;       // Initial direction
    private int[] animDirections;   // Direction sequence for ghost animation
    private Tweener tweener;

    [SerializeField] private float speed = 4.0f;  // Speed, Movement duration depends on distance.
    private float duration = 0;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GameObject.FindWithTag("Manager").GetComponent<Tweener>();
        animDirections = new int[] {0,2,1,2}; // Direction sequence for ghost animation
        positions = new Vector3[4];
        positions[0] = new Vector3(5.0f, 3.0f, 0.0f);
        positions[1] = new Vector3(5.0f, -3.0f, 0.0f); 
        positions[2] = new Vector3(-5.0f, -3.0f, 0.0f);
        positions[3] = new Vector3(-5.0f, 3.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        if(!tweener.TweenExists(gameObject.transform)) duration = Vector3.Distance(gameObject.transform.position, positions[direction]) / speed;
        if(tweener.AddTween(gameObject.transform, gameObject.transform.position, positions[direction], duration))
        {
            gameObject.GetComponent<Animator>().SetInteger("direction", animDirections[direction]);
            ChangeDirection();
        }
    }

    public void ChangeDirection()
    {
        direction--;
        if(direction < 0)
        {
            direction = 3;
        }


    }
}
