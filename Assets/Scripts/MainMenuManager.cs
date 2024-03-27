using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField]
    private GameObject titleScreen;

    [SerializeField]
    private GameObject levelSelectionMenu;

    [SerializeField]
    private SettingsUI settingsMenu;

    [SerializeField]
    private ConfirmationUI confirmationUI;

    public void Start()
    {
        titleScreen.SetActive(true);
        levelSelectionMenu.SetActive(false);
        settingsMenu.gameObject.SetActive(false);
    }

    // SELECTION 

    public void ButtonSelectLevelClick(int lvl)
    {
        LevelManager.selectedLevel = lvl - 1;
        SceneLoader.LoadScene("Level");
    }

    public void ButtonReturnToMenu()
    {
        titleScreen.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
        levelSelectionMenu.SetActive(false);
    }

    // MAIN

    public void ButtonStartClick()
    {
        titleScreen.SetActive(false);
        levelSelectionMenu.SetActive(true);
    }

    public void ButtonExitClick()
    {
        Application.Quit();
    }

    // SETTINGS MENU

    public void ButtonSettingsClick()
    {
        titleScreen.SetActive(false);
        settingsMenu.LoadSettings();
        settingsMenu.gameObject.SetActive(true);
    }

    public void ButtonConfirmNewSettings()
    {
        settingsMenu.SaveSettings();
        titleScreen.SetActive(true);
        settingsMenu.gameObject.SetActive(false);
    }

    public void ButtonDeleteProgress()
    {
        confirmationUI.Confirm("Удалить прогресс?", DeleteProgress);
    }

    private void DeleteProgress()
    {
        StaticData.DeleteSaveFile();
        StaticData.LoadData();
        SceneLoader.LoadScene("MainMenu");
    }
}

