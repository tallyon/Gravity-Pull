using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class UISummary : MonoBehaviour
{
    private PlayerController player;
    [SerializeField]
    private Text timeText = null, healthText = null, scoreText = null;

    void Start()
    {
        player = FindObjectOfType<PlayerController>();
        // Subscribe to TrailGeneration to call OnTrailCompleted when trail is completed
        TrailGeneration.CallOnTrailCompleted += OnTrailCompleted;
        // Set starting position to off screen
        GetComponent<RectTransform>().localPosition = new Vector3(2000, 0, 0);
    }

    private void OnTrailCompleted(ScoreType score)
    {
        // Set position to center of the scene
        GetComponent<RectTransform>().localPosition = Vector3.zero;
        timeText.text = score.TimeFormatted();
        scoreText.text = score.Score.ToString();
        healthText.text = player.Health.ToString();
    }
}
