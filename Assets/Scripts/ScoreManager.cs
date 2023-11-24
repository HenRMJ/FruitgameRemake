using UnityEngine;
using TMPro;
using System;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    public event EventHandler OnLostGame;

    private FMODUnity.StudioEventEmitter soundHighScore;

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

        soundHighScore = GetComponent<FMODUnity.StudioEventEmitter>();
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
        highScoreText.text = SaveManager.Instance.GetHighScore(SaveManager.Instance.Mode).ToString();
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

            if (highestFruit == Fruit.Pumpkin) break;
        }

        // Saving
        GameAttempt attempt = new GameAttempt(score, fruitsCombined, highestFruit,SaveManager.Instance.Mode);
        SaveManager.Instance.SaveAttempt(attempt);
        SaveManager.Instance.SaveHighScore(score);

        OnLostGame?.Invoke(this, EventArgs.Empty);
        UpdateText();
    }

    private void CheckHighscore()
    {
        if (score > SaveManager.Instance.GetHighScore(SaveManager.Instance.Mode))
        {
            SaveManager.Instance.SaveHighScore(score);

            if (!newHighScore)
            {
                newHighScore = true;
                soundHighScore.Play();
            }
        }        
    }

    public bool HasLost() => lost;
}
