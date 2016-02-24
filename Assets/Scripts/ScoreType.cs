using System;
using UnityEngine;

/// <summary>
/// Score is saved in PlayerPrefs as pairs ("Level01", 1); ("Level01_Score", score); ("Level01_Time", time);
/// </summary>
namespace Assets.Scripts
{
    public class ScoreType
    {
        public string LevelName { get; protected set; }
        public long Time { get; protected set; }
        public int Score { get; protected set; }

        /// <summary>
        /// Create ScoreType object for saving score.
        /// </summary>
        /// <param name="name">Level name</param>
        /// <param name="_time">Level time at completion</param>
        /// <param name="_score">Level score at completion</param>
        public ScoreType(string name, long _time, int _score)
        {
            LevelName = name;
            Time = _time;
            Score = _score;
        }

        public void SaveScore()
        {
            // Check if current score - if any - is lower then currently achieved
            if (PlayerPrefs.GetInt(LevelName + "_Score") < Score)
            {
                // Set keys for level
                PlayerPrefs.SetInt(LevelName, 1);
                PlayerPrefs.SetInt(LevelName + "_Score", Score);
                PlayerPrefs.SetInt(LevelName + "_Time", (int)Time);
                PlayerPrefs.Save();
                Debug.Log("Saved score: " + ToString());
            }
            else
                Debug.Log("Got worse score! Saved: " + PlayerPrefs.GetInt(LevelName + "_Score").ToString() + ", achieved: " + Score.ToString());
        }

        /// <summary>
        /// Get ScoreType score object for /levelName/. If there is no /levelName/ saved return (levelName, -1, -1) object
        /// </summary>
        /// <param name="levelName">Name of the level</param>
        /// <returns></returns>
        public static ScoreType LoadLevelScore(string levelName)
        {
            if (PlayerPrefs.HasKey(levelName))
            {
                int time = PlayerPrefs.GetInt(levelName + "_Time");
                int score = PlayerPrefs.GetInt(levelName + "_Score");
                ScoreType returnScore = new ScoreType(levelName, time, score);
                return returnScore;
            }
            else
            {
                ScoreType invalidScore = new ScoreType(levelName, -1, -1);
                return invalidScore;
            }
        }

        public string TimeFormatted()
        {
            return (Time / 60000).ToString("D2") + ":" + ((Time / 1000) % 60).ToString("D2") + ":" + (Time % 1000).ToString("D3");
        }

        public override string ToString()
        {
            string returnString = "Level name: " + LevelName + "; Score: " + Score + "; Time: " + Time;
            return returnString;
        }
    }
}
