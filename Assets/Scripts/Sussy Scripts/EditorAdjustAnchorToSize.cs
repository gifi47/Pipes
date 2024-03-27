#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

public class EditorAdjustAnchorToSize
{
    [MenuItem("Tools/Anchor Around Object")]
    static void uGUIAnchorAroundObject()
    {
        foreach (var obj in Selection.gameObjects)
        {
            RectTransform rectTransform;
            if (obj.TryGetComponent(out rectTransform)) 
            { 
                AdjustAnchors(rectTransform); 
            }
        }
    }

    public static void AdjustAnchors(RectTransform rectTransform)
    {
        var p = rectTransform.transform.parent.GetComponent<RectTransform>();
        if (p == null) return;
        Undo.RecordObject(rectTransform, "Adjust Anchor");

        var offsetMin = rectTransform.offsetMin;
        var offsetMax = rectTransform.offsetMax;
        var _anchorMin = rectTransform.anchorMin;
        var _anchorMax = rectTransform.anchorMax;

        var parent_width = p.rect.width;
        var parent_height = p.rect.height;

        var anchorMin = new Vector2(_anchorMin.x + (offsetMin.x / parent_width),
                                    _anchorMin.y + (offsetMin.y / parent_height));
        var anchorMax = new Vector2(_anchorMax.x + (offsetMax.x / parent_width),
                                    _anchorMax.y + (offsetMax.y / parent_height));

        rectTransform.anchorMin = anchorMin;
        rectTransform.anchorMax = anchorMax;

        rectTransform.offsetMin = new Vector2(0, 0);
        rectTransform.offsetMax = new Vector2(0, 0);
        rectTransform.pivot = new Vector2(0.5f, 0.5f);
    }
}
#endif