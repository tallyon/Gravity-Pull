using UnityEngine;

/// <summary>
/// Various UI functions
/// </summary>
public class UIBehaviour : MonoBehaviour
{
    void Start()
    {
        gameObject.SetActive(true);
    }

    /// <summary>
    /// Goes back to Main Menu - should go back to level select
    /// </summary>
    public void OnBackToLevelSelectButton()
    {
        // Set position of GameOver UI element to off screen
        transform.FindChild("GameOver").GetComponent<RectTransform>().localPosition = new Vector3(2000, 0, 0);

        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black;
        Camera.main.cullingMask = 0;
        gameObject.SetActive(false);

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Restarts current level
    /// </summary>
    public void OnRestartLevel()
    {
        // Set position of GameOver UI element to off screen
        transform.FindChild("GameOver").GetComponent<RectTransform>().localPosition = new Vector3(2000, 0, 0);

        // Load current scene to restart level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }
}
