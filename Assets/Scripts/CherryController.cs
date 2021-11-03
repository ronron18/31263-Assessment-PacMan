using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// No it's not a cherry, it's a carrot now :)
public class CherryController : MonoBehaviour
{
    private float maximumPlayableWidth;
    private float minimumPlayableWidth;
    private float maximumPlayableHeight;
    private float minimumPlayableHeight;
    [SerializeField] private Vector2 midPoint;
    [SerializeField] private GameObject cherryPrefab;
    private Tweener tweener;
    private Camera mainCamera;
    private GameObject cherry = null;

    [SerializeField] private float duration = 5.0f;
    private float timer = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        tweener = GetComponent<Tweener>();
        minimumPlayableWidth = mainCamera.ScreenToWorldPoint(Vector2.zero).x - 1.0f;
        maximumPlayableWidth = mainCamera.ScreenToWorldPoint(new Vector2(Screen.width, 0)).x + 1.0f;
        minimumPlayableHeight = mainCamera.ScreenToWorldPoint(Vector2.zero).y - 1.0f;
        maximumPlayableHeight = mainCamera.ScreenToWorldPoint(new Vector2(0, Screen.height)).y + 1.0f;
        midPoint = new Vector2((maximumPlayableWidth+minimumPlayableWidth)/2, (maximumPlayableHeight+minimumPlayableHeight)/2);
        //Debug.Log(midPoint);
    }

    // Update is called once per frame
    void Update()
    {
        if(timer >= 10.0f)  // Every 10 Seconds
        {
            GenerateCherry();
            timer -= 10.0f; // "resets" the timer
        }
        if(cherry != null) {
            if(!tweener.TweenExists(cherry.transform)) Destroy(cherry);
        }

        timer += Time.deltaTime;
        //Debug.Log("Tweener timer: " + timer);
    }

    // bonus "cherry" spawning, idk how this is supposed to be implemented but i tried :)
    void GenerateCherry()
    {
        bool yAxisSelected = (Random.value > 0.5f); // left/right movement if true, y axis position will be selected
        bool negativeDirection = (Random.value > 0.5f); // Move to the negative direction if true.
        cherry = Instantiate(cherryPrefab);
        float startPosition = 0.0f; 
        Vector2 startPositionVector = Vector2.zero;

        if(yAxisSelected)
        {
            startPosition = Random.Range(minimumPlayableHeight, maximumPlayableHeight);    // Selects a random point in the Y axis off-screen.
            if(negativeDirection)
            {
                // L-R
                startPositionVector = new Vector2(startPosition, maximumPlayableHeight);
            }
            else
            {
                // R-L
                startPositionVector = new Vector2(startPosition, minimumPlayableHeight);
            }
        }
        else
        {
            startPosition = Random.Range(minimumPlayableWidth, maximumPlayableWidth);
            if(negativeDirection)
            {
                // T-B
                startPositionVector = new Vector2(maximumPlayableWidth, startPosition);
            }
            else
            {
                // B-T
                startPositionVector = new Vector2(minimumPlayableWidth, startPosition);
            }
            
        }
        Vector2 halfDistanceVector = startPositionVector - midPoint; // Stores direction, and distance
        Vector2 endPositionVector = midPoint - halfDistanceVector;
        //Debug.Log("startPositionVector " + startPositionVector);
        //Debug.Log("endPositionVector " + endPositionVector);

        tweener.AddTween(cherry.transform, startPositionVector, endPositionVector, duration);
        
    }
}
