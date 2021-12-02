using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Stack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _onTarget;
    private bool _isGameLose;

    private void Start()
    {
        AnimationManager.Instance.RunAnimation(_animator);
    }

    private void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.PrepareGame:
                break;
            case GameState.MainGame:
                break;
            case GameState.LoseGame:
                if (!_isGameLose)
                {
                    _isGameLose = true;
                    AnimationManager.Instance.DeathAnimation(_animator);
                }

                break;
            case GameState.WinGame:
                AnimationManager.Instance.WinAnimation(_animator);
                gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
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
            Destroy(gameObject, 2f);
            player.stackGameObjectList.Remove(gameObject);
            transform.SetParent(null);
            gameObject.GetComponent<Stack>().StackDeath();
            gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
            player.CalculateStackPositions();
        }


        if (other.CompareTag("CubeRed") && gameObject.CompareTag("StackRed") && !_onTarget)
        {
            _onTarget = true;
            other.gameObject.tag = "Untagged";
            gameObject.transform.SetParent(null);
            gameObject.transform.DOMove(other.gameObject.transform.position + Vector3.down, 1.5f);
            StackDestroyCube(player, other);
            gameObject.GetComponent<Animator>().applyRootMotion = true;
            AnimationManager.Instance.KickAnimation(_animator);
        }

        if (other.CompareTag("CubeGreen") && gameObject.CompareTag("StackGreen") && !_onTarget)
        {
            _onTarget = true;
            other.gameObject.tag = "Untagged";
            gameObject.transform.SetParent(null);
            gameObject.transform.DOMove(other.gameObject.transform.position + Vector3.down, 1.5f);
            StackDestroyCube(player, other);
            gameObject.GetComponent<Animator>().applyRootMotion = true;
            AnimationManager.Instance.KickAnimation(_animator);
        }

        if (other.CompareTag("CubeBlue") && gameObject.CompareTag("StackBlue") && !_onTarget)
        {
            _onTarget = true;
            other.gameObject.tag = "Untagged";
            gameObject.transform.SetParent(null);
            gameObject.transform.DOMove(other.gameObject.transform.position + Vector3.down, 1.5f);
            StackDestroyCube(player, other);
            gameObject.GetComponent<Animator>().applyRootMotion = true;
            AnimationManager.Instance.KickAnimation(_animator);
        }

        if (other.CompareTag("CubeYellow") && gameObject.CompareTag("StackYellow") && !_onTarget)
        {
            _onTarget = true;
            other.gameObject.tag = "Untagged";
            gameObject.transform.SetParent(null);
            gameObject.transform.DOMove(other.gameObject.transform.position + Vector3.down, 1.5f);
            StackDestroyCube(player, other);
            gameObject.GetComponent<Animator>().applyRootMotion = true;
            AnimationManager.Instance.KickAnimation(_animator);
        }
    }

    private void StackDestroyCube(PlayerController player, Collider other)
    {
        Destroy(other.gameObject, 1.3f);
        player.stackGameObjectList.Remove(gameObject);
        Destroy(gameObject, 1.3f);
        player.CalculateStackPositions();
    }
}