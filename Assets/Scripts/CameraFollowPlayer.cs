using UnityEngine;
using System.Collections;

public class CameraFollowPlayer : MonoBehaviour
{
    private Transform player;
    public float yOffset;

    [SerializeField]
    private bool startFollowing;
    public bool StartFollowing
    {
        get { return startFollowing; }
        set { startFollowing = value; }
    }

    void Start()
    {
        player = GameObject.Find("Player").transform;
        if (player == null)
            Debug.Log("Player not found");
    }

    void Update()
    {
        if (StartFollowing)
        {
            // Set only y position to keep up with the player
            Vector3 newPos = new Vector3(transform.position.x, player.position.y + yOffset, transform.position.z);
            transform.position = newPos;
        }
    }
}
