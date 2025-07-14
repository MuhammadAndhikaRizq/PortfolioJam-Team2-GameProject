using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameState State;
    public TimeCycle currentTime;

    public static event Action<GameState> OnStateChange;

    void Start()
    {
        UpdateGamesState(GameState.MainMenu);
    }
    void Awake()
    {
        Instance = this;
    }

    #region GameState
    public void UpdateGamesState(GameState newState)
    {
        State = newState;

        switch (newState)
        {
            case GameState.MainMenu:
                HandleMainemenu();
                break;
            case GameState.Playing:
                break;
            case GameState.Summary:
                break;
            case GameState.End:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newState), newState, null);
        }

        OnStateChange?.Invoke(newState);
    }

    private void HandleMainemenu()
    {
        throw new NotImplementedException();
    }
    #endregion


    #region TimeCycle
    public void UpdateTimeCycle(TimeCycle newCycle)
    {
        currentTime = newCycle;

        switch (newCycle)
        {
            case TimeCycle.Morning:
                break;
            case TimeCycle.Night:
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(newCycle), newCycle, null);
        }
    }
    #endregion

}

public enum TimeCycle
{
    Morning, 
    Night
}

public enum GameState
{
    MainMenu,
    Playing,
    Summary,
    End
}
