using System;
using UnityEngine;

public class PlayerStack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    
    private void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.PrepareGame:
                AnimationController.Instance.IdleAnimation(_animator);
                break;
            case GameState.MainGame:
                AnimationController.Instance.RunAnimation(_animator);
                break;
            case GameState.LoseGame:
                AnimationController.Instance.DeathAnimation(_animator);
                break;
            case GameState.WinGame:
                AnimationController.Instance.WinAnimation(_animator);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}