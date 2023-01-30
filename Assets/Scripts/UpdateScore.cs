using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UpdateScore : MonoBehaviour
{
    private GameController gameController;
    public TextMeshProUGUI text;
    public int score;

    private void Awake()
    {
        gameController = FindObjectOfType<GameController>();
    }

    private void Update()
    {
        score = gameController.totalScore;
        text.text = "Score: " + score.ToString();
    }
}
