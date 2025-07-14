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
        UpdateGamesState(GameState.Playing);
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
            case GameState.Playing:
                HandlePlaying();
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

    private void HandlePlaying()
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
    Playing,
    Summary,
    End
}
