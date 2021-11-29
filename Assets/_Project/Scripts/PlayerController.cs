using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    #region Fields

    public List<GameObject> stackGameObjectList;
    public List<Vector3> stackVectorList;

    [SerializeField] private float _runSpeed, _slideSpeed, _maxSlideAmount;

    [SerializeField] private Transform _playerModel;
    [SerializeField] private Transform _playerModelStickman;

    [SerializeField] private GameObject _stackedPlayer;

    [SerializeField] private Animator _animator;

    private bool _isPlayerInteract;
    private bool _isFirstLineCreated;

    private float _newLinePositionX = -0.7f;
    private float _newLinePositionZ = -2f;
    private float _currentLinePositionX;
    private int _stackHolder;
    private int _stackNumber;

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
                AnimationController.Instance.IdleAnimation(_animator);
                break;
            case GameState.MainGame:
                ForwardMovement();
                SwerveMovement();
                AnimationController.Instance.RunAnimation(_animator);
                break;
            case GameState.LoseGame:
                break;
            case GameState.WinGame:
                AnimationController.Instance.WinAnimation(_animator);
                _playerModelStickman.transform.rotation = Quaternion.Euler(0, 180, 0);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    private void LateUpdate()
    {
        MergeStackCrowd();
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
        for (int i = 0; i < 2; i++)
        {
            if (other.CompareTag("PlusGreen")) // TODO Seperate IF's
            {
                UIManager.Instance.gold++;
                GameObject stackPlayer = Instantiate(_stackedPlayer, _playerModel.transform.position, _playerModel.transform.rotation);
                stackPlayer.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                stackPlayer.transform.DOScale(new Vector3(2f, 2f, 2f), 0.25f);
                StartCoroutine(PlayerScaleCountDown(stackPlayer));
                stackPlayer.transform.SetParent(_playerModel.transform);

                _stackNumber++;

                stackPlayer.transform.localPosition = new Vector3(_currentLinePositionX, stackPlayer.transform.position.y, _newLinePositionZ);

                if (stackGameObjectList.Count == stackVectorList.Count)
                {
                    stackVectorList.Add(stackPlayer.transform.localPosition);
                }

                stackGameObjectList.Add(stackPlayer);

                stackPlayer.transform.position = _playerModel.transform.position;

                stackPlayer.transform.DOLocalMove(new Vector3(_currentLinePositionX, stackPlayer.transform.position.y, _newLinePositionZ), 0.25f);

                _currentLinePositionX += 0.7f;

                CameraManager.Instance.mainGameCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView += 0.2f;

                if (_stackNumber == 3 && !_isFirstLineCreated)
                {
                    _isFirstLineCreated = true;
                    _newLinePositionX -= 0.7f;
                    _newLinePositionZ -= 2f;
                    _currentLinePositionX = _newLinePositionX;
                    _stackHolder = _stackNumber;
                    _stackNumber = 0;
                }

                if (_stackNumber == _stackHolder + 2 && _stackNumber != 2)
                {
                    _newLinePositionX -= 0.7f;
                    _newLinePositionZ -= 2f;
                    _currentLinePositionX = _newLinePositionX;
                    _stackHolder = _stackNumber;
                    _stackNumber = 0;
                }
            }
        }

        if (stackGameObjectList.Count == 0)
        {
            return;
        }
        else
        {
            for (int i = 0; i < 2; i++)
            {
                if (other.CompareTag("Minus"))
                {
                    stackGameObjectList[stackGameObjectList.Count - 1].gameObject.transform.SetParent(null);
                    stackGameObjectList[stackGameObjectList.Count - 1].gameObject.GetComponent<PlayerStack>().PlayerStackDeath();
                    stackGameObjectList[stackGameObjectList.Count - 1].gameObject.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.black;
                    Destroy(stackGameObjectList[stackGameObjectList.Count - 1], 2f);
                    stackGameObjectList.RemoveAt(stackGameObjectList.Count - 1);
                    _stackNumber--;
                    _currentLinePositionX -= 0.7f;

                    if (_stackNumber == -1)
                    {
                        _newLinePositionX += 0.7f;
                        _newLinePositionZ += 2f;
                        _currentLinePositionX = _newLinePositionX * -1;
                        _stackNumber = _stackHolder - 1;
                        _stackHolder -= 2;
                    }

                    CameraManager.Instance.mainGameCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= 0.2f;
                }
            }
        }
    }

    #endregion


    #region Methods

    public void MergeStackCrowd()
    {
        foreach (var stack in stackGameObjectList)
        {
            for (int i = 0; i < stackGameObjectList.Count; i++)
            {
                if (stackGameObjectList[i].transform.localPosition != stackVectorList[i])
                {
                    stackGameObjectList[i].transform.localPosition = Vector3.MoveTowards(stackGameObjectList[i].transform.localPosition, stackVectorList[i], 0.5f * Time.deltaTime);
                }
            }
        }
    }

    public void CalculateStackPositions()
    {
        _stackNumber--;
        _currentLinePositionX -= 0.7f;

        if (_stackNumber == -1)
        {
            _newLinePositionX += 0.7f;
            _newLinePositionZ += 2f;
            _currentLinePositionX = _newLinePositionX * -1;
            _stackNumber = _stackHolder - 1;
            _stackHolder -= 2;
        }

        CameraManager.Instance.mainGameCam.GetComponent<CinemachineVirtualCamera>().m_Lens.FieldOfView -= 0.2f;
    }

    private void PlayerDeath()
    {
        _isPlayerInteract = true;
        _runSpeed = 0;
        AnimationController.Instance.DeathAnimation(_animator);
        GameManager.Instance.LoseGame();
    }

    #endregion


    #region Coroutines

    public IEnumerator PlayerSpeedDown()
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

    private IEnumerator PlayerScaleCountDown(GameObject stackPlayer)
    {
        yield return new WaitForSeconds(0.15f);
        stackPlayer.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
    }

    #endregion
}