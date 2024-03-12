using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiquidTankEnd : LiquidTank
{
    public LiquidType RequiredLiquidType = 0;
    
    [SerializeField]
    protected SpriteRenderer requiredLiquidSign;

    private void Start()
    {
        if (this.spriteRenderer == null) this.spriteRenderer = GetComponent<SpriteRenderer>();
        if (this.spriteRenderer == null) this.spriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

        requiredLiquidSign.color = PipeGameManager.GetColor(RequiredLiquidType);
    }
}
