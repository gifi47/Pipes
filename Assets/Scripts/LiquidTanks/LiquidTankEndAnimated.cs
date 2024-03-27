using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidLayerEndAnimated : LiquidTankEnd
{
    [SerializeField]
    protected LiquidLayer liquidLayer;

    public void Start()
    {
        if (liquidLayer == null)
            liquidLayer = GetComponentInChildren<LiquidLayer>();

        liquidLayer.Paint(PipeGameManager.GetColor(liquidType));

        requiredLiquidSign.color = PipeGameManager.GetColor(RequiredLiquidType);
    }

    public override void Paint(Color color, float time)
    {
        this.liquidLayer.StartPaint(paintModes.Dequeue(), color, 0.5f);
        StartCoroutine(ClearPaint(time));
    }

    public override void Paint(Color color)
    {
        this.liquidLayer.StartPaint(paintModes.Dequeue(), color, 0.5f);
    }

    protected override IEnumerator ClearPaint(float time)
    {
        yield return new WaitForSeconds(time);
        this.liquidLayer.Paint(new Color(0, 0, 0, 0));
    }
}

