using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputControlsUI : MonoBehaviour
{
    [SerializeField]
    protected Button buttonSingleFinger;

    [SerializeField]
    protected Button buttonTwoFingers;

    [SerializeField]
    protected GameObject statusB1;

    [SerializeField]
    protected GameObject statusB2;

    public SettingsStruct.InputMode selectedInputMode;

    public void SetMode(SettingsStruct.InputMode inputMode)
    {
        bool val = (inputMode == SettingsStruct.InputMode.SingleFinger);

        selectedInputMode = inputMode;

        buttonSingleFinger.interactable = !val;
        buttonTwoFingers.interactable = val;
        statusB1.SetActive(val);
        statusB2.SetActive(!val);
    }

    public void ButtonSingleFingerClick()
    {
        SetMode(SettingsStruct.InputMode.SingleFinger);
    }

    public void ButtonTwoFingersClick() 
    {
        SetMode(SettingsStruct.InputMode.TwoFingers);
    }
}

