using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI scoreText;

    float playerScore = 0f;

    float highestScore = 0f;

    bool count = true;

    private void Start()
    {
        scoreText.text = "Score: 0";
    }

    public void Update()
    {
        if (count)
        {
            playerScore += Time.deltaTime * 2;
            scoreText.text = "Score: " + Mathf.RoundToInt(playerScore) + " (HI: "+ Mathf.RoundToInt(highestScore) + ")";
        }
    }

    public void Restart()
    {
        highestScore = playerScore > highestScore? playerScore : highestScore;
        playerScore = 0;
        count = true;
    }

    public void StopCounting()
    {
        count = false;
    }
}
