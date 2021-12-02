using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [SerializeField] private Transform _player;

    private void Update()
    {
        if (_player.position.z > transform.position.z + 15)
        {
            gameObject.SetActive(false);
        }
    }
}
