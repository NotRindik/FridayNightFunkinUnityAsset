using FridayNightFunkin;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;

public class PauseMenu : MenuBehaviour
{
    [SerializeField] protected UnityEvent OnEscapeUnpressed;
    private bool isPressed;
    public PlayableDirector director;

    public override void Escape()
    {
        isPressed = !isPressed;

        if (isPressed)
            OnEscapePressed?.Invoke();
        else
            OnEscapeUnpressed?.Invoke();

        GameStates();
    }

    private void GameStates()
    {
        GameState currentGameState = GameStateManager.instance.CurrentGameState;
        GameState newGameState = currentGameState == GameState.GamePlay ? GameState.Paused : GameState.GamePlay;

        GameStateManager.instance.SetState(newGameState);
    }

    public void RestartSong()
    {
        GameStateManager.instance.SetState(GameState.GamePlay);
        SceneLoad.instance.StartLoad(SceneManager.GetActiveScene().name);
    }
    
    public void ChangeDifficult(int difficult)
    {
        PlayerPrefs.SetInt("Difficult", difficult);
        RestartSong();
    }

    public void ExitToMenu()
    {
        GameStateManager.instance.SetState(GameState.GamePlay);
        PlayerPrefs.SetInt("AfterLevel", 1);
        SceneLoad.instance.StartLoad("MainMenu");
    }
}
