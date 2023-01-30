using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ReloadScene : MonoBehaviour
{
    public GameController gameController;
    public Button nextIslandButton;
    public GameObject replayButton;
    public TextMeshProUGUI winText;
    public GameObject startSquare;
    public int mapSeed;


    private void Update()
    {
        SetActive();
    }

    public void SetActive()
    {
        if (gameController.currentIslandScore >= gameController.islandScoreGoal)
        {
            nextIslandButton.interactable = true;
        }
        if (gameController.islandNr >= gameController.islandWinNr)
        {
            nextIslandButton.gameObject.SetActive(false);
            winText.gameObject.SetActive(true);
            replayButton.SetActive(true);
        }
    }

    public void NextIslandClick()
    {
        
        gameController.islandNr++;
        int islandScore = gameController.currentIslandScore;
        gameController.currentIslandScore = islandScore - gameController.islandScoreGoal;
        gameController.islandScoreGoal = gameController.islandScoreGoal + (gameController.islandScoreGoal/2);
        nextIslandButton.interactable = false;
        mapSeed = Random.Range(0, 1000);
        gameController.mapSeed = mapSeed;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void PlayAgainClick()
    {
        winText.gameObject.SetActive(false);
        gameController.islandScoreGoal = 0;
        gameController.islandNr = 0;
        gameController.islandScoreGoal = 1000;
        gameController.currentIslandScore = 0;
        gameController.islandWinNr = Random.Range(3, 10);
        replayButton.SetActive(false);
        nextIslandButton.gameObject.SetActive(true);
        mapSeed = Random.Range(0, 1000);
        gameController.mapSeed = mapSeed;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();

        if (gameController.islandNr == 1)
        {
            StartCoroutine(DisplayStart());
        }
    }

    IEnumerator DisplayStart()
    {
        startSquare.SetActive(true);
        startSquare.GetComponentInChildren<TextMeshProUGUI>().text =
            "Your goal is to reach island nr " + gameController.islandWinNr;
        yield return new WaitForSecondsRealtime(3);
        startSquare.SetActive(false);
    }
}
