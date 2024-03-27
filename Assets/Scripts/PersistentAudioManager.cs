using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentAudioManager : MonoBehaviour
{
    [SerializeField]
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip clipMusic;

    static bool isNotLoaded = true; 

    private void Start()
    {
        if (isNotLoaded)
        {
            DontDestroyOnLoad(gameObject); // Keeps AudioManager alive between scene changes
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
            audioSource.volume = StaticData.settings.globalVolume * StaticData.settings.musicVolume;
            isNotLoaded = false;
        } else
        {
            Destroy(this.gameObject);
        }
        //SceneManager.sceneLoaded += OnSceneLoaded; // Subscribe to scene loaded event
    }

    public void UpdateVolume()
    {
        audioSource.volume = StaticData.settings.globalVolume * StaticData.settings.musicVolume;
    }

    public void UpdateVolume(float globalVolume, float musicVolume)
    {
        audioSource.volume = globalVolume * musicVolume;
    }
}

