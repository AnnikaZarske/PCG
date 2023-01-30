using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[Serializable]

public class DiceRoll : MonoBehaviour
{
    private Sprite[] diceSides;
    public SpriteRenderer rend;
    public int finalSide = 0;
    public string diceType;
    public RollDice rollDice;
    
    private void Start()
    {
        diceSides = Resources.LoadAll<Sprite>("DiceSides/");
        rollDice = FindObjectOfType<RollDice>();
    }
    
    private IEnumerator RollTheDice()
    {
        int randomDiceSide = 0;
        
        for (int i = 0; i <= 20; i++)
        {
            randomDiceSide = Random.Range(0, 5);
            rend.sprite = diceSides[randomDiceSide];
            yield return new WaitForSeconds(0.05f);
        }

        finalSide = randomDiceSide + 1;
        DiceType(finalSide);
        rollDice.counter++;
    }

    void DiceType(int finalDiceSide)
    {
        if (finalDiceSide == 1)
            diceType = "Grass";
        if (finalDiceSide == 2)
            diceType = "Forest";
        if (finalDiceSide == 3)
            diceType = "Beach";
        if (finalDiceSide == 4)
            diceType = "Mountain";
        if (finalDiceSide == 5)
            diceType = "Water";
    }
}
