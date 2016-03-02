using UnityEngine;
using UnityEngine.UI;

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

        Time.timeScale = 1;

        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    /// <summary>
    /// Restarts current level
    /// </summary>
    public void OnRestartLevel()
    {
        // Set position of GameOver UI element to off screen
        transform.FindChild("GameOver").GetComponent<RectTransform>().localPosition = new Vector3(2000, 0, 0);

        Time.timeScale = 1;

        // Load current scene to restart level
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                                                            UnityEngine.SceneManagement.LoadSceneMode.Single);
    }

    /// <summary>
    /// Create text field in UI at local position /position/, with /text/ using text color /color/. Destroys itself after /time/ seconds, default 5.
    /// </summary>
    /// <param name="position">Local position</param>
    /// <param name="text">String that will be the text</param>
    /// <param name="color">Text color</param>
    /// <param name="time">Time after which popup disappears, default 5 seconds</param>
    public static void PopupText(Vector3 position, string text, Color color, float time = 5)
    {
        Canvas canvasObject = FindObjectOfType<Canvas>();
        GameObject textPopup = new GameObject();
        textPopup.transform.SetParent(canvasObject.transform);
        textPopup.transform.localScale = Vector3.one;
        textPopup.transform.localPosition = position;
        textPopup.name = "textPopup_ID_" + textPopup.GetInstanceID().ToString();
        Text textElement = textPopup.AddComponent<Text>();
        textPopup.GetComponent<RectTransform>().sizeDelta = new Vector2(400, 150);
        textPopup.GetComponent<RectTransform>().anchoredPosition = canvasObject.GetComponent<RectTransform>().rect.center;
        Color yellowTextColor = new Color(246, 255, 95);
        textElement.font = Resources.Load<Font>("ARIAL");
        textElement.font.material.color = yellowTextColor;
        textElement.fontSize = 25;
        textElement.text = text;
        textElement.alignment = TextAnchor.MiddleLeft;
        textElement.color = color;

        // Destroy popup after 5 seconds
        Destroy(textPopup, time);
    }
}
