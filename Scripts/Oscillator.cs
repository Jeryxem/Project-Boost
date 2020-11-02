using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oscillator : MonoBehaviour
{
    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;
    [SerializeField] bool canRotate = false;
    [SerializeField] float rotateSpeedX, rotateSpeedY, rotateSpeedZ;
    
    float movementFactor; // 0 for not moved, 1 for fully moved
    Vector3 startingPos;

    void Start()
    {
        startingPos = transform.position;
    }

    void Update()
    {
        float cycles = Time.time / period;

        const float tau = Mathf.PI * 2f;  // about 6.28
        float rawSinWave = Mathf.Sin(cycles * tau); // goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f; // turns (-1 to +1) to (0 to +1)
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;

        if(canRotate)
            transform.Rotate(Time.deltaTime * rotateSpeedX, rotateSpeedY, rotateSpeedZ);
    }
}
