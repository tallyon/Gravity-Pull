using UnityEngine;

public class TrailPointController : MonoBehaviour
{
    public TrailGeneration trailGen;

    // On player entering the trigger let TrailGeneration script know and decide which index of trail point was achieved
    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            trailGen.TrailPointAchieved(transform);
        }
    }
}
