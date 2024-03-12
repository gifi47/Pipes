using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pipe : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.startRotation = this.transform.rotation;
        this.endRotation = this.transform.rotation;

    }

    public static float timeScale = 3.5f;

    private Quaternion startRotation;
    private Quaternion endRotation;
    private float time = 1;

    private bool isRotating = false;

    // Update is called once per frame
    void Update()
    {
        if (isRotating)
        {
            this.transform.rotation = Quaternion.Slerp(this.startRotation, this.endRotation, this.time);
            if (this.time > 1) this.isRotating = false;
            this.time += Pipe.timeScale * Time.deltaTime;
        }
    }

    public void Rotate()
    {
        this.startRotation = this.transform.rotation;
        this.endRotation = this.endRotation * Quaternion.Euler(0, 0, 90);
        this.time = 0;
        this.isRotating = true;
    }
}
