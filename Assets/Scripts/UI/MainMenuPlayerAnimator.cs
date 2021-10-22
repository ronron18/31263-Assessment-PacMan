using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayerAnimator : MonoBehaviour
{
    [SerializeField] private Vector3[] positions;
    public int direction = 2;   // Initial direction
    private Tweener tweener;

    [SerializeField] private float speed = 10.0f;  // Speed, Movement duration depends on distance.
    private float duration = 0;

    // Start is called before the first frame update
    void Start()
    {
        tweener = GameObject.FindWithTag("Manager").GetComponent<Tweener>();
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
            gameObject.GetComponent<Animator>().SetInteger("direction", direction);
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
