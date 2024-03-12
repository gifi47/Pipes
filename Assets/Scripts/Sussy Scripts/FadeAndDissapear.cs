using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeAndDissapear : MonoBehaviour
{
    public float delayBeforeFading = 1f;

    public float fadingTime = 2f;

    private bool fading = false;

    private float time = 0;

    [SerializeField]
    private Image[] images;

    // Start is called before the first frame update
    void Start()
    {
        if (images == null || images.Length == 0)
        {
            images = GetComponentsInChildren<Image>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        if (!fading)
        {
            if (time > delayBeforeFading)
            {
                time -= delayBeforeFading;
                fading = true;
            }
        } else
        {
            if (time > fadingTime)
            {
                time = 1;
                fading = false;
                this.gameObject.SetActive(false);
            }
            for (int i = 0; i < images.Length; i++) 
            {
                Color color = images[i].color;
                color.a = (1 - time / fadingTime);
                images[i].color = color;
            }
        }
    }
}
