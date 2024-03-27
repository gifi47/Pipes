using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarsUI : MonoBehaviour
{
    [SerializeField]
    protected Sprite fadedStar;

    [SerializeField]
    protected Sprite enlightedStar;

    [SerializeField]
    protected Image[] stars;

    public void ShowStars(int score)
    {

        for (int i = 0; i < Mathf.Min(score, stars.Length); i++) {
            stars[i].sprite = enlightedStar;
        }

        for (int i = score; i < stars.Length; i++)
        {
            stars[i].sprite = fadedStar;
        }
    }
}

