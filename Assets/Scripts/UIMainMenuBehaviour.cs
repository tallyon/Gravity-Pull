using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class UIMainMenuBehaviour : MonoBehaviour
{
    [SerializeField]
    private RectTransform levelSelect = null, mainMenu = null, loadingScreen = null;
    [SerializeField]
    private Vector3 levelSelectPosition;

    private Vector2 uiCenterPosition = new Vector2(0, 0);
    private Vector2 uiOffscreenPosition = new Vector2();
    private List<RectTransform> uiElements = new List<RectTransform>();

    [SerializeField]
    private int sceneToLoad;
    public int SceneToLoad
    {
        get { return sceneToLoad; }
        protected set { sceneToLoad = value; }
    }

    void Start()
    {
        float canvasWidth = levelSelectPosition.x;
        uiOffscreenPosition.Set(canvasWidth, 0);

        uiElements.Add(mainMenu);
        uiElements.Add(levelSelect);
        uiElements.Add(loadingScreen);

        CenterUIElement(mainMenu);
    }

    /// <summary>
    /// Centers /uiElement/'s RectTransform to the visible area. Other UI elements are stacked to the right
    /// </summary>
    /// <param name="uiElement">UI element's RectTransform</param>
    private void CenterUIElement(RectTransform uiElement)
    {
        for (int i = 0; i < uiElements.Count; i++)
        {
            if (uiElements[i] == uiElement)
                uiElements[i].localPosition = uiCenterPosition;
            else
                uiElements[i].localPosition = uiOffscreenPosition;
        }
    }

    public void PrepareLoadingScreen(RectTransform uiLoadingScreen)
    {
        Camera.main.clearFlags = CameraClearFlags.SolidColor;
        Camera.main.backgroundColor = Color.black;

        CenterUIElement(uiLoadingScreen);
    }

    /// <summary>
    /// Move UICamera to Level select UI
    /// </summary>
    public void OnClick_StartButton() { CenterUIElement(levelSelect); }

    public void OnClick_QuitButton() { Application.Quit(); }

    public void OnClick_BackButton_LevelSelect() { CenterUIElement(mainMenu); }

    public void OnClick_LevelSelected(string levelName)
    {
        PrepareLoadingScreen(loadingScreen);
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelName);
    }
}
