using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    private bool _isFinished;

    private void OnTriggerEnter(Collider other)
    {
        PlayerController playerController = other.GetComponentInParent<PlayerController>();
        if (playerController)
        {
            StartCoroutine(playerController.PlayerSpeedDown());
            WinMiniGame();
            GameManager.Instance.WinGame();
        }
    }

    public void WinMiniGame()
    {
        if (!_isFinished && _player.transform.position.z < transform.position.z + 15.01f &&
            _player.transform.position.z > transform.position.z + 4.99f)
        {
            _isFinished = true;
        }
        else if (!_isFinished && _player.transform.position.z < transform.position.z + 25.01f &&
                 _player.transform.position.z > transform.position.z + 14.99f)
        {
            _isFinished = true;
            UIManager.Instance.gold *= 2;
        }
        else if (!_isFinished && _player.transform.position.z < transform.position.z + 35.01f &&
                 _player.transform.position.z > transform.position.z + 24.99f)
        {
            _isFinished = true;
            UIManager.Instance.gold *= 4;
        }
        else if (!_isFinished && _player.transform.position.z < transform.position.z + 45.01f &&
                 _player.transform.position.z > transform.position.z + 34.99f)
        {
            _isFinished = true;
            UIManager.Instance.gold *= 6;
        }
        else if (!_isFinished && _player.transform.position.z < transform.position.z + 55.01f &&
                 _player.transform.position.z > transform.position.z + 44.99f)
        {
            _isFinished = true;
            UIManager.Instance.gold *= 8;
        }
        else if (!_isFinished && _player.transform.position.z < transform.position.z + 65.01f &&
                 _player.transform.position.z > transform.position.z + 54.99f)
        {
            _isFinished = true;
            UIManager.Instance.gold *= 10;
        }
    }
}