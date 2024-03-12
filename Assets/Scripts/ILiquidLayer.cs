using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ILiquidLayer
{
    void Hide();
    void Paint();
    void Paint(string paintMode);
    void SetPaintMode(string paintMode);

    GameObject GetGameObject();
}
