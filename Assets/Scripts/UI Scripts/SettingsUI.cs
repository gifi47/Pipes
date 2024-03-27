using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [SerializeField]
    protected Slider sliderGlobalVolume;

    [SerializeField]
    protected Slider sliderMusicVolume;

    [SerializeField]
    protected Slider sliderSoundVolume;

    [SerializeField]
    protected InputControlsUI inputControls;

    [SerializeField]
    protected Slider sliderMoveSpeed;

    [SerializeField]
    protected Slider sliderZoomSpeed;

    [SerializeField]
    protected PersistentAudioManager _persistentAudioManager;

    private static PersistentAudioManager persistentAudioManager = null;

    private void Awake()
    {
        if (persistentAudioManager == null)
        {
            persistentAudioManager = _persistentAudioManager;
        }
    }

    public void SaveSettings()
    {
        StaticData.settings.inputMode = inputControls.selectedInputMode;

        StaticData.settings.globalVolume = sliderGlobalVolume.value;
        StaticData.settings.musicVolume = sliderMusicVolume.value;
        StaticData.settings.soundVolume = sliderSoundVolume.value;

        StaticData.settings.camMoveSpeed = sliderMoveSpeed.value;
        StaticData.settings.camZoomSpeed = sliderZoomSpeed.value;

        persistentAudioManager.UpdateVolume();

        StaticData.SaveSettings();
    }

    public void LoadSettings() 
    {
        inputControls.SetMode(StaticData.settings.inputMode);

        sliderGlobalVolume.value = StaticData.settings.globalVolume;
        sliderMusicVolume.value = StaticData.settings.musicVolume;
        sliderSoundVolume.value = StaticData.settings.soundVolume;

        sliderMoveSpeed.value = StaticData.settings.camMoveSpeed;
        sliderZoomSpeed.value = StaticData.settings.camZoomSpeed;
    }

    public void SliderVolumeValueChanged()
    {
        if (persistentAudioManager != null)
        {
            persistentAudioManager.UpdateVolume(sliderGlobalVolume.value, sliderMusicVolume.value);
        }
    }
}
