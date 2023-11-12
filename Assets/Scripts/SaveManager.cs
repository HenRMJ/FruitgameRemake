using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("You have another instance of the save manager in the scene");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void SaveHighScore(int score)
    {
        if (score > GetHighScore())
        {
            ES3.Save("highScore", score);
        }
    }

    public void SaveAttempt(GameAttempt attempt)
    {
        ES3.Save("lastAttempt", attempt);
    }

    public GameAttempt GetLastAttempt()
    {
        return (GameAttempt)ES3.Load("lastAttempt");
    }

    public int GetHighScore()
    {
        int returnValue = 0;

        if (ES3.KeyExists("highScore"))
        {
            returnValue = (int)ES3.Load("highScore");
        }

        return returnValue;
    }
}
