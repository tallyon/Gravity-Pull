using UnityEngine;
using Assets.Scripts;

public class TestCubeMovement : MonoBehaviour
{
    GravityField gravityField = null;
    Vector3 newMousePosition;

    void Update()
    {
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Vector3 touchPosition = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                SpawnGravity(touchPosition, 500);
            }
            // Move gravity field's center according to finger movement
            if (Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 newFingerPos = Input.GetTouch(0).position;
                gravityField.Move(Camera.main.ScreenToWorldPoint(newFingerPos));
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Ended && gravityField != null)
            {
                DespawnGravity();
            }
        }
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                SpawnGravity(clickPosition, 300);
            }
            if (Input.GetMouseButtonUp(0) && gravityField != null)
            {
                DespawnGravity();
            }

            // Mouse move
            newMousePosition = Input.mousePosition;

            // Move active gravity field's center according to mouse movement
            if (gravityField != null)
                gravityField.Move(Camera.main.ScreenToWorldPoint(newMousePosition));
        }
    }

    void SpawnGravity(Vector3 center, float magnitude)
    {
        gravityField = new GravityField(center, magnitude, 20);
    }

    void DespawnGravity()
    {
        gravityField.Stop();
        Destroy(gravityField.GravityGameObject);
        gravityField = null;
    }
}
