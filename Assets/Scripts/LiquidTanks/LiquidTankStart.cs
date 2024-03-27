using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidTankStart : LiquidTank
{
    public LiquidType StartLiquidType = 0;

    void Start()
    {
        if (this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.spriteRenderer == null) this.spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        liquidType = StartLiquidType;
        Paint(PipeGameManager.GetColor(liquidType));
    }
}

