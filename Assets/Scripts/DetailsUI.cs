using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class DetailsUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI combinedFruitsText, largestFruitText, scoreText, highScoreText;

    private GameAttempt attempt;

    private void Start()
    {
        attempt = SaveManager.Instance.GetLastAttempt();

        combinedFruitsText.text = attempt.GetFruitsCombined().ToString();
        largestFruitText.text = attempt.GetLargestFruit().ToString();
        scoreText.text = attempt.GetScore().ToString();
        highScoreText.text = SaveManager.Instance.GetHighScore().ToString();
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
