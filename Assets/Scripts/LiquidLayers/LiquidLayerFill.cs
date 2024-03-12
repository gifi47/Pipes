using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayerFill : LiquidLayer
{

    public float fillSpeed = 0.6f;

    private bool firstFill = true;

    public void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override void Configure(LiquidLayer baseLL, string paintMode, Color color, float delay)
    {
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = baseLL.spriteRenderer.sortingOrder + 1;
        // paintMode

        if (firstFill)
        {
            baseLL.animator.SetFloat("animSpeed", fillSpeed);
            baseLL.animator.Play("LLFanim");
            firstFill = false;
        }
        animator.SetFloat("animSpeed", fillSpeed);
        animator.Play("LLFanim");

        // ********************** 

        StartCoroutine(Sink(baseLL, delay));
    }
}
