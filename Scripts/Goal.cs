using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    int placement;

    void Awake()
    {
        placement = 0;
    }

    void Update()
    {
        transform.Rotate(Time.deltaTime * 0, 0.1f, 0);
    }

    public int GetPlacement
    {
        get { return placement; }
        set { placement = value; }
    }
}
