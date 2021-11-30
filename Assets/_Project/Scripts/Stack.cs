using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Stack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

    private void Start()
    {
        AnimationManager.Instance.RunAnimation(_animator);
    }

    public void StackDeath()
    {
        AnimationManager.Instance.DeathAnimation(_animator);
    }

    public void OnTriggerEnter(Collider other)
    {
        PlayerController player = GetComponentInParent<PlayerController>();

        if (other.CompareTag("Obstacle"))
        {
            player.stackGameObjectList.Remove(gameObject);
            transform.SetParent(null);
            gameObject.GetComponent<Stack>().StackDeath();
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
            player.CalculateStackPositions();
        }
    }
}