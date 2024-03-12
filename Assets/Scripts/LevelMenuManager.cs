using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class LevelMenuManager : MonoBehaviour
{
    [SerializeField]
    private InterfaceUI hud;

    [SerializeField]
    private InterfaceUI pauseMenu;

    [SerializeField]
    private ConfirmationUI confirmationMenu;

    [SerializeField]
    private ResultMenuUI resultMenu;

    public void Start()
    {
        hud.SetLevelName($"Уровень {LevelManager.selectedLevel + 1}");
        pauseMenu.SetLevelName($"Уровень {LevelManager.selectedLevel + 1}");
    }

    public void ButtonHomeClick(bool needConfirmation)
    {
        if (needConfirmation)
        {
            confirmationMenu.Confirm("Выйти в главное меню?", Home);
        }
        else
        {
            Home();
        }
    }

    public void ButtonPauseClick()
    {
        Time.timeScale = 0.0f;
        pauseMenu.gameObject.SetActive(true);
        hud.gameObject.SetActive(false);
    }

    public void ButtonResumeClick()
    {
        Time.timeScale = 1.0f;
        pauseMenu.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);
    }

    public void ButtonReloadClick(bool needConfirmation)
    {
        if (needConfirmation)
        {
            confirmationMenu.Confirm("Перезапустить уровень?", ReloadLevel);
        }
        else
        {
            ReloadLevel();
        }
    }

    public void Home()
    {
        Time.timeScale = 1.0f;
        SceneLoader.LoadScene("MainMenu");
    }

    public void ReloadLevel()
    {
        Time.timeScale = 1.0f;
        SceneLoader.LoadScene("Level");
    }

    public void ButtonNextClick()
    {
        LevelManager.selectedLevel += 1;
        SceneLoader.LoadScene("Level");
    }

    public void LevelCompleted(int score)
    {
        hud.gameObject.SetActive(false);
        resultMenu.ShowStars(score);
        resultMenu.ShowParticles(score);
        resultMenu.SetMessage(score == 0 ? "Попробуйте ещё раз" : "Уровень пройден!");
        resultMenu.gameObject.SetActive(true);
    }
}
