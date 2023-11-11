using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public event EventHandler OnLostGame;

    [SerializeField] private TMP_Text scoreText, highScoreText;

    private int score;
    private bool lost;
    private bool newHighScore;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("You have another instance of the score manager");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Start()
    {
        UpdateText();
    }

    public void IncreaseScore(int valueToIncrease)
    {
        if (lost) return;

        score += valueToIncrease;

        CheckHighscore();
        UpdateText();
    }

    private void UpdateText()
    {
        scoreText.text = score.ToString();
        highScoreText.text = SaveManager.Instance.GetHighScore().ToString();
    }

    public void LostGame()
    {
        lost = true;
        SaveManager.Instance.SaveScore(score);
        score = 0;
        OnLostGame?.Invoke(this, EventArgs.Empty);
        UpdateText();
    }

    private void CheckHighscore()
    {
        if (score > SaveManager.Instance.GetHighScore())
        {
            SaveManager.Instance.SaveScore(score);

            if (!newHighScore)
            {
                newHighScore = true;
                SoundManager.Instance.PlaySound("Highscore");
            }
        }        
    }

    public bool HasLost() => lost;
}
