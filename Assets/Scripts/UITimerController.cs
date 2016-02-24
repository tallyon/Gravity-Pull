using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;
using System.Diagnostics;

public class UITimerController : MonoBehaviour
{
    private Text timerText;
    private LevelTimer levelTimer;

    // Use this for initialization
    void Start()
    {
        timerText = GetComponent<Text>();
        levelTimer = LevelTimer.Instance;
        // Subscribe to receive notifications when trail is completed
        TrailGeneration.CallOnTrailCompleted += TrailCompleted;
    }

    /// <summary>
    /// Called when player hit last Trail Point on the level
    /// </summary>
    private void TrailCompleted(ScoreType score)
    {
        // Make sure that at the completion of the trail timer shows right finish time
        timerText.text = levelTimer.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (levelTimer.IsRunning)
            timerText.text = levelTimer.ToString();
    }
}
