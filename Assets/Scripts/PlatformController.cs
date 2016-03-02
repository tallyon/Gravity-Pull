using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Controls movement of the platform it's attached to by leading it through all the points in space specified with constant speed
/// </summary>
public class PlatformController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private List<Transform> stops = new List<Transform>();
    private int index = 0;
    private Vector3 direction;
    [SerializeField]
    private Transform cube;

    void Start()
    {
        // Add starting platform position at the start of stops list
        GameObject stop0 = new GameObject("stop0");
        stop0.transform.SetParent(transform);
        stop0.transform.position = cube.position;
        stops.Insert(0, stop0.transform);
    }

    void Update()
    {
        direction = (stops[index].position - cube.position).normalized;
        cube.position += direction * speed * Time.deltaTime;

        if (Vector3.Distance(stops[index].position, cube.position) < .1f)
        {
            if (index == stops.Count - 1)
            {
                index = 0;
            }
            else
            {
                index++;
            }
        }
    }

}
