using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GridScript : MonoBehaviour
{
    public int level;
    private bool isLevelCompleted = false;

    public GameObject gridSquare;
    public GameObject nextLevelButton;
    public GameObject retryLevelButton;

    public ShapeStorageScript shapeStorage;
    public TextAsset jsonFile;

    public Text timerText;

    private GridManager gridManager;
    public GameManagerScript gameManagerScript;
    public bool[] pieces; 

    void Start()
    {
        shapeStorage.ShapeCreator();
        CreateGrid();
        gameManagerScript.StartCountdownTimer();        
    }

    private void CreateGrid()
    {
        gridManager = new GridManager(gridSquare, transform);
        pieces = gridManager.CreateGrid(level, jsonFile);

        if (pieces.Length == 9)
        {
            this.GetComponent<RectTransform>().position = new Vector3(580, 960, 0);
        }
        if(pieces.Length == 16)
        {
            this.GetComponent<RectTransform>().position = new Vector3(440, 960, 0);
        }
        if(pieces.Length == 25)
        {
            this.GetComponent<RectTransform>().position = new Vector3(340, 900, 0);
        }
    }

    public int ReturnTruePiecesToCalculateScore()
    {
        int truePiecesCount = 0;

        for (int i = 0; i < pieces.Length; i++)
        {
            if (pieces[i])
            {
                truePiecesCount++;
            }
        }

        return truePiecesCount;
    }

    private void Update()
    {
        if(level < 13)
        {
            LevelEnd();
        }
    }

    public bool CheckIfCompleted()
    {
        return gridManager.CheckIfCompleted();
    }

    private void ClearDeckWithDelay()
    {
        gridManager.ClearGrid();
        shapeStorage.DeleteSquareImagesInShapes();
    }

    public void LevelEnd()
    {
        if (CheckIfCompleted() && !isLevelCompleted)
        {
            gameManagerScript.Score();
            isLevelCompleted = true;
            ClearDeckWithDelay();

            if(level < 12)
            {
                nextLevelButton.SetActive(true);
            }
            else
            {
                gameManagerScript.timerText.enabled = false;
            }
        }

        if (gameManagerScript.time == 0f)
        {
            gameManagerScript.Score();
            gameManagerScript.timerCoroutine = null;
            isLevelCompleted = false;
            Invoke("ClearDeckWithDelay", 1.1f);
            Invoke("RetryLevelButtonSetActiveWithDelay", 1.1f);
        }
    }

    public void RetryLevelButtonSetActiveWithDelay()
    {
        retryLevelButton.SetActive(true);
    }

    public void RetryLevel()
    {
        retryLevelButton.SetActive(false);

        shapeStorage.ShapeCreator();
        CreateGrid();

        gameManagerScript.StartCountdownTimer();
    }

    public void NextLevel()
    {
        nextLevelButton.SetActive(false);

        level++;
        shapeStorage.level++;

        shapeStorage.ShapeCreator();
        CreateGrid();

        gameManagerScript.timerCoroutine = null;
        gameManagerScript.level++;
        isLevelCompleted = false;
        gameManagerScript.StartCountdownTimer();
    }
}
