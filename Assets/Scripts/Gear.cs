using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    bool isRotating = false;

    public bool Rotate()
    {
        if (!isRotating)
        {
            StartCoroutine(RotateCoroutine());
            return true;
        }
        return false;
    }

    IEnumerator RotateCoroutine()
    {
        isRotating = true;
        float time = 0;
        Quaternion rotation = this.transform.rotation;
        Quaternion endRotation = rotation * Quaternion.Euler(0, 0, 180);
        while (this.isRotating)
        {
            this.transform.rotation = Quaternion.Slerp(rotation, endRotation, time);
            if (time > 1) this.isRotating = false;
            time += Time.deltaTime;
            yield return null;
        }
    }
}
