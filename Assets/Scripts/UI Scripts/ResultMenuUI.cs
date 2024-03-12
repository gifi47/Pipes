using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResultMenuUI : StarsUI
{
    [SerializeField]
    protected TMPro.TMP_Text messageText;

    public void SetMessage(string text)
    {
        messageText.text = text;
    }

    [SerializeField]
    protected ParticleSystem particle;

    public void ShowParticles(int score)
    {
        particle.Play();
        var emission = particle.emission;
        emission.rateOverTime = new ParticleSystem.MinMaxCurve(5f * score);
    }

}
