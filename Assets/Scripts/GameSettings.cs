using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    [SerializeField] private Slider musicSlider, sfxSlider;
    [SerializeField] private Toggle oskSetting;
    [SerializeField] private TMP_Dropdown batteryDropdown;

    private void Start()
    {
        if (ES3.KeyExists("musicVolume"))
        {
            musicSlider.value = (float)ES3.Load("musicVolume");
        
        }

        if (ES3.KeyExists("sfxSlider"))
        {
            sfxSlider.value = (float)ES3.Load("sfxSlider");
    
        }

        if (ES3.KeyExists("osk") && oskSetting != null)
        {
            oskSetting.isOn = (bool)ES3.Load("osk");
        }

        SetFramerate();
    }

    private void SetFramerate()
    {
        int frameRate;

        if (ES3.KeyExists("batteryDropdown"))
        {
            frameRate = (int)ES3.Load<BatteryMode>("batteryDropdown");

            if (batteryDropdown != null)
            {
                switch (frameRate)
                {
                    case 15:
                        batteryDropdown.value = 0;
                        break;
                    case 30:
                        batteryDropdown.value = 1;
                        break;
                    case 60:
                        batteryDropdown.value = 2;
                        break;
                    case -1:
                        batteryDropdown.value = 3;
                        break;
                }
            }
        }
        else
        {
            frameRate = 30;
        }

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }

    public void AdjustSound(string sound)
    {
        if (sound == "music")
        {
            ES3.Save("musicVolume", musicSlider.value);
        }
        else
        {
            ES3.Save("sfxSlider", sfxSlider.value);
        }
    }

    public void SetBatteryMode()
    {
        int frameRate;

        switch(batteryDropdown.value)
        {
            case 0:
                frameRate = 15;
                break;
            case 1:
                frameRate = 30;
                break;
            case 2:
                frameRate = 60;
                break;
            default:
                frameRate = -1;
                break;
        }
        
        Application.targetFrameRate = frameRate;
        ES3.Save<BatteryMode>("batteryDropdown", (BatteryMode)frameRate);
    }

    public void SetKeyboardSetting()
    {
        ES3.Save("osk", oskSetting.isOn);
    }
}
