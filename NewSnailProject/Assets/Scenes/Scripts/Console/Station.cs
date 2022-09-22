using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Station : MonoBehaviour
{
    [Header("Stations")]
    public GameObject station_1;
    public GameObject station_2;
    public GameObject station_3;
    public GameObject station_4;
    public GameObject station_5;
    public GameObject station_6;
    public GameObject station_7;
    public GameObject station_8;
    public GameObject station_9;

    public int activationRate = 60;

    public int randNum;


    public int maxNumStations = 1;
    public int numStationsActive = 0;



    void Start()
    {
        DeactivateStations();
        StartCoroutine(StationTimer());
        StartCoroutine(DifficultyLevel());
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    IEnumerator StationTimer()
    {
       
        while (numStationsActive <= maxNumStations)
        {
            ActivateStations();
            numStationsActive++;

            yield return new WaitForSeconds(activationRate);

        }

    }
    IEnumerator DifficultyLevel()
    {
        while (maxNumStations <= 9)
        {
            yield return new WaitForSeconds(activationRate);
            maxNumStations++;
            Debug.Log("test");
        }
        
    }

    public void ActivateStations()
    {
       
        randNum = Random.Range(1, 10);
        if (randNum == 1)
        {
            station_1.SetActive(true);
        }
        else if (randNum == 2)
        {
            station_2.SetActive(true);
        }
        else if (randNum == 3)
        {
            station_3.SetActive(true);
        }
        else if (randNum == 4)
        {
            station_4.SetActive(true);
        }
        else if (randNum == 5)
        {
            station_5.SetActive(true);
        }
        else if (randNum == 6)
        {
            station_6.SetActive(true);
        }
        else if (randNum == 7)
        {
            station_7.SetActive(true);
        }
        else if (randNum == 8)
        {
            station_8.SetActive(true);
        }
        else if (randNum == 9)
        {
            station_9.SetActive(true);
        }
        randNum = 0;
        //numStationsActive++;

        


    }
    private void DeactivateStations()
    {
        station_1.SetActive(false);
        station_2.SetActive(false);
        station_3.SetActive(false);
        station_4.SetActive(false);
        station_5.SetActive(false);
        station_6.SetActive(false);
        station_7.SetActive(false);
        station_8.SetActive(false);
        station_9.SetActive(false);

        numStationsActive = 0;
    }
}
