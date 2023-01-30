using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Landscape;
using TMPro;
using UnityEngine;

public class HexGrid : MonoBehaviour
{
    public int width = 6;
    public int height = 6;
    [NonSerialized] public Color defaultColor = Color.clear;
    [NonSerialized] public Color touchedColor = Color.green;
    public Sprite defaultSprite;
    public Sprite clickedSprite;
    public bool showTextCords;

    public HexCell cellPrefab;
    public TextMeshProUGUI cellLabelPrefab;
    Canvas gridCanvas;
    public Camera camera;
    public MapGeneratorPreview mapGeneratorPreview;
    public RollDice diceValues;
    public GameController gameController;
    public AudioSource audioSource;
    public AudioClip tileClickSound;

    HexCell[] cells;
    HexMesh hexMesh;
    public LandscapeList landscapeList;

    void Awake () {
        gridCanvas = GetComponentInChildren<Canvas>();
        hexMesh = GetComponentInChildren<HexMesh>();
        mapGeneratorPreview = FindObjectOfType<MapGeneratorPreview>();
        landscapeList = new LandscapeList(mapGeneratorPreview);
        diceValues = FindObjectOfType<RollDice>();
        gameController = FindObjectOfType<GameController>();
        audioSource = this.GetComponent<AudioSource>();

        cells = new HexCell[height * width];

        for (int y = 0, i = 0; y < height; y++) {
            for (int x = 0; x < width; x++) {
                CreateCell(x, y, i++);
            }
        }
    }

    void Start () {
        hexMesh.Triangulate(cells);
    }
    
    void Update () {
        if (Input.GetMouseButtonUp(0)) {
            HandleInput();
        }
    }

    void HandleInput () {
        Ray inputRay = camera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(inputRay, out hit)) {
            TouchCell(hit.point);
        }
    }
    
    void TouchCell (Vector3 position)
    {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Y * width + coordinates.Y / 2;
        HexCell cell = cells[index];
        if (!cell.clicked && diceValues.rolledDice)
        {
            GameObject cellChild = cell.gameObject.transform.Find("CellChild").gameObject;
            cellChild.GetComponent<SpriteRenderer>().sprite = clickedSprite;
            //hexMesh.Triangulate(cells);
        
            audioSource.PlayOneShot(tileClickSound);
            cell.clicked = true;
            gameController.totalScore += cell.availablePoints;
            gameController.currentIslandScore += cell.availablePoints;
            diceValues.rolledDice = false;
        }
    }

    void CreateCell (int x, int y, int i) {
        Vector3 position;
        position.x = (x + y * 0.5f - y / 2) * (HexMetrics.innerRadius * 2f);
        position.y = y * (HexMetrics.outerRadius * 1.5f);
        position.z = 0f;

        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, y);
        cell.GetComponentInChildren<SpriteRenderer>().sprite = defaultSprite;

        if (showTextCords)
        {
            TextMeshProUGUI label = Instantiate<TextMeshProUGUI>(cellLabelPrefab);
            label.rectTransform.SetParent(gridCanvas.transform, false);
            label.rectTransform.anchoredPosition = new Vector2(position.x, position.y);
            label.text = cell.coordinates.ToStringOnSeparateLines();
        }
    }
}