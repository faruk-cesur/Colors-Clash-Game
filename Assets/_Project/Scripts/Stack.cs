using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class Stack : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    private bool _onTarget = false;

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
            gameObject.transform.DOMove(other.gameObject.transform.position, 2f);
            StackDestroyCube(player, other);
        }

        if (other.CompareTag("CubeGreen") && gameObject.CompareTag("StackGreen") && !_onTarget)
        {
            _onTarget = true;
            other.gameObject.tag = "Untagged";
            gameObject.transform.SetParent(null);
            gameObject.transform.DOMove(other.gameObject.transform.position, 2f);
            StackDestroyCube(player, other);
        }

        if (other.CompareTag("CubeBlue") && gameObject.CompareTag("StackBlue") && !_onTarget)
        {
            _onTarget = true;
            other.gameObject.tag = "Untagged";
            gameObject.transform.SetParent(null);
            gameObject.transform.DOMove(other.gameObject.transform.position, 2f);
            StackDestroyCube(player, other);
        }

        if (other.CompareTag("CubeYellow") && gameObject.CompareTag("StackYellow") && !_onTarget)
        {
            _onTarget = true;
            other.gameObject.tag = "Untagged";
            gameObject.transform.SetParent(null);
            gameObject.transform.DOMove(other.gameObject.transform.position, 2f);
            StackDestroyCube(player, other);
        }
    }

    private void StackDestroyCube(PlayerController player, Collider other)
    {
        //gameObject.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        Destroy(other.gameObject, 2f);
        player.stackGameObjectList.Remove(gameObject);

        Destroy(gameObject, 2f);
        player.CalculateStackPositions();
    }
}