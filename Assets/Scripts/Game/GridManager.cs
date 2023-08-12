using System.Collections.Generic;
using UnityEngine;

public class GridManager
{
    private int rows;
    private int columns;
    private float gaps = 0;
    private float squareScale = 2;
    private float everySquareOffset = 0.0f;
    private Vector2 startPosition = new Vector2(0.0f, 0.0f);
    private Vector2 _offset = new Vector2(0.0f, 0.0f);
    private List<GameObject> _gridSquares = new List<GameObject>();
    private GameObject gridSquarePrefab;
    private bool[] pieces;
    private Transform parentTransform;

    public List<GameObject> GridSquares => _gridSquares;

    public GridManager(GameObject gridSquarePrefab, Transform parentTransform)
    {
        this.gridSquarePrefab = gridSquarePrefab;
        this.parentTransform = parentTransform;
    }

    public bool[] CreateGrid(int level, TextAsset jsonFile)
    {
        if(level < 13)
        {
            SetGrid(jsonFile, level);
            SpawnGridSquares();
            SetGridSquaresPosition();
        }
        return pieces;
    }

    public void ClearGrid()
    {
        foreach (GameObject square in _gridSquares)
        {
            Object.Destroy(square);
        }
        _gridSquares.Clear();
    }

    private void SetGrid(TextAsset jsonFile, int level)
    {
        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(jsonFile.text);
        rows = data.gridSize[level];
        columns = rows;
        pieces = new bool[rows * rows];

        for (int i = 0; i < (rows * rows); i++)
        {
            pieces[i] = false;
        }
    }

    public bool CheckIfCompleted()
    {
        for (int i = 0; i < (rows * rows); i++)
        {
            if (pieces[i] == false)
            {
                return false;
            }
        }
        return true;
    }

    public void SpawnGridSquares()
    {
        int squareIndex = 0;
        for (int row = 0; row < rows; ++row)
        {
            for (int column = 0; column < columns; ++column)
            {
                GameObject newGridSquare = GameObject.Instantiate(gridSquarePrefab, parentTransform);
                newGridSquare.transform.localScale = new Vector3(squareScale, squareScale, squareScale);
                newGridSquare.GetComponent<GridSquareScript>().SetImage();
                _gridSquares.Add(newGridSquare);
                squareIndex++;
            }
        }
    }

    private void SetGridSquaresPosition()
    {
        int columnNumber = 0;
        int rowNumber = 0;
        Vector2 squareGapNumber = new Vector2(0.0f, 0.0f);
        bool rowMoved = false;

        _offset.x = 100 * 2 + everySquareOffset;
        _offset.y = 100 * 2 + everySquareOffset;

        foreach (GameObject square in _gridSquares)
        {
            if (columnNumber + 1 > columns)
            {
                squareGapNumber.x = 0;
                columnNumber = 0;
                rowNumber++;
                rowMoved = false;
            }

            float posXOffset = _offset.x * columnNumber + (squareGapNumber.x * gaps);
            float posYOffset = _offset.y * rowNumber + (squareGapNumber.y * gaps);

            if (columnNumber > 0 && columnNumber % 3 == 0)
            {
                squareGapNumber.x++;
                posXOffset += gaps;
            }

            if (columnNumber == 0)
            {
                squareGapNumber.x++;
                posXOffset += gaps;
            }

            if (rowNumber > 0 && rowNumber % 3 == 0 && rowMoved == false)
            {
                rowMoved = true;
                squareGapNumber.y++;
                posYOffset += gaps;

            }

            square.GetComponent<RectTransform>().localPosition = new Vector3(startPosition.x + posXOffset, startPosition.y + posYOffset, 0.0f);

            columnNumber++;
        }
    }
}
