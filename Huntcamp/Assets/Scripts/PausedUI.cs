using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PausedUI : MonoBehaviour
{
    // Resumes the game
    public void Resume() => GameManager.Instance.PauseGame();

    // Exits the game
    public void ExitGame() => GameManager.Instance.ExitGame();

    // Goes back to main menu
    public void MainMenu() => GameManager.Instance.GoMainMenu();
}
