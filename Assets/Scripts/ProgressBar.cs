using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public int maximum;
    public int current;
    public Image fillImage;
    public GameController gameController;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    // Update is called once per frame
    void Update()
    {
        maximum = gameController.islandScoreGoal;
        current = gameController.currentIslandScore;
        GetCurrentFill();
    }

    private void GetCurrentFill()
    {
        float fillAmount = (float)current / (float)maximum;
        fillImage.fillAmount = fillAmount;
    }
}
