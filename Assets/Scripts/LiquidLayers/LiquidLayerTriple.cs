using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayerTriple : LiquidLayer
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

        if (paintMode == "Collider1")
        {
            spriteRenderer.flipY = true;
            animator.Play("LLTanim1");
        }
        else if (paintMode == "Collider2")
        {
            spriteRenderer.flipY = false;
            animator.Play("LLTanim1");
        }
        else
        {
            spriteRenderer.flipY = false;
            animator.Play("LLTanim2");
        }

        // ********************** 

        StartCoroutine(Sink(baseLL, delay));
    }
}

