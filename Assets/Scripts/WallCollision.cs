using UnityEngine;

namespace Assets.Scripts
{
    class WallCollision : MonoBehaviour
    {
        void OnTriggerEnter(Collider col)
        {
            // If triggered Cube then apply bounce force to it
            if (col.CompareTag("CubeObstacle"))
            {
                // Apply bounce force that mirrors horizontal velocity of Cube
                Rigidbody colRigidbody = col.gameObject.GetComponent<Rigidbody>();
                Vector3 bounceForce = new Vector3(colRigidbody.velocity.x * -2, 0, 0);
                colRigidbody.AddForce(bounceForce, ForceMode.VelocityChange);
            }
        }
    }
}
