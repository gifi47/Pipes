using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Valve : MonoBehaviour, IInteractable
{
    bool isRotating = false;

    private Action interact;
    public Action OnInteract { get => interact; set => interact = value; }

    [SerializeField]
    private Gear[] gears;

    public bool Interact()
    {
        OnInteract?.Invoke();
        return Rotate();
    }

    public bool Rotate()
    {
        if (!isRotating)
        {
            foreach (Gear gear in gears) { gear.Rotate(); }
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
