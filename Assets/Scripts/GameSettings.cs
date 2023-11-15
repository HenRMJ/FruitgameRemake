using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider;
    [SerializeField] private Toggle oskSetting;

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

        if (ES3.KeyExists("osk") && oskSetting != null)
        {
            oskSetting.isOn = (bool)ES3.Load("osk");
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

    public void SetKeyboardSetting()
    {
        ES3.Save("osk", oskSetting.isOn);
    }
}
