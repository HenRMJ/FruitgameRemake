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
        if (score > GetHighScore(Mode))
        {
            if (Mode == GameMode.Classic)
            {
                ES3.Save("highScore", score);
            } else
            {
                ES3.Save("highScoreEndless", score);
            }
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

    public int GetHighScore(GameMode mode)
    {
        int returnValue = 0;
        
        switch (mode)
        {
            case GameMode.Classic:
                if (ES3.KeyExists("highScore"))
                {
                    returnValue = (int)ES3.Load("highScore");
                }
                break;
            case GameMode.Endless:
                if (ES3.KeyExists("highScoreEndless"))
                {
                    returnValue = (int)ES3.Load("highScoreEndless");
                }
                break;
        }
        

        return returnValue;
    }
}
