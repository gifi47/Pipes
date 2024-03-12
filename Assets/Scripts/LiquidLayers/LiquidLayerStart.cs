using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayerStart : LiquidLayer
{
    [SerializeField]
    protected Animator animatorGate;

    public bool isClosed = true;

    // Start is called before the first frame update
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

        if (paintMode == "Start")
        {
            baseLL.animator.Play("LLStartanim2half");
            animator.Play("LLStartanim2half");
            animatorGate.Play("Separatoranim");
            (baseLL as LiquidLayerStart).isClosed = false;
        }
        else if (paintMode == "Collider1")
        {
            spriteRenderer.flipY = false;
            animator.Play("LLStartanim");
        } else
        {
            spriteRenderer.flipY = true;
            animator.Play("LLStartanim1half");
            if (!(baseLL as LiquidLayerStart).isClosed) {
                (baseLL as LiquidLayerStart).isClosed = true;
                (baseLL as LiquidLayerStart).animatorGate.Play("Separatoranim2"); 
            };
            return;
        }

        // ********************** 

        StartCoroutine(Sink(baseLL, delay));
    }
}
