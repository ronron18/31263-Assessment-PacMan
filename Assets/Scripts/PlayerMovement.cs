using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public Vector3[] positions;
    public int direction;

    // Start is called before the first frame update
    void Start()
    {
        positions = new Vector3[4];
        positions[0] = new Vector3(-9.0f, 5.0f, 0.0f);
        positions[1] = new Vector3(-4.0f, 5.0f, 0.0f);
        positions[2] = new Vector3(-4.0f, 1.0f, 0.0f);
        positions[3] = new Vector3(-9.0f, 1.0f, 0.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
