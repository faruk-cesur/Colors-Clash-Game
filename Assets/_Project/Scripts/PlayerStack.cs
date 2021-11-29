using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        AnimationController.Instance.RunAnimation(_animator);
    }

    public void PlayerStackDeath()
    {
        AnimationController.Instance.DeathAnimation(_animator);
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerController player = GetComponentInParent<PlayerController>();

        if (other.CompareTag("Obstacle"))
        {
            player.stackGameObjectList.Remove(gameObject);
            transform.SetParent(null);
            gameObject.GetComponent<PlayerStack>().PlayerStackDeath();
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
            player.CalculateStackPositions();
        }
    }
}