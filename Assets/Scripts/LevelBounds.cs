using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelBounds : MonoBehaviour
{
    public Vector2 topLeft;
    public Vector2 bottomRight;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(topLeft, topLeft + new Vector2(bottomRight.x - topLeft.x, 0));
        Gizmos.DrawLine(topLeft + new Vector2(bottomRight.x - topLeft.x, 0), bottomRight);
        Gizmos.DrawLine(bottomRight, topLeft - new Vector2(0, topLeft.y - bottomRight.y));
        Gizmos.DrawLine(topLeft - new Vector2(0, topLeft.y - bottomRight.y), topLeft);
    }
#endif
}

