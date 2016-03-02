using UnityEngine;
using Assets.Scripts;

public class ApplyGravity : MonoBehaviour
{
    [SerializeField]
    private LayerMask gravitySensitive;
    private GravityField gravityField;

    public GravityField GravityField
    {
        get { return gravityField; }
        set { gravityField = value; }
    }

    void FixedUpdate()
    {
        // LayerMask 1 << 9 is 9th layer mask - which should be GravitySensitive
        Collider[] collided = Physics.OverlapSphere(gravityField.CenterOfGravity, gravityField.DistanceOfInfluence, 1 << 9);

        foreach (Collider col in collided)
        {
            Rigidbody rbd = col.attachedRigidbody;
            if (rbd != null)
            {
                Vector3 normalizedGravityVector = Vector3.Normalize(transform.position - col.transform.position);
                //float sqrDist = Vector3.SqrMagnitude(transform.position - col.transform.position);
                float dist = Vector3.Distance(transform.position, col.transform.position);

                // Gravitational attraction in actual world is G * m1 * m2 / r^2, but here we use r instead of r^2 to make attraction much more dynamic
                // The G * m1 is the GravityField.Magnitude set in PlayerController script when setting up the GravityField object
                Vector3 gravitationalAttractionForce = normalizedGravityVector * gravityField.Magnitude * rbd.mass / dist;
                rbd.AddForce(gravitationalAttractionForce);
            }
        }
    }
}
