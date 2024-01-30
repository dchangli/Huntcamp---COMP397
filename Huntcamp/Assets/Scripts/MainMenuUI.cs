using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUI : MonoBehaviour
{
    // Starts the game
    public void StartGame() => GameManager.Instance.PlayGame();

    // Exits the game
    public void ExitGame() => GameManager.Instance.ExitGame();
}

