using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;


public class GameManagerScript : MonoBehaviour
{
    public Text timerText;
    public Text scoreText;
    public Coroutine timerCoroutine;
    public float time;
    public int level;
    public TextAsset jsonFile;
    public GameObject nextLevelButton;
    public GridScript gridScript;

    public void StartCountdownTimer()
    {
        scoreText.enabled = false;

        if (timerCoroutine != null)
        {
            StopCoroutine(timerCoroutine);
        }

        var data = Newtonsoft.Json.JsonConvert.DeserializeObject<Data>(jsonFile.text);
        time = data.time[level];
        UpdateTimerText();
        timerCoroutine = StartCoroutine(UpdateTimerCoroutine());
    }

    
    public IEnumerator UpdateTimerCoroutine()
    {
        while (time >= 0)
        {
            if (nextLevelButton.activeSelf == false)
            {
                UpdateTimerText();
                time--;
                yield return new WaitForSeconds(1f);
            }
            else
            {
                break;
            }
        }
    }

    public void UpdateTimerText()
    {
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        string timerString = string.Format("{0:00}:{1:00}", minutes, seconds);
        timerText.text = timerString;
    }

    public float Score()
    {
        float score = 0;

        float percentage = gridScript.ReturnTruePiecesToCalculateScore() * 100.0f / gridScript.pieces.Length * 1.0f;

        scoreText.enabled = true;

        if (!gridScript.CheckIfCompleted())
        {
            score += percentage;
            scoreText.GetComponent<Text>().text = "Your Score Is " + score.ToString("0");
        }
        else
        {
            score = 150;
            scoreText.GetComponent<Text>().text = "Your Score Is " + score.ToString("0");
        }

        return score;
    }

}
