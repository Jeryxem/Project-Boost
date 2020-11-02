using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyParticleEffect : MonoBehaviour
{
    [Header("Unity Setup")]
    public float durationOfEffect;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, durationOfEffect);
    }
}
