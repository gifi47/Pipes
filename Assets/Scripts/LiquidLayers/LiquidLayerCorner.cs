using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayerCorner : LiquidLayer
{
    void Start()
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

        if (paintMode == "Collider1")
        {
            spriteRenderer.flipY = false;
            transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        }
        else
        {
            spriteRenderer.flipY = true;
            transform.localRotation = Quaternion.Euler(0f, 0f, 90f);
        }
        animator.Play("LLCanim");

        // ********************** 

        StartCoroutine(Sink(baseLL, delay));
    }
}
