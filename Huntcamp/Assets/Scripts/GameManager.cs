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
    public void PauseGame()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu") return;
        IsGamePaused = !IsGamePaused;
        if (IsGamePaused)
        {
            Time.timeScale = 0;
            SceneManager.LoadScene("PausedUI", LoadSceneMode.Additive);
        }
        else
        {
            Time.timeScale = 1;
            SceneManager.UnloadSceneAsync("PausedUI");
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            PauseGame();
        }
    }
}


