using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider;

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
    }

    public void AdjustSound(string sound)
    {
        if (sound == "music")
        {
            ES3.Save("musicVolume", musicSlider.value);
            SoundManager.Instance.GetAudioSource().volume = (float)ES3.Load("musicVolume");
        }
        else
        {
            ES3.Save("sfxSlider", sfxSlider.value);
            SoundManager.Instance.SetSoundEffectVolume((float)ES3.Load("sfxSlider"));
        }
    }

    public void ToggleSettings()
    {
        settingsOpen = !settingsOpen;

        animator.SetBool("settingsOpen", settingsOpen);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }
}
