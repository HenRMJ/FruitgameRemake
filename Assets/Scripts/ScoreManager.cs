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
    private int fruitsCombined = 0;
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
        BaseFruit.OnFruitCombined += BaseFruit_OnFruitCombined;

        UpdateText();
    }

    private void OnDestroy()
    {
        BaseFruit.OnFruitCombined -= BaseFruit_OnFruitCombined;
    }

    private void BaseFruit_OnFruitCombined(object sender, EventArgs e)
    {
        fruitsCombined++;
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
        Fruit highestFruit = Fruit.Cherry;

        // Checking highest fruit in the game
        foreach(BaseFruit fruit in FindObjectsOfType<BaseFruit>())
        {
            if (fruit.GetFruitType() > highestFruit)
            {
                highestFruit = fruit.GetFruitType();
            }
        }

        // Saving
        GameAttempt attempt = new GameAttempt(score, fruitsCombined, highestFruit);
        SaveManager.Instance.SaveAttempt(attempt);
        SaveManager.Instance.SaveHighScore(score);

        OnLostGame?.Invoke(this, EventArgs.Empty);
        UpdateText();
    }

    private void CheckHighscore()
    {
        if (score > SaveManager.Instance.GetHighScore())
        {
            SaveManager.Instance.SaveHighScore(score);

            if (!newHighScore)
            {
                newHighScore = true;
                SoundManager.Instance.PlaySound("Highscore");
            }
        }        
    }

    public bool HasLost() => lost;
}
