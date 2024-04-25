using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public static GameSceneManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("A Game Scene Manager already exits inside your scene");
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    public void LoadNewScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadLastGameMode()
    {
        switch (MySaveManager.Instance.Mode)
        {
            case GameMode.Quick:
                LoadNewScene("QuickPlay");
                break;
            default:
                LoadNewScene("BaseLevel");
                break;
        }
    }
}
