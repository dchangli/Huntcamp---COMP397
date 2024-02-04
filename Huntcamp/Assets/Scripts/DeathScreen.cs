using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    void Awake()
    {
        GameManager.Instance.PauseGame(false, true);
    }

    // Play Again
    public void PlayAgain()
    {
        GameManager.Instance.PauseGame(true);
        SetDeathScreen(false);
        Player.Instance.OnRespawn?.Invoke();
    }

    // Exits the game
    public void ExitGame() => GameManager.Instance.ExitGame();

    // Enables or disables the DeathScreen 
    public static void SetDeathScreen(bool enable)
    {
        try
        {
            if (enable)
            {
                if (GameManager.Instance.IsSceneLoaded("DeathScreen")) return;
                SceneManager.LoadScene("DeathScreen", LoadSceneMode.Additive);
            }
            else
            {
                SceneManager.UnloadScene("DeathScreen");
            }
        } catch { }
    }
}
