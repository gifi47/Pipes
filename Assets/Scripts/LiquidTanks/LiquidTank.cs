using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidTank : MonoBehaviour
{
    public LiquidType liquidType = 0;

    public Queue<string> paintModes = new Queue<string>();

    [SerializeField]
    protected SpriteRenderer spriteRenderer;

    private void Start()
    {
        if (this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.spriteRenderer == null) this.spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();
    }

    public virtual void Paint(Color color, float time)
    {
        this.spriteRenderer.color = color;
        StartCoroutine(ClearPaint(time));
    }

    public virtual void Paint(Color color)
    {
        this.spriteRenderer.color = color;
    }

    protected virtual IEnumerator ClearPaint(float time)
    {
        yield return new WaitForSeconds(time);
        Paint(Color.white);
    }
}

public enum LiquidType
{
    None = 0,
    Water = 1,
    Oil = 2,
    WaterOil = 3,
    Gas = 4,
    WaterGas = 5,
    OilGas = 6,
    WaterOilGas = 7
}
