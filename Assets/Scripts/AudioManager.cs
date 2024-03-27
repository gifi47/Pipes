using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource pipeClickSoundSource;

    [SerializeField]
    private AudioSource valveSoundSource;

    [SerializeField]
    private AudioSource musicSoundSource;

    [SerializeField]
    private AudioSource ambientSoundSource;

    [SerializeField]
    private AudioSource levelEndSoundSource;

    [SerializeField]
    private AudioClip clipClick;

    [SerializeField]
    private AudioClip clipValve;

    [SerializeField]
    private AudioClip clipMusic;

    [SerializeField]
    private AudioClip clipWaterFlow;

    [SerializeField]
    private AudioClip clipWin;

    [SerializeField]
    private AudioClip clipDefeat;

    // Start is called before the first frame update
    void Start()
    {
        pipeClickSoundSource.volume = StaticData.settings.globalVolume * StaticData.settings.soundVolume;
        valveSoundSource.volume = StaticData.settings.globalVolume * StaticData.settings.soundVolume;
        musicSoundSource.volume = StaticData.settings.globalVolume * StaticData.settings.musicVolume;
        ambientSoundSource.volume = StaticData.settings.globalVolume * StaticData.settings.musicVolume;
        levelEndSoundSource.volume = StaticData.settings.globalVolume * StaticData.settings.soundVolume;
    }

    public void PlayClickSound()
    {
        pipeClickSoundSource.clip = clipClick;
        pipeClickSoundSource.Play();
    }

    public void PlayValveSound()
    {
        valveSoundSource.clip = clipValve;
        valveSoundSource.Play();
        StartCoroutine(StopPlaying(valveSoundSource, 1.5f));
    }

    IEnumerator StopPlaying(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        source.Stop();
    }

    public void PlayWaterFlow()
    {
        ambientSoundSource.clip = clipWaterFlow;
        ambientSoundSource.Play();
        ambientSoundSource.loop = true;
    }

    public void StopWaterFlow()
    {
        ambientSoundSource.Stop();
    }

    public void PlayWinSound()
    {
        levelEndSoundSource.clip = clipWin;
        levelEndSoundSource.Play();
    }

    public void PlayLoseSound()
    {
        levelEndSoundSource.clip = clipDefeat;
        levelEndSoundSource.Play();
    }
}

