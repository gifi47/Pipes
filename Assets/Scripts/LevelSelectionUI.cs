﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectionUI : MonoBehaviour
{
    [SerializeField]
    private GameObject prefabButtonSelectLevel;

    private void Start()
    {
        if (StaticData.data.levelsScore == null || StaticData.data.levelsScore.Length == 0)
        {
            StaticData.data.levelsScore = new int[] { 0, 0, 0, 0, 0, 0 };
        }
        for (int i = 0; i < StaticData.data.levelsScore.Length; i++)
        {
            GameObject button = Instantiate(prefabButtonSelectLevel);
            RectTransform rectTransform = button.GetComponent<RectTransform>();
            rectTransform.SetParent(this.transform);
            //                                                                 500f
            rectTransform.anchoredPosition = new Vector3(-300 + (i % 3) * 300, 400f - (i / 3) * 300, 0f);

            var lvl = i;

            button.GetComponentInChildren<TMPro.TMP_Text>().text = $"{lvl + 1}";

            button.GetComponent<Button>().onClick.AddListener(() => {
                LevelManager.selectedLevel = lvl;
                SceneLoader.LoadScene("Level");
            });

            button.GetComponent<StarsUI>().ShowStars(StaticData.data.levelsScore[i]);
        }
    }
}

