using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class RollDice : MonoBehaviour
{
    public List<DiceRoll> diceToRoll;
    public TextMeshProUGUI grassNeed, waterNeed, mountainNeed, forestNeed, beachNeed;
    public int waterValue;
    public int grassValue;
    public int mountainValue;
    public int forestValue;
    public int beachValue;
    public int counter;
    public bool rolledDice = false;
    public AudioSource audioSource;
    public AudioClip diceRollSound;

    public void ClickButton()
    {
        counter = 0;
        waterValue = 0;
        grassValue = 0;
        mountainValue = 0;
        forestValue = 0;
        beachValue = 0;
        
        audioSource.PlayOneShot(diceRollSound);
        
        foreach (var dice in diceToRoll)
        {
            dice.StartCoroutine("RollTheDice");
        }
        
        StartCoroutine(UpdateValues());
        rolledDice = true;
    }
    
    IEnumerator UpdateValues()
    {
        yield return new WaitUntil(() => counter >= 5);
        
        foreach (var dice in diceToRoll)
        {
            if (dice.diceType == "Grass")
                grassValue += 20;
            if (dice.diceType == "Water")
                waterValue += 20;
            if (dice.diceType == "Forest")
                forestValue += 20;
            if (dice.diceType == "Mountain")
                mountainValue += 20;
            if (dice.diceType == "Beach")
                beachValue += 20;
        }
    }
    
    private void Update()
    {
        grassNeed.text = "Grass: " + grassValue;
        waterNeed.text = "Water: " + waterValue;
        forestNeed.text = "Forest: " + forestValue;
        mountainNeed.text = "Mountain: " + mountainValue;
        beachNeed.text = "Beach: " + beachValue;
    }
}
