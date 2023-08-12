using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
[System.Serializable]

public class ShapeDataScript : ScriptableObject
{
    [System.Serializable]
    public class Row
    {
        public bool[] column;
        private int _size = 0;

        public Row() { }

        public Row(int size)
        {
            CreateRow(size);
        }

        public void CreateRow(int size)
        {
            _size = size;
            column = new bool[_size];
            ClearRow();
        }

        public void ClearRow()
        {
            for(int i = 0; i < _size; i++)
            {
                column[i] = false;

            }
        }
    }

    public int columns = 0;
    public int rows = 0;

    public Row[] board;

    public void Clear()
    {
        for(var i = 0; i < rows; i++)
        {
            board[i].ClearRow();
        }
    }

    public void CreateNewBoard()
    {
        board = new Row[rows];
        for(var i = 0; i < rows; i++)
        {
            board[i] = new Row(columns);
        }
    }

     public void ColorGenerator(ShapeDataScript currentShapeData, List<GameObject> _currentShape)
    {
        if (currentShapeData.name == "1")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(204f, 0f, 0f);
            }
        }
        if (currentShapeData.name == "2")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(0, 153, 255);
            }
        }
        if (currentShapeData.name == "3")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(0, 204, 0);
            }
        }
        if (currentShapeData.name == "4")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(102, 0, 153);
            }
        }
        if (currentShapeData.name == "5")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(204, 51, 204);
            }
        }
        if (currentShapeData.name == "6")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(51, 204, 204);
            }
        }
        if (currentShapeData.name == "7")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(255, 204, 0);
            }
        }
        if (currentShapeData.name == "8")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(255, 102, 0);
            }
        }
        if (currentShapeData.name == "9")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(0, 0, 204);
            }
        }
        if (currentShapeData.name == "10")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(255, 102, 102);
            }
        }
        if (currentShapeData.name == "11")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(51, 204, 204);
            }
        }
        if (currentShapeData.name == "12")
        {
            for (int i = 0; i < _currentShape.Count; i++)
            {
                var square = _currentShape[i].GetComponent<Image>();
                square.color = new Color(204, 0, 0);
            }
        }
    }
    
}
