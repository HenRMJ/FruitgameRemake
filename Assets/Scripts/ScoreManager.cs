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
        highScoreText.text = MySaveManager.Instance.GetHighScore(MySaveManager.Instance.Mode).ToString();
    }

    public void LostGame()
    {
        lost = true;
        Fruit highestFruit = Fruit.Cherry;
        // youLoseSound.Play();

        

        // Checking highest fruit in the game
        foreach(BaseFruit fruit in FindObjectsOfType<BaseFruit>())
        {
            if (fruit.GetFruitType() > highestFruit)
            {
                highestFruit = fruit.GetFruitType();
            }

            if (highestFruit == Fruit.Pumpkin) break;
        }

        // Saving
        GameAttempt attempt = new GameAttempt(score, fruitsCombined, highestFruit,MySaveManager.Instance.Mode);
        MySaveManager.Instance.SaveAttempt(attempt);
        MySaveManager.Instance.SaveHighScore(score);

        OnLostGame?.Invoke(this, EventArgs.Empty);
        UpdateText();
    }

    private void CheckHighscore()
    {
        if (score > MySaveManager.Instance.GetHighScore(MySaveManager.Instance.Mode))
        {
            MySaveManager.Instance.SaveHighScore(score);

            if (!newHighScore)
            {
                newHighScore = true;
                // soundHighScore.Play();
            }
        }        
    }

    public bool HasLost() => lost;
}
