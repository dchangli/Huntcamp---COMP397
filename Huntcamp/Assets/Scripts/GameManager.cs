using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public bool IsGamePaused { get; private set; }

    // This instance will be globally accessible in the code
    public static GameManager Instance { get; private set; }

    // Here the Instance will be asigned if the instance doens't exist yet
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(this);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Event that is triggerd when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (mode == LoadSceneMode.Single)
        {
            PauseGame(true);
        }
    }

    public bool IsSceneLoaded(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCount; i++)
        {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == sceneName)
            {
                return true;
            }
        }
        return false;
    }

    // Will transfer the player to the gameplay scene.
    public void PlayGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    // This will kill the game.
    public void ExitGame()
    {
        Debug.Log("You have exited the game");
        Application.Quit();
    }

    // This will lead you to the Main Menu
    public void GoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    // This will pause the game.
    public void PauseGame(bool forceResume = false, bool forcePause = false)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        if (forcePause)
        {
            IsGamePaused = true;
            Time.timeScale = 0f;
            return;
        }
        IsGamePaused = forceResume ? false : !IsGamePaused;
        try
        {
            if (IsGamePaused)
            {
                Time.timeScale = 0;
                SceneManager.LoadScene("PausedUI", LoadSceneMode.Additive);
            }
            else
            {
                Time.timeScale = 1;
                SceneManager.UnloadScene("PausedUI");
            }
        } catch { }
    }

    private void Update()
    {
        UpdateCursor();
    }

    private void UpdateCursor()
    {
        bool showCursor = (IsGamePaused || SceneManager.GetActiveScene().name == "MainMenu");

        Cursor.lockState = showCursor ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = showCursor;
    }
}


