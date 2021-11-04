using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovationPelletController : MonoBehaviour
{
    private GameObject[] pellets;
    private GameObject[] powerUps;
    // Start is called before the first frame update
    void Start()
    {
        pellets = GameObject.FindGameObjectsWithTag("Pellet");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
