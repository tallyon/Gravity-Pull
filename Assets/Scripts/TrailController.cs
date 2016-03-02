using UnityEngine;
using System.Collections.Generic;
using Assets.Scripts;

public class TrailController : MonoBehaviour
{
    private List<Transform> trailList = new List<Transform>();
    public List<Transform> TrailList
    {
        get { return trailList; }
        private set { trailList = value; }
    }

    public Transform TargetTrailPoint { get; set; }

    public int CurrentTrailPointIndex { get; set; }

    public delegate void OnTrailPointAchieved(int current, int max);
    public static event OnTrailPointAchieved CallOnTrailPointAchieved;

    [SerializeField]
    private GameObject UISummary;

    public delegate void OnTrailCompleted(ScoreType score);
    public static event OnTrailCompleted CallOnTrailCompleted;

    [SerializeField]
    private bool drawTrailLines;
    private LevelTimer levelTimer;
    [SerializeField]
    private PlayerController player;
    [SerializeField]
    private CameraFollowPlayer cameraFollowScript;

    void Awake()
    {
        CallOnTrailCompleted = null;
        CallOnTrailPointAchieved = null;
    }

    void Start()
    {
        if (player == null)
            player = FindObjectOfType<PlayerController>();
        if (UISummary == null)
            UISummary = FindObjectOfType<UISummary>().gameObject;
        if (cameraFollowScript == null)
            cameraFollowScript = FindObjectOfType<CameraFollowPlayer>();
        GenerateTrailList();
        CurrentTrailPointIndex = 0;
        TrailPointAchieved(TrailList[0]);
        levelTimer = LevelTimer.Instance;
        levelTimer.Start();
    }

    void Update()
    {
        if (drawTrailLines) DrawTrail(TrailList);
    }

    /// <summary>
    /// Traverse all Trail's children to create list of Transform elements of trail points that form valid trail for player in game.
    /// </summary>
    private void GenerateTrailList()
    {
        trailList.Clear();

        // First element in trail list is always player's position
        TrailList.Add(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>());

        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            TrailList.Add(child);
        }

        // Inform all subscribers that we are on 0 trailPoint and how much are there
        if (CallOnTrailPointAchieved != null)
            CallOnTrailPointAchieved(0, TrailList.Count - 1);
    }

    /// <summary>
    /// Draws lines between each trail point in generated trail contained inside /trailList/.
    /// </summary>
    /// <param name="trail"></param>
    private void DrawTrail(List<Transform> trail)
    {
        for (int i = CurrentTrailPointIndex; i < trail.Count - 1; i++)
        {
            Debug.DrawLine(trail[i].position, trail[i + 1].position, Color.blue);
        }
    }

    /// <summary>
    /// Based on input /trailPoint/ sets current and next trail point
    /// </summary>
    /// <param name="trailPoint">Trail point that was just achieved</param>
    public void TrailPointAchieved(Transform trailPoint)
    {
        // Set currently achieved trail point
        int achievedIndex = FindTrailPointInList(trailPoint, TrailList);
        CurrentTrailPointIndex = achievedIndex;
        // Disable BoxCollider on current trail point - if it's not player (0)
        if (CurrentTrailPointIndex > 0)
            TrailList[CurrentTrailPointIndex].GetComponent<BoxCollider>().enabled = false;

        // If the trail point achieved is the first one - enable camera follow
        if (CurrentTrailPointIndex == 1)
            cameraFollowScript.StartFollowing = true;

        // Set next trail point if it wasn't the last one
        if (CurrentTrailPointIndex < TrailList.Count - 1)
        {
            TargetTrailPoint = TrailList[achievedIndex + 1];
            // Enable BoxCollider on target trail point
            TargetTrailPoint.GetComponent<BoxCollider>().enabled = true;
        }
        else
        {
            // If achieved last trail point handle events on completion of the level
            TrailCompleted();
        }

        // Inform all subscribers what is the current trail point and total number of trail points
        if (CallOnTrailPointAchieved != null)
            CallOnTrailPointAchieved(CurrentTrailPointIndex, TrailList.Count - 1);
    }

    /// <summary>
    /// Returns index int of /trailPoint/ in list /trail/.
    /// </summary>
    /// <param name="trailPoint">Trail point to find</param>
    /// <param name="trail">List of trail points</param>
    /// <returns>Index of /trailPoint/</returns>
    private int FindTrailPointInList(Transform trailPoint, List<Transform> trail)
    {
        return trail.FindIndex(
            delegate (Transform trp)
            {
                return trp.GetInstanceID() == trailPoint.GetInstanceID();
            }
            );
    }

    /// <summary>
    /// Prepare summary UI of the level, save score and send event that trail was completed.
    /// </summary>
    private void TrailCompleted()
    {
        // Activate the summary
        UISummary.SetActive(true);
        long timeMSElapsed = levelTimer.Stop();
        int scorePoints = (int)timeMSElapsed * player.Health / player.MaxHealth;
        ScoreType score = new ScoreType(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name, timeMSElapsed, scorePoints);
        score.SaveScore();

        // Fire the event that trail was completed
        if (CallOnTrailCompleted != null)
            CallOnTrailCompleted(score);
    }
}
