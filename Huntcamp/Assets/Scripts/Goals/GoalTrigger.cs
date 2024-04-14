using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalTrigger : MonoBehaviour
{
    [SerializeField]
    private TutorialIndexes GoalIndex;

    private void OnTriggerEnter(Collider other)
    {
        Tutorial.TutorialComplete?.Invoke((int)GoalIndex);
    }
}
