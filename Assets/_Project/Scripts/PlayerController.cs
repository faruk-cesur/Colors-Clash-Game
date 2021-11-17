using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Fields

    [SerializeField] private float _runSpeed, _slideSpeed, _maxSlideAmount;

    [SerializeField] private Transform _playerModel;

    [SerializeField] private GameObject _stackedPlayer;

    private bool _isPlayerInteract;
    private bool _isFirstLineCreated;

    private float _newLinePositionX = -1f;
    private float _newLinePositionZ = -2f;
    private float _currentLinePositionX;
    private int _stackHolder;
    private int _stackNumber;
    [SerializeField] private List<GameObject> _stackPlayerList;

    #endregion

    private void Start()
    {
        _currentLinePositionX = _newLinePositionX;
    }

    private void Update()
    {
        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.PrepareGame:
                AnimationController.Instance.IdleAnimation();
                break;
            case GameState.MainGame:
                ForwardMovement();
                SwerveMovement();
                break;
            case GameState.LoseGame:
                break;
            case GameState.WinGame:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    #region PlayerMovement

    private void ForwardMovement()
    {
        transform.Translate(Vector3.forward * _runSpeed * Time.deltaTime);
    }

    private float _mousePosX;
    private float _playerVisualPosX;

    private void SwerveMovement()
    {
        if (!_isPlayerInteract)
        {
            if (Input.GetMouseButtonDown(0))
            {
                _playerVisualPosX = _playerModel.localPosition.x;
                _mousePosX = CameraManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
            }

            if (Input.GetMouseButton(0))
            {
                float currentMousePosX = CameraManager.Cam.ScreenToViewportPoint(Input.mousePosition).x;
                float distance = currentMousePosX - _mousePosX;
                float posX = _playerVisualPosX + (distance * _slideSpeed);
                Vector3 pos = _playerModel.localPosition;
                pos.x = Mathf.Clamp(posX, -_maxSlideAmount, _maxSlideAmount);
                _playerModel.localPosition = pos;
            }
            else
            {
                Vector3 pos = _playerModel.localPosition;
            }
        }
    }

    #endregion

    #region PlayerTriggerEvents

    private void OnTriggerEnter(Collider other)
    {
        Collectable collectable = other.GetComponentInParent<Collectable>();
        if (collectable)
        {
            UIManager.Instance.gold++;
            //Instantiate(particleCollectable, playerModel.transform.position + new Vector3(0, 2, 0), Quaternion.identity);
            SoundManager.Instance.PlaySound(SoundManager.Instance.collectableSound, 0.4f);
            Destroy(other.gameObject);

            GameObject stackPlayer = Instantiate(_stackedPlayer, transform.position, transform.rotation);
            stackPlayer.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            stackPlayer.transform.DOScale(new Vector3(1, 1, 1), 1);
            stackPlayer.transform.SetParent(this.transform);
            
            _stackPlayerList.Add(stackPlayer);
            _stackNumber++;

            stackPlayer.transform.DOLocalMove(new Vector3(_currentLinePositionX, stackPlayer.transform.position.y, _newLinePositionZ), 1.5f);

            _currentLinePositionX += 1f;

            if (_stackNumber == 3 && !_isFirstLineCreated)
            {
                _isFirstLineCreated = true;
                _newLinePositionX -= 1f;
                _newLinePositionZ -= 2f;
                _currentLinePositionX = _newLinePositionX;
                _stackHolder = _stackNumber;
                _stackNumber = 0;
            }

            if (_stackNumber == _stackHolder + 2 && _stackNumber != 2)
            {
                _newLinePositionX -= 1f;
                _newLinePositionZ -= 2f;
                _currentLinePositionX = _newLinePositionX;
                _stackHolder = _stackNumber;
                _stackNumber = 0;
            }
        }
    }

    #endregion


    #region Methods

    public void PlayerSpeedDown()
    {
        StartCoroutine(FinishGame());
    }

    private void PlayerDeath()
    {
        _runSpeed = 0;
        GameManager.Instance.LoseGame();
        AnimationController.Instance.DeathAnimation();
        StartCoroutine(SoundManager.Instance.LoseGameSound());
        GameManager.Instance.CurrentGameState = GameState.LoseGame;
        _isPlayerInteract = true;
    }

    #endregion


    #region Coroutines

    private IEnumerator FinishGame()
    {
        float timer = 0;
        float fixSpeed = _runSpeed;
        while (true)
        {
            timer += Time.deltaTime;
            _runSpeed = Mathf.Lerp(fixSpeed, 0, timer);

            if (timer >= 1f)
            {
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    private IEnumerator PlayerInteractBool()
    {
        _isPlayerInteract = true;
        yield return new WaitForSeconds(1f);
        _isPlayerInteract = false;
    }

    #endregion
}