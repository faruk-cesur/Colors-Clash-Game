using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private Animator _animator;

   

    public bool aaa = false;

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
        Trap trap = other.GetComponentInParent<Trap>();
        PlayerController player = GetComponentInParent<PlayerController>();

        if (trap)
        {
            aaa = true;
            player.stackGameObjectList.Remove(gameObject);
            transform.SetParent(null);
            player.olen.Add(gameObject);
            
            gameObject.GetComponent<PlayerStack>().PlayerStackDeath();
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
            player.CalculateStackPositions();
        }
    }
}