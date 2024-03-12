#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

public class EditorReplace : EditorWindow
{
    GameObject prefabQ1;
    GameObject prefabQ2;

    [MenuItem("Tools/Replace Prefabs")]
    public static void ShowWindow()
    {
        GetWindow<EditorReplace>("Replace Prefabs");
    }

    void OnGUI()
    {
        prefabQ1 = EditorGUILayout.ObjectField("Prefab Q1", prefabQ1, typeof(GameObject), false) as GameObject;
        prefabQ2 = EditorGUILayout.ObjectField("Prefab Q2", prefabQ2, typeof(GameObject), false) as GameObject;

        if (GUILayout.Button("Replace Selected"))
        {
            ReplaceSelectedObjects();
        }
    }

    void ReplaceSelectedObjects()
    {
        if (prefabQ1 == null || prefabQ2 == null)
        {
            Debug.LogError("Please assign both prefabs.");
            return;
        }

        Transform[] selectedTransforms = Selection.transforms;

        foreach (Transform selectedTransform in selectedTransforms)
        {
            GameObject selectedGameObject = selectedTransform.gameObject;
            PrefabType prefabType = PrefabUtility.GetPrefabType(selectedGameObject);

            if (prefabType == PrefabType.PrefabInstance && PrefabUtility.GetCorrespondingObjectFromSource(selectedGameObject) == prefabQ1)
            {
                Vector3 position = selectedTransform.position;
                Quaternion rotation = selectedTransform.rotation;
                Transform parent = selectedTransform.parent;

                GameObject newObject = PrefabUtility.InstantiatePrefab(prefabQ2) as GameObject;
                Undo.RegisterCreatedObjectUndo(newObject, "Replace Prefabs");
                newObject.transform.position = position;
                newObject.transform.rotation = rotation;

                if (parent != null)
                {
                    newObject.transform.parent = parent;
                }

                Undo.DestroyObjectImmediate(selectedGameObject);
            }
        }
    }
}
#endif