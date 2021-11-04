using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InnovationPelletController : MonoBehaviour
{
    private GameObject[] pellets;
    private GameObject[] powerUps;
    private bool allPelletsConsumed = false;
    private bool allPowerUpsConsumed = false;
    
    // Start is called before the first frame update
    void Start()
    {
        pellets = GameObject.FindGameObjectsWithTag("Pellet");
        powerUps = GameObject.FindGameObjectsWithTag("PowerUp");
        Debug.Log(pellets.Length);
        Debug.Log(powerUps.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if(!allPelletsConsumed)
        {
            foreach(GameObject pellet in pellets)
            {
                if(pellet.activeSelf)
                {
                    allPelletsConsumed = false;
                    break;
                }
                else
                {
                    allPelletsConsumed = true;
                }
            }
        }
        
        if(!allPowerUpsConsumed)
        {
            foreach(GameObject powerUp in powerUps)
            {
                if(powerUp.activeSelf)
                {
                    allPowerUpsConsumed = false;
                    break;
                }
                else
                {
                    allPowerUpsConsumed = true;
                }
            }
        }
        

        if(allPelletsConsumed && allPowerUpsConsumed) ReactivateConsumables();
    }

    void ReactivateConsumables()
    {
        for(int i = 0; i < (pellets.Length/4); i++)
        {
            bool added = false;
            int randomNumber = Random.Range(0, pellets.Length);
            while(!added)
            {
                if(!pellets[randomNumber].activeSelf)
                {
                    added = true;
                    pellets[randomNumber].SetActive(true);
                }
                else
                {
                    randomNumber = Random.Range(0, pellets.Length);
                }
            }
        }

        for(int i = 0; i < (powerUps.Length/2); i++)
        {
            bool added = false;
            int randomNumber = Random.Range(0, powerUps.Length);
            while(!added)
            {
                if(!powerUps[randomNumber].activeSelf)
                {
                    added = true;
                    powerUps[randomNumber].SetActive(true);
                }
                else
                {
                    randomNumber = Random.Range(0, powerUps.Length);
                }
            }
        }

        allPelletsConsumed = false;
        allPowerUpsConsumed = false;
    }
}
