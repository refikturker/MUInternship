using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ShapeScript : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IInitializePotentialDragHandler
{
    public GameObject shapeImage;
    public Vector3 shapeSelectedScale;

    private CanvasGroup canvasGroup;
    [HideInInspector]
    public ShapeDataScript currentShapeData;
        
    public int TotalSquareNumber { get; set;}


    public List<GameObject> _currentShape = new List<GameObject>();
    private Vector3 _shapeStartScale;
    private RectTransform _transform;
    private RectTransform _initialTransform;

    private Vector3 _startPosition;
    private Vector2 _pointerOffset;
    private List<Vector2> _shapeOffsets = new List<Vector2>();

    public GridScript gridScript;


    public void Awake()
    {
        _shapeStartScale = this.GetComponent<RectTransform>().localScale;
        _transform = this.GetComponent<RectTransform>();
        _initialTransform = _transform;
        _startPosition = _transform.position;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public bool IsOnStartPosition()
    {
        return _transform.position == _startPosition;
    }

    public void DeleteSquareImages()
    {
        foreach (GameObject square in _currentShape)
        {
            Destroy(square);
        }

        _currentShape.Clear();
    }

    public void CreateShape(ShapeDataScript shapeData)
    {
        currentShapeData = shapeData;
        TotalSquareNumber = GetNumberOfSquares(shapeData);

        while (_currentShape.Count < TotalSquareNumber)
        {
            _currentShape.Add(Instantiate(shapeImage, transform) as GameObject);
        }        

        foreach (var square in _currentShape)
        {
            square.gameObject.SetActive(false);
        }

        var squareRect = shapeImage.GetComponent<RectTransform>();
        var distance = new Vector2(squareRect.rect.width * squareRect.localScale.x, squareRect.rect.height * squareRect.localScale.y);

        int currentIndexInList = 0;

        for (var row = 0; row < shapeData.rows; row++)
        {
            for (var column = 0; column < shapeData.columns; column++)
            {
                if (shapeData.board[row].column[column])
                {
                    if (currentIndexInList < _currentShape.Count)
                    {
                        _currentShape[currentIndexInList].SetActive(true);
                        _currentShape[currentIndexInList].GetComponent<RectTransform>().localPosition =
                            new Vector2(GetXPosition(shapeData, column, distance), GetYPosition(shapeData, row, distance));
                    }

                    else
                    {
                        var newSquare = Instantiate(shapeImage, transform) as GameObject;
                        newSquare.SetActive(true);
                        newSquare.GetComponent<RectTransform>().localPosition =
                        new Vector2(GetXPosition(shapeData, column, distance), GetYPosition(shapeData, row, distance));
                        _currentShape.Add(newSquare);
                    }                    

                    currentIndexInList++;
                }
            }
        }

        while (_currentShape.Count > TotalSquareNumber)
        {
            var square = _currentShape[_currentShape.Count - 1];
            _currentShape.Remove(square);
            Destroy(square);
        }

        _shapeOffsets.Clear();
        foreach (var square in _currentShape)
        {
            _shapeOffsets.Add(square.GetComponent<RectTransform>().localPosition);
        }

        currentShapeData.ColorGenerator(currentShapeData, _currentShape);
    }

    private float GetYPosition(ShapeDataScript shapeData, int row, Vector2 distance)
    {
        float shiftOnY = 0f;

        if (shapeData.rows > 1)
        {
            if (shapeData.rows == 4)
            {
                shiftOnY = (row - 1.5f) * distance.y;
            }
            else
            {
                if (shapeData.rows % 2 != 0)
                {
                    var middleSquareIndex = (shapeData.rows - 1) / 2;
                    var multiplier = (shapeData.rows - 1) / 2;

                    if (row < middleSquareIndex)
                    {
                        shiftOnY = distance.y * 1;
                        shiftOnY *= multiplier;
                    }
                    else if (row > middleSquareIndex)
                    {
                        shiftOnY = distance.y * -1;
                        shiftOnY *= multiplier;
                    }
                }

                else
                {
                    var middleSquareIndex1 = (shapeData.rows == 2) ? 0 : (shapeData.rows - 2);
                    var middleSquareIndex2 = (shapeData.rows == 2) ? 1 : (shapeData.rows / 2);
                    var middleSquareIndex3 = (shapeData.rows == 4) ? 2 : (shapeData.rows * -1);

                    var multiplier = shapeData.rows / 2;
                    var secondMultiplier = shapeData.rows * -1;


                    if (row == middleSquareIndex1 || row == middleSquareIndex2 || row == middleSquareIndex3)
                    {
                        if (row == middleSquareIndex2)
                        {
                            shiftOnY = (distance.y / 2) * -1;
                        }
                        if (row == middleSquareIndex1)
                        {
                            shiftOnY = (distance.y / 2);
                        }
                        if (row == middleSquareIndex3)
                        {
                            shiftOnY = (distance.y / 2) * -1;
                            shiftOnY *= secondMultiplier / 4;
                        }
                    }


                    if (row < middleSquareIndex1 && row < middleSquareIndex2)
                    {
                        shiftOnY = distance.y * -1;
                        shiftOnY *= multiplier;

                    }
                    else if (row > middleSquareIndex1 && row > middleSquareIndex2)
                    {
                        shiftOnY = distance.y * -1;
                        shiftOnY *= multiplier;
                    }
                }
            }
        }

        return shiftOnY;
    }

    private float GetXPosition(ShapeDataScript shapeData, int column, Vector2 distance)
    {
        float shiftOnX = 0f;

        if (shapeData.columns > 1)
        {
            if (shapeData.columns % 2 != 0)
            {
                var middleSquareIndex = (shapeData.columns - 1) / 2;
                var multiplier = (shapeData.columns - 1) / 2;

                if (column < middleSquareIndex)
                {
                    shiftOnX = distance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex)
                {
                    shiftOnX = distance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
            else
            {
                var middleSquareIndex1 = (shapeData.columns == 2) ? 0 : (shapeData.columns - 1);
                var middleSquareIndex2 = (shapeData.columns == 2) ? 1 : (shapeData.columns / 2);
                var multiplier = shapeData.columns / 2;

                if (column == middleSquareIndex1 || column == middleSquareIndex2)
                {
                    if (column == middleSquareIndex2)
                    {
                        shiftOnX = distance.x / 2;
                    }
                    if (column == middleSquareIndex1)
                    {
                        shiftOnX = (distance.x / 2) * -1;
                    }
                }
                if (column < middleSquareIndex1 && column < middleSquareIndex2)
                {
                    shiftOnX = distance.x * -1;
                    shiftOnX *= multiplier;
                }
                else if (column > middleSquareIndex1 && column > middleSquareIndex2)
                {
                    shiftOnX = distance.x * 1;
                    shiftOnX *= multiplier;
                }
            }
        }

        return shiftOnX;
    }

    private int GetNumberOfSquares(ShapeDataScript shapeData)
    {
        int number = 0;

        foreach (var rowData in shapeData.board)
        {
            foreach (var active in rowData.column)
            {
                if (active)
                {
                    number++;
                }
            }
        }
        return number;
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        this.GetComponent<RectTransform>().Rotate(Vector3.forward, 90.0f);
    }

    public void SetPivotOfTheShape()
    {
        if (currentShapeData.name == "2" || currentShapeData.name == "7")
        {
            _transform.pivot = new Vector2(0f, 0.5f);
        }

        if (currentShapeData.name == "4" || currentShapeData.name == "5")
        {
            _transform.pivot = new Vector2(1f, 1f);
        }

        if (currentShapeData.name == "6" || currentShapeData.name == "8")
        {
            _transform.pivot = new Vector2(1f, 0.5f);
        }
        if (currentShapeData.name == "9")
        {
            _transform.pivot = new Vector2(0f, 0.5f);
        }
        if (currentShapeData.name == "10")
        {
            _transform.pivot = new Vector2(0.5f, 0f);
        }
        if (currentShapeData.name == "12")
        {
            _transform.pivot = new Vector2(0.5f, 1f);
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        SetPivotOfTheShape();
        canvasGroup.alpha = .6f;
        canvasGroup.blocksRaycasts = false;

        if (_currentShape.Count > 0)
        {
            var square = _currentShape[_currentShape.Count - 1];
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.GetComponent<RectTransform>().localScale = new Vector3(2, 2, 2);    

        _transform.position = eventData.position + _pointerOffset;

    }


    public void OnEndDrag(PointerEventData eventData)
    {       
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;

        Collider2D[] colliders = Physics2D.OverlapBoxAll(transform.position, new Vector2(100,100), 0);

        List<Collider2D> filteredColliders = new List<Collider2D>();
        
        foreach (Collider2D collider in colliders)
        {
            if (!collider.CompareTag("GridSquareTag"))
            {
                filteredColliders.Add(collider);
            }
        }

        int collidersCount = Mathf.Max(filteredColliders.Count, 1); // to prevent that sometimes filteredColliders.Count = 0 

        foreach (Collider2D collider in filteredColliders)
        {
            if (collidersCount > 1)
            {
                MoveShapeToStartPosition();
            }
        }
    }

    public void OnInitializePotentialDrag(PointerEventData eventData)
    {
        eventData.useDragThreshold = false;
    }

    public void MoveShapeToStartPosition()
    {
        if(IsOnStartPosition() == false)
        {
            SetToStartPosition();
            this.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.5f);
            this.GetComponent<RectTransform>().Rotate(Vector3.forward, 0.0f);
            this.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
        }
    }

    public Vector3 SetToStartPosition()
    {
        return (this.GetComponent<RectTransform>().position = _startPosition);
    }

}



