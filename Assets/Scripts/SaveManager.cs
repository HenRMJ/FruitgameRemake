using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    public GameMode Mode { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("You have another instance of the save manager in the scene");
            Destroy(gameObject);
            return;
        }

        Instance = this;

        if (ES3.KeyExists("mode"))
        {
            Mode = GetGameMode();
        }
        else
        {
            Mode = GameMode.Classic;
        }
    }

    public void SaveHighScore(int score)
    {
        if (score > GetHighScore())
        {
            ES3.Save("highScore", score);
        }
    }

    public void SaveGameMode(GameMode mode)
    {
        ES3.Save("mode", mode);
    }

    public void SaveGameMode(string mode)
    {
        switch(mode)
        {
            case "Classic":
                SaveGameMode(GameMode.Classic);
                break;
            case "Endless":
                SaveGameMode(GameMode.Endless);
                break;

        }
    }

    private GameMode GetGameMode()
    {
        if (ES3.KeyExists("mode"))
        {
            return (GameMode)ES3.Load("mode");
        }

        return GameMode.Classic;
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
