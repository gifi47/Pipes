using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayer : MonoBehaviour
{
    [SerializeField]
    public SpriteRenderer spriteRenderer;

    [SerializeField]
    public Animator animator;

    private void Start()
    {
        if (animator == null)
            animator = GetComponent<Animator>();
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Paint(Color color)
    {
        spriteRenderer.color = color;
    }

    public void StartPaint(string paintMode, Color color, float delay)
    {
        GameObject tempLL = Instantiate(this.gameObject, this.transform.parent);
        tempLL.GetComponent<LiquidLayer>().Configure(this, paintMode, color, delay);
    }

    public virtual void Configure(LiquidLayer baseLL, string paintMode, Color color, float delay)
    {
        spriteRenderer.color = color;
        spriteRenderer.sortingOrder = baseLL.spriteRenderer.sortingOrder + 1;
// paintMode

        animator.Play("");

// ********************** 

        StartCoroutine(Sink(baseLL, delay));
    }

    protected IEnumerator Sink(LiquidLayer baseLL, float delay)
    {
        yield return new WaitForSeconds(delay);
        baseLL.Paint(this.spriteRenderer.color);
        Destroy(this.gameObject);
    }
}

