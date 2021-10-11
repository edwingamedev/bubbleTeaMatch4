using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ScoreController : MonoBehaviour
{
    public TextMeshProUGUI playerScoreText;
    private int playerScore;

    public void ResetScore()
    {
        playerScore = 0;
        UpdateScore();
    }

    public void AddPoints(int amount)
    {
        playerScore += amount;

        UpdateScore();
    }

    private void UpdateScore()
    {
        playerScoreText.text = playerScore.ToString("00000000");
    }
}
