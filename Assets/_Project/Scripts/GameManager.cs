using System;
using UnityEngine;

public enum GameState
{
    PrepareGame,
    MainGame,
    LoseGame,
    WinGame
}

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance => _instance;

    private GameState _currentGameState;

    public GameState CurrentGameState
    {
        get { return _currentGameState; }
        set
        {
            switch (value)
            {
                case GameState.PrepareGame:
                    break;
                case GameState.MainGame:
                    break;
                case GameState.LoseGame:
                    break;
                case GameState.WinGame:
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(value), value, null);
            }

            _currentGameState = value;
        }
    }

    private void Awake()
    {
        Application.targetFrameRate = 60;
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }

        CurrentGameState = GameState.PrepareGame;
    }

    public void StartGame()
    {
        CurrentGameState = GameState.MainGame;
        UIManager.Instance.MainGameUI();
        CameraManager.Instance.MainGameCamera();
    }

    public void RestartGame()
    {
        UIManager.Instance.RetryButton();
    }

    public void NextLevel()
    {
        UIManager.Instance.NextLevelButton();
    }

    public void LoseGame()
    {
        StartCoroutine(UIManager.Instance.DurationLoseGameUI());
        //StartCoroutine(SoundManager.Instance.LoseGameSound());
        CameraManager.Instance.LoseGameCamera();
        CurrentGameState = GameState.LoseGame;
    }

    public void WinGame()
    {
        StartCoroutine(UIManager.Instance.DurationWinGameUI());
        CameraManager.Instance.WinGameCamera();
        //SoundManager.Instance.PlaySound(SoundManager.Instance.winGameSound, 1);
        CurrentGameState = GameState.WinGame;
    }
}