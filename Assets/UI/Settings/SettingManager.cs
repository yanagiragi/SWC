using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SettingManager : MonoBehaviour
{
    public Toggle fullscreenToggle;
    public Dropdown resolutionDropdown;
    public Dropdown texturequalityDropdown;
    public Dropdown antialiasingDropdown;
    public Dropdown vSyncDropdown;
    public Slider musicvolumeSlider;
    public Button applybutton;
    public GameObject menu;

    public AudioSource musicSource;
    public Resolution[] resolutions;
    public GameSettings gamesettings;


    void OnEnable()
    {

        gamesettings = new GameSettings();
        //gamesettings.Antialiasing = 2;
        fullscreenToggle.onValueChanged.AddListener(delegate { OnfullscreenToggle(); });
        resolutionDropdown.onValueChanged.AddListener(delegate { OnResolutionChange(); });
        texturequalityDropdown.onValueChanged.AddListener(delegate { OnTextureQualityChange(); });
        antialiasingDropdown.onValueChanged.AddListener(delegate { OnAntialiasingChange(); });
        vSyncDropdown.onValueChanged.AddListener(delegate { OnVsyncChange(); });
        musicvolumeSlider.onValueChanged.AddListener(delegate { OnMusicVolumeChange(); });
        applybutton.onClick.AddListener(delegate { OnApplyButtonClick(); });

        resolutions = Screen.resolutions;
        foreach (Resolution resolution in resolutions)
        {
            resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString()));
        }

        LoadSettings();
    }
    public void OnfullscreenToggle()
    {
        gamesettings.fullscreen = Screen.fullScreen = fullscreenToggle.isOn;
    }
    public void OnResolutionChange()
    {
        Screen.SetResolution(resolutions[resolutionDropdown.value].width, resolutions[resolutionDropdown.value].height, Screen.fullScreen);
        gamesettings.Resolutionindex = resolutionDropdown.value;
    }
    public void OnTextureQualityChange()
    {
        QualitySettings.masterTextureLimit = gamesettings.Texturequlity = texturequalityDropdown.value;

    }
    public void OnAntialiasingChange()
    {
        QualitySettings.antiAliasing = gamesettings.Antialiasing = (int)Mathf.Pow(2, antialiasingDropdown.value);
    }
    public void OnVsyncChange()
    {
        QualitySettings.vSyncCount = gamesettings.Vsync = vSyncDropdown.value;
    }
    public void OnMusicVolumeChange()
    {
        musicSource.volume = gamesettings.Musicvolume = musicvolumeSlider.value;
    }
    public void OnApplyButtonClick()
    {
        SaveSettings();
        menu.SetActive(false);
    }


    public void SaveSettings()
    {
        string jsondata = JsonUtility.ToJson(gamesettings, true);
        File.WriteAllText(Application.persistentDataPath + "/gamesettings.json", jsondata);
    }
    public void LoadSettings()
    {
        gamesettings = JsonUtility.FromJson<GameSettings>(File.ReadAllText(Application.persistentDataPath + "/gamesettings.json"));
        musicvolumeSlider.value = gamesettings.Musicvolume;
        antialiasingDropdown.value = gamesettings.Antialiasing;
        vSyncDropdown.value = gamesettings.Vsync;
        texturequalityDropdown.value = gamesettings.Texturequlity;
        resolutionDropdown.value = gamesettings.Resolutionindex;
        fullscreenToggle.isOn = gamesettings.fullscreen;
        Screen.fullScreen = gamesettings.fullscreen;



        resolutionDropdown.RefreshShownValue();
    }

}