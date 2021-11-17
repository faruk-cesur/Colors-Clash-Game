using System;
using UnityEngine;

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
}