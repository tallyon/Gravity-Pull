using UnityEngine;

namespace Assets.Scripts
{
    public class GravityField
    {
        /// <summary>
        /// Corresponds to G * m1 in gravitational attraction formula
        /// </summary>
        public float Magnitude { get; protected set; }

        /// <summary>
        /// Only objects within /DistanceOfInfluence/ of gravitational field will be attracted
        /// </summary>
        public float DistanceOfInfluence { get; protected set; }

        /// <summary>
        /// Returns current position of gravity's GameObject
        /// </summary>
        public Vector3 CenterOfGravity
        {
            get { return GravityGameObject.transform.position; }
        }

        /// <summary>
        /// State of gravitational field for input checking
        /// </summary>
        public bool Activated { get; protected set; }

        private GameObject gravityGameObject;
        public GameObject GravityGameObject
        {
            get { return gravityGameObject; }
            protected set
            {
                if (gravityGameObject != null)
                    gravityGameObject = null;
                gravityGameObject = value;
            }
        }

        /// <summary>
        /// Constructor for GravityField object with /_center/ location, /_magnitude/ attraction strength and /_distInfluence/ distance of attraction influence
        /// </summary>
        /// <param name="_center">Gravity field's center in space</param>
        /// <param name="_magnitude">Strength of attraction</param>
        /// <param name="_distInfluence">Distance past which there is no attraction</param>
        public GravityField(Vector3 _center, float _magnitude, float _distInfluence)
        {
            Vector3 gravPos = _center;
            gravPos.z = 0;
            Magnitude = _magnitude;
            DistanceOfInfluence = _distInfluence;

            // Instantiate GravityField object
            gravityGameObject = Object.Instantiate(Resources.Load("GravityField")) as GameObject;
            gravityGameObject.transform.position = gravPos;

            // Reference current GravityField object in ApplyGravity script that hangs on the prefab of gravity field
            gravityGameObject.GetComponent<ApplyGravity>().GravityField = this;

            // GravityField object should start disabled, so we stop it immediately
            Stop();
        }

        /// <summary>
        /// Set position of gravity center to /pos/
        /// </summary>
        /// <param name="pos">New location of gravity center</param>
        public void SetPosition(Vector3 pos)
        {
            GravityGameObject.transform.position = pos;
        }

        /// <summary>
        /// Move center of gravity to /mv/
        /// </summary>
        /// <param name="mv">New location of gravity center</param>
        public void Move(Vector3 mv)
        {
            GravityGameObject.transform.Translate(mv - GravityGameObject.transform.position);
        }

        /// <summary>
        /// Stops gravity - disables ApplyGravity script and deactivates game object
        /// </summary>
        public void Stop()
        {
            // Disable ApplyGravity script to cease all gravitational influence
            gravityGameObject.GetComponent<ApplyGravity>().enabled = false;
            gravityGameObject.SetActive(false);
            Activated = false;
        }

        /// <summary>
        /// Activates gravity, positions it to /center/ and sets Magnitude to /magnitude/
        /// </summary>
        /// <param name="center">Position</param>
        /// <param name="magnitude">Attraction strength</param>
        public void SpawnGravity(Vector3 center, float magnitude)
        {
            //gravityField = new GravityField(center, magnitude, 20);
            SetPosition(center);
            Magnitude = magnitude;
            gravityGameObject.SetActive(true);
            gravityGameObject.GetComponent<ApplyGravity>().enabled = true;
            Activated = true;
        }
    }
}
