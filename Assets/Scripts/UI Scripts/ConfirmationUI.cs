using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfirmationUI : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text messageText;

    private Action onConfirm;

    public void Confirm(string message, Action action)
    {
        gameObject.SetActive(true);
        onConfirm = null;
        onConfirm += action;
        SetMessage(message);
    }

    public void ButtonConfirmClick()
    {
        onConfirm?.Invoke();
        gameObject.SetActive(false);
    }

    public void ButtonCancelClick()
    {
        gameObject.SetActive(false);
    }

    public void SetMessage(string message)
    {
        messageText.text = message;
    }
}
