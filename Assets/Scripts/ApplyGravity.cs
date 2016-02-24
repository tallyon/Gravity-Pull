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
        set
        {
            gravityField = value;
            // Set radius of collider that checks if objects should be affected by gravity to value in GravityField
            GetComponent<SphereCollider>().radius = gravityField.DistanceOfInfluence;
        }
    }

    void OnTriggerStay(Collider col)
    {
        // If object that is on layer GravitySensitive stays in a trigger then apply gravitational force to it's rigidbody
        if (1 << col.gameObject.layer == gravitySensitive)
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
