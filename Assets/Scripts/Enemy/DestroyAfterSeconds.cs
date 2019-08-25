using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterSeconds : MonoBehaviour
{
    public float DestroyTime;

    void Update()
    {
        Destroy(gameObject, DestroyTime);
    }
}
