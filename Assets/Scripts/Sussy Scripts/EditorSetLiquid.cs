#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class EditorSeLiquid : EditorWindow
{

    LiquidType liquidType;

    [MenuItem("Tools/Set Liquid")]
    public static void ShowWindow()
    {
        GetWindow<EditorSeLiquid>("Set Liquid");
    }

    void OnGUI()
    {
        liquidType = (LiquidType)EditorGUILayout.EnumPopup("Liquid Type", liquidType);

        if (GUILayout.Button("Set Liquid"))
        {
            SetLiquid();
        }
    }

    void SetLiquid()
    {
        Transform[] selectedTransforms = Selection.transforms;

        foreach (Transform selectedTransform in selectedTransforms)
        {
            LiquidTank lt = selectedTransform.gameObject.GetComponent<LiquidTank>();
            if (lt != null)
            {
                if (lt is LiquidTankStart || lt is LiquidTankStartAnimated) { (lt as LiquidTankStart).StartLiquidType = liquidType; }
                lt.liquidType = liquidType;
            }
        }
    }
}
#endif

