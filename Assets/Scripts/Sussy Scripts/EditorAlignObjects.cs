#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnPrefabEditor
{
    [MenuItem("Align/Align_Selected/Align %&n")]
    static void AlignSelectedObjects()
    {
        foreach (GameObject obj in Selection.gameObjects)
        {
            obj.transform.localPosition = new Vector3(
                Mathf.Round(obj.transform.localPosition.x), 
                Mathf.Round(obj.transform.localPosition.y), 
                Mathf.Round(obj.transform.localPosition.z)
            );
        }
    }

    [MenuItem("Align/Align_Selected/Align2 %&n")]
    static void Align2SelectedObjects()
    {
        foreach (Transform trf in Selection.transforms)
        {
            trf.localPosition = new Vector3(
                Mathf.Round(trf.localPosition.x),
                Mathf.Round(trf.localPosition.y),
                Mathf.Round(trf.localPosition.z)
            );
        }
    }
}
#endif
