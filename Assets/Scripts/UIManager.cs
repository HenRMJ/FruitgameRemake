using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider;
    [SerializeField] private GameObject detailsPanel, combinationUI, nextFruitUI;

    private Animator animator;

    private bool settingsOpen;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (ES3.KeyExists("musicVolume"))
        {
            musicSlider.value = (float)ES3.Load("musicVolume");
            SoundManager.Instance.GetAudioSource().volume = (float)ES3.Load("musicVolume");
        }

        if (ES3.KeyExists("sfxSlider"))
        {
            sfxSlider.value = (float)ES3.Load("sfxSlider");
            SoundManager.Instance.SetSoundEffectVolume((float)ES3.Load("sfxSlider"));
        }

        ScoreManager.Instance.OnLostGame += ScoreManager_OnLostGame;
    }

    private void OnDestroy()
    {
        ScoreManager.Instance.OnLostGame -= ScoreManager_OnLostGame;
    }

    private void ScoreManager_OnLostGame(object sender, EventArgs e)
    {
        detailsPanel.SetActive(true);
        combinationUI.SetActive(false);
        nextFruitUI.SetActive(false);
    }

    public void ToggleSettings()
    {
        settingsOpen = !settingsOpen;

        animator.SetBool("settingsOpen", settingsOpen);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}
