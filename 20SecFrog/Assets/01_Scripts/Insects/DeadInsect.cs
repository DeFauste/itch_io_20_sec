using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class DeadInsect : MonoBehaviour
{
    private void OnEnable()
    {
        int rotation = Random.Range(0, 18) * 20;
        transform.rotation = Quaternion.Euler(0f, 0f, rotation);
    }
}
