using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;

public enum TutorialIndexes
{
    Walk = 0,
    Jump = 1,
    Camp = 2,
    Pick = 3,
    Bear = 4,
    Shoot = 5,
    Kill = 6
}

public class Tutorial : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI _textToShow;

    [SerializeField]
    private int _tutorialIndex = 0;

    public static Action<int> TutorialComplete;

    [SerializeField]
    private string[] _tutorialMessages;

    [SerializeField]
    private GameObject TutorialPanel;

    private void Awake()
    {
        TutorialComplete += OnTutorialComplete;
    }

    private void Update()
    {
        _textToShow.text = _tutorialMessages[_tutorialIndex];
    }

    private void OnTutorialComplete(int index)
    {
        if (_tutorialIndex == index)
        {
            if ((index + 1) < _tutorialMessages.Length)
            {
                _tutorialIndex++;
            }
            else
            {
                TutorialPanel.SetActive(false);
            }
            SoundManager.Instance?.PlaySound("Achievement");
        }
    }
}
