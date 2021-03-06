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

    [SerializeField] private GameObject _stackPrefab;

    [SerializeField] private Animator _animator;

    private bool _isPlayerInteract;
    private bool _isFirstLineCreated;
    private bool _isPlayerWin;
    private bool _isPlayerDead;

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
                AnimationManager.Instance.IdleAnimation(_animator);
                break;
            case GameState.MainGame:
                ForwardMovement();
                SwerveMovement();
                AnimationManager.Instance.RunAnimation(_animator);
                break;
            case GameState.LoseGame:
                break;
            case GameState.WinGame:
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
        if (other.CompareTag("PlusRed"))
        {
            GameObject stackPlayer = Instantiate(_stackPrefab, _playerModel.transform.position, _playerModel.transform.rotation);
            stackPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.red;
            stackPlayer.tag = "StackRed";
            stackPlayer.transform.localScale = new Vector3(0.10f, 0.10f, 0.10f);
            stackPlayer.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.25f);
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

            NextStackPosition();
        }

        if (other.CompareTag("PlusGreen"))
        {
            GameObject stackPlayer = Instantiate(_stackPrefab, _playerModel.transform.position, _playerModel.transform.rotation);
            stackPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.green;
            stackPlayer.tag = "StackGreen";
            stackPlayer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            stackPlayer.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.25f);
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

            NextStackPosition();
        }

        if (other.CompareTag("PlusBlue"))
        {
            GameObject stackPlayer = Instantiate(_stackPrefab, _playerModel.transform.position, _playerModel.transform.rotation);
            stackPlayer.GetComponentInChildren<SkinnedMeshRenderer>().material.color = new Color(0, 0.5f, 1f);
            stackPlayer.tag = "StackBlue";
            stackPlayer.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            stackPlayer.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.25f);
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

            NextStackPosition();
        }

        if (other.CompareTag("PlusYellow"))
        {
            GameObject stack = Instantiate(_stackPrefab, _playerModel.transform.position, _playerModel.transform.rotation);
            stack.GetComponentInChildren<SkinnedMeshRenderer>().material.color = Color.yellow;
            stack.tag = "StackYellow";
            stack.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);
            stack.transform.DOScale(new Vector3(1.8f, 1.8f, 1.8f), 0.25f);
            StartCoroutine(PlayerScaleCountDown(stack));
            stack.transform.SetParent(_playerModel.transform);

            _stackNumber++;

            stack.transform.localPosition = new Vector3(_currentLinePositionX, stack.transform.position.y, _newLinePositionZ);

            if (stackGameObjectList.Count == stackVectorList.Count)
            {
                stackVectorList.Add(stack.transform.localPosition);
            }

            stackGameObjectList.Add(stack);

            stack.transform.position = _playerModel.transform.position;

            stack.transform.DOLocalMove(new Vector3(_currentLinePositionX, stack.transform.position.y, _newLinePositionZ), 0.25f);

            NextStackPosition();
        }

        if (other.CompareTag("Win"))
        {
            _isPlayerWin = true;
        }

        if (other.CompareTag("X1"))
        {
            UIManager.Instance.gold *= 1;
            UIManager.Instance.getGoldText.text = "GET X1";
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.CompareTag("X2"))
        {
            UIManager.Instance.gold *= 2;
            UIManager.Instance.getGoldText.text = "GET X2";
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.CompareTag("X4"))
        {
            UIManager.Instance.gold /= 2;
            UIManager.Instance.gold *= 4;
            UIManager.Instance.getGoldText.text = "GET X4";
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.CompareTag("X6"))
        {
            UIManager.Instance.gold /= 4;
            UIManager.Instance.gold *= 6;
            UIManager.Instance.getGoldText.text = "GET X6";
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.CompareTag("X8"))
        {
            UIManager.Instance.gold /= 6;
            UIManager.Instance.gold *= 8;
            UIManager.Instance.getGoldText.text = "GET X8";
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (other.CompareTag("X10"))
        {
            UIManager.Instance.gold /= 8;
            UIManager.Instance.gold *= 10;
            UIManager.Instance.getGoldText.text = "GET X10";
            other.gameObject.GetComponent<Collider>().enabled = false;
        }

        if (stackGameObjectList.Count == 0)
        {
            return;
        }
        else
        {
            if (other.CompareTag("Minus"))
            {
                stackGameObjectList[stackGameObjectList.Count - 1].gameObject.transform.SetParent(null);
                stackGameObjectList[stackGameObjectList.Count - 1].gameObject.GetComponent<Stack>().StackDeath();
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

    private void OnCollisionEnter(Collision other)
    {
        if (!_isPlayerWin && !_isPlayerDead && (other.gameObject.CompareTag("CubeRed") || other.gameObject.CompareTag("CubeGreen") || other.gameObject.CompareTag("CubeBlue") ||
                                                other.gameObject.CompareTag("CubeYellow")))
        {
            _isPlayerDead = true;
            gameObject.GetComponentInChildren<Animator>().applyRootMotion = true;
            PlayerDeath();
        }

        if (_isPlayerWin && (other.gameObject.CompareTag("CubeRed") || other.gameObject.CompareTag("CubeGreen") || other.gameObject.CompareTag("CubeBlue") ||
                             other.gameObject.CompareTag("CubeYellow")))
        {
            _runSpeed = 0;
            gameObject.GetComponentInChildren<Animator>().applyRootMotion = true;
            AnimationManager.Instance.WinAnimation(_animator);
            PlayerWinCondition();
        }

        if (other.gameObject.CompareTag("Win"))
        {
            _runSpeed = 0;
            gameObject.GetComponentInChildren<Animator>().applyRootMotion = true;
            AnimationManager.Instance.WinAnimation(_animator);
            PlayerWinCondition();
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
                    stackGameObjectList[i].transform.localPosition = Vector3.MoveTowards(stackGameObjectList[i].transform.localPosition, stackVectorList[i], 1f * Time.deltaTime);
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

    private void NextStackPosition()
    {
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

    private void PlayerDeath()
    {
        _isPlayerInteract = true;
        _runSpeed = 0;
        AnimationManager.Instance.DeathAnimation(_animator);
        GameManager.Instance.LoseGame();
    }

    private void PlayerWinCondition()
    {
        if (_runSpeed == 0 && _isPlayerWin)
        {
            GameManager.Instance.WinGame();
        }
    }

    private IEnumerator PlayerScaleCountDown(GameObject stackPlayer)
    {
        yield return new WaitForSeconds(0.10f);
        stackPlayer.transform.DOScale(new Vector3(1, 1, 1), 0.25f);
    }

    #endregion
}