using System;
using System.Collections;
using System.Collections.Generic;
using Landscape;
using TMPro;
using UnityEngine;

public class HexCell : MonoBehaviour
{
    public HexCoordinates coordinates;
    [NonSerialized] public Color color;
    public Sprite hoverSprite;
    public Sprite defaultSprite;
    public GameObject grassValue, waterValue, mountainValue, forestValue, beachValue;
    public TextMeshProUGUI grassValueText, waterValueText, mountainValueText, forestValueText, beachValueText, availablePointsText;
    public HexGrid hexGrid;
    private GameObject hexSpriteChild;
    private SpriteRenderer hexSpriteRend;
    public bool clicked = false;
    public Dictionary<LandscapeKind, float> landscapeColors;
    public int availablePoints;

    private void Awake()
    { 
        grassValue = GameObject.Find("GrassValue"); 
        grassValueText = grassValue.GetComponent<TextMeshProUGUI>();
        
        waterValue = GameObject.Find("WaterValue");
        waterValueText = waterValue.GetComponent<TextMeshProUGUI>();
        
        mountainValue = GameObject.Find("MountainValue");
        mountainValueText = mountainValue.GetComponent<TextMeshProUGUI>();
        
        forestValue = GameObject.Find("ForestValue");
        forestValueText = forestValue.GetComponent<TextMeshProUGUI>();
        
        beachValue = GameObject.Find("BeachValue");
        beachValueText = beachValue.GetComponent<TextMeshProUGUI>();

        availablePointsText = GameObject.Find("AvailablePoints").GetComponent<TextMeshProUGUI>();
        
        hexSpriteChild = gameObject.transform.Find("HexChild").gameObject;
        hexSpriteRend = hexSpriteChild.GetComponent<SpriteRenderer>();

        hexGrid = FindObjectOfType<HexGrid>();
    }

    private void OnMouseEnter()
    {
        if (!clicked)
        {
            landscapeColors = LandscapeUnderCell(hexGrid.camera, hexGrid.landscapeList, hexGrid.mapGeneratorPreview.mapTexture);
            hexSpriteRend.sprite = hoverSprite;
            CalcPoints();
            UpdateText();
        }
    }

    private void OnMouseExit()
    {
        if (!clicked)
        {
            landscapeColors.Clear(); 
            hexSpriteRend.sprite = defaultSprite;
        }
    }

    public void CalcPoints()
    {
        float landscapeValue = 0;
        int diceValue = 0;
        availablePoints = 0;

        if (landscapeColors.TryGetValue(LandscapeKind.Grass, out landscapeValue)) {
            diceValue = hexGrid.diceValues.grassValue;
            if (landscapeValue <= diceValue)
                availablePoints += (int)landscapeValue;
            else
                availablePoints += diceValue;
        }
        if (landscapeColors.TryGetValue(LandscapeKind.Water, out landscapeValue)) {
            diceValue = hexGrid.diceValues.waterValue;
            if (landscapeValue <= diceValue)
                availablePoints += (int)landscapeValue;
            else
                availablePoints += diceValue;
        }
        if (landscapeColors.TryGetValue(LandscapeKind.Forest, out landscapeValue)) {
            diceValue = hexGrid.diceValues.forestValue;
            if (landscapeValue <= diceValue)
                availablePoints += (int)landscapeValue;
            else
                availablePoints += diceValue;
        }
        if (landscapeColors.TryGetValue(LandscapeKind.Mountain, out landscapeValue)) {
            diceValue = hexGrid.diceValues.mountainValue;
            if (landscapeValue <= diceValue)
                availablePoints += (int)landscapeValue;
            else
                availablePoints += diceValue;
        }
        if (landscapeColors.TryGetValue(LandscapeKind.Beach, out landscapeValue)) {
            diceValue = hexGrid.diceValues.beachValue;
            if (landscapeValue <= diceValue)
                availablePoints += (int)landscapeValue;
            else
                availablePoints += diceValue;
        }

        if (hexGrid.diceValues.rolledDice)
        {
            availablePointsText.transform.position = this.transform.position;
            availablePointsText.text = "+ " + availablePoints;
        }
    }
    
    private void UpdateText()
    {
        float currentValue = 0;

        if(landscapeColors.TryGetValue(LandscapeKind.Grass, out currentValue))
            grassValueText.text = "Grass: " + currentValue;
        else 
            grassValueText.text = "Grass: 0";
        
        if(landscapeColors.TryGetValue(LandscapeKind.Water, out currentValue))
            waterValueText.text = "Water: " + currentValue;
        else 
            waterValueText.text = "Water: 0";
        
        if(landscapeColors.TryGetValue(LandscapeKind.Forest, out currentValue))
            forestValueText.text = "Forest: " + currentValue;
        else 
            forestValueText.text = "Forest: 0";
        
        if(landscapeColors.TryGetValue(LandscapeKind.Mountain, out currentValue))
            mountainValueText.text = "Mountain: " + currentValue;
        else 
            mountainValueText.text = "Mountain: 0";
        
        if(landscapeColors.TryGetValue(LandscapeKind.Beach, out currentValue))
            beachValueText.text = "Beach: " + currentValue;
        else 
            beachValueText.text = "Beach: 0";
    }
    
    public Dictionary<LandscapeKind, float> LandscapeUnderCell(Camera camera, LandscapeList landscapeList, Texture2D texture2d)
    {
        var landscapeColorsCount = new Dictionary<LandscapeKind, int>();
        var landscapeColorsPercent = new Dictionary<LandscapeKind, float>();
        var cellWorldPos = this.transform.position;
        var cellScreenPos = camera.WorldToScreenPoint(cellWorldPos);
        cellScreenPos = cellScreenPos / 1.5f;
        
        //TODO: correct texture coordinate calculation
        float outerRadius = (HexMetrics.outerRadius * 2.5f) * 2f;
        var div = 20;
        float delta = outerRadius / div;
        for (int x = -div/2; x < div/2; x++) {
            for (int y = -div/2; y < div/2; y++) {
                float testPosX = cellScreenPos.x + x * delta + delta/2;
                float testPosY = cellScreenPos.y + y * delta + delta/2;
                //texture2d.SetPixel((int)testPosX, (int)testPosY, Color.black);
                float dis = Mathf.Sqrt(Mathf.Pow(cellScreenPos.x - testPosX, 2f) + Mathf.Pow(cellScreenPos.y - testPosY, 2f));
                if (dis <= HexMetrics.innerRadius * 2.5f) {
                    //texture2d.SetPixel((int)testPosX, (int)testPosY, Color.black);
                    LandscapeKind landscapeKind = landscapeList.GetLandscape(texture2d.GetPixel((int)testPosX, (int)testPosY));
                    
                    int currentCount;
                    landscapeColorsCount.TryGetValue(landscapeKind, out currentCount);
                    landscapeColorsCount[landscapeKind] = currentCount + 1;
                }
            }
        }
        //texture2d.Apply();
        
        float totalSum = 0;
        foreach (KeyValuePair<LandscapeKind, int> col in landscapeColorsCount) {
            if (col.Key != LandscapeKind.Empty) { 
                totalSum += col.Value;
            }
        }
        foreach (KeyValuePair<LandscapeKind, int> col in landscapeColorsCount) {
            if (col.Key != LandscapeKind.Empty) { 
                landscapeColorsPercent[col.Key] = Mathf.Round((col.Value / totalSum) * 100);
            }
        }

        return landscapeColorsPercent;
        /*foreach (KeyValuePair<LandscapeKind, float> col in landscapeColorsPercent) {
            Debug.Log ("Key " + col.Key + " Value: " + col.Value);
            
        }*/
    }
}
