using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class GameController : MonoBehaviour
{
    public int mapSeed;
    public int totalScore;
    public int islandNr = 1;
    public int islandWinNr;
    public int islandScoreGoal = 1000;
    public int currentIslandScore;
    

    private void Awake()
    {
        islandWinNr = Random.Range(3, 10);
        var randomizer = new System.Random();
        mapSeed = randomizer.Next(0, 1000);
        UnityEngine.Random.InitState(mapSeed);
        
        int numGameControllers = FindObjectsOfType<GameController>().Length;
        if (numGameControllers != 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}
