using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceUI : MonoBehaviour
{
    [SerializeField]
    private TMPro.TMP_Text textLvlName;

    public void SetLevelName(string lvlNname) 
    {
        textLvlName.text = lvlNname;
    }
}