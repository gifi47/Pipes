using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static int selectedLevel = 0;

    public Level[] Levels;

    public Level LoadedLevel { get => Levels[selectedLevel]; }

    void Awake()
    {
#if DEBUG
        if (StaticData.data.levelsScore == null || StaticData.data.levelsScore.Length == 0)
        {
            StaticData.LoadData();
            StaticData.LoadSettings();
        }
#endif
        if (selectedLevel < 0 || selectedLevel >= Levels.Length)
        {
            SceneLoader.LoadScene("MainMenu");
        }
        for (int i = 0; i < Levels.Length; i++)
        {
            Levels[i].gameObject.SetActive(i == selectedLevel);
        }
    }
}

