using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SkyCube : MonoBehaviour
{
    private void Update()
    {
        gameObject.transform.Rotate(Vector3.right * Time.deltaTime * 10f);
    }
}