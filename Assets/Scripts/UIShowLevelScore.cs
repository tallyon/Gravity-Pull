using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts;

public class UIShowLevelScore : MonoBehaviour
{
    private string levelName = string.Empty;
    [SerializeField]
    private Text score = null, time = null;

    void Start()
    {
        // Scene name should be exactly the name of the level in saved scores
        levelName = name;
        ScoreType scoreForTheLevel = ScoreType.LoadLevelScore(levelName);

        if (scoreForTheLevel.Score > -1 && scoreForTheLevel.Time > -1)
        {
            // Set score and time text
            score.text = scoreForTheLevel.Score.ToString();
            time.text = (scoreForTheLevel.Time / 60000).ToString("D2") + ":"
                + ((scoreForTheLevel.Time / 1000) % 60).ToString("D2") + ":"
                + (scoreForTheLevel.Time % 1000).ToString("D3");

            // Set color of button's background to grey
            ColorBlock buttonColor = GetComponent<Button>().colors;
            buttonColor.normalColor = new Color(.7f, .7f, .7f);
            GetComponent<Button>().colors = buttonColor;
        }
        else
        {
            score.text = " - ";
            time.text = " - ";
        }
    }
}