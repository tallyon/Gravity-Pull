using UnityEngine;

public class UIGameOver : MonoBehaviour
{
    void Start()
    {
        // Subscribe for the event GameOver in PlayerController
        PlayerController.CallOnGameOver += OnGameOver;

        // Set position off screen
        GetComponent<RectTransform>().localPosition = new Vector3(2000, 0, 0);
    }

    private void OnGameOver()
    {
        // Set position to center of the scene
        GetComponent<RectTransform>().localPosition = Vector3.zero;
    }
}
