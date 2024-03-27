using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayerStraightCone : LiquidLayer
{
    void Start()
    {
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
        if (animator == null)
            animator = GetComponent<Animator>();
    }

    public override void Configure(LiquidLayer baseLL, string paintMode, Color color, float delay)
    {
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = baseLL.spriteRenderer.sortingOrder + 1;
        // paintMode

        if (paintMode == "Collider2")
        {
            spriteRenderer.flipY = true;
        }
        else
        {
            spriteRenderer.flipY = false;
        }
        animator.Play("LLConeSanim");

        // ********************** 

        StartCoroutine(Sink(baseLL, delay));
    }
}

