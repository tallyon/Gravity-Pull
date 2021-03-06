﻿using UnityEngine;
using Assets.Scripts;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Text verticalAcc = null, horizontalAcc = null;
    [SerializeField]
    private Text verticalVel = null, horizontalVel = null;
    
    private GravityField gravityField;

    private Rigidbody rbd;
    [SerializeField]
    private Renderer rendererPlayer;
    private Vector3 newMousePosition;

    [SerializeField]
    private float speedModifier;
    [SerializeField]
    private float maxSpeed;
    [SerializeField]
    private float gravityMagnitude;
    private bool paused = false;

    // Event fired when Health reaches 0
    public delegate void OnGameOver();
    public static event OnGameOver CallOnGameOver;

    private int health;
    public int Health
    {
        get { return health; }
        protected set
        {
            if (value <= 0)
            {
                health = 0;
                CallOnGameOver();
            }
            else
                health = value;
        }
    }
    public bool IsAlive
    {
        get { return Health > 0; }
    }

    public int MaxHealth { get; protected set; }
    private bool invincibilityFrame = false;
    [SerializeField]
    private float invincibilityTime;

    private float horizontalAxis, verticalAxis;
    private float horizontalAxisSignInLastFrame = 0;

    void Awake()
    {
        CallOnGameOver = null;
    }

    void Start()
    {
        if (rendererPlayer == null)
            rendererPlayer = GameObject.Find("Default").GetComponent<Renderer>();
        rbd = GetComponent<Rigidbody>();
        Health = MaxHealth = 100;

        // Default invincibility time should be .5 second
        if (invincibilityTime == 0)
            invincibilityTime = .5f;

        // Instantiate GravityField
        gravityField = new GravityField(transform.position, 500, 20);

        // Subscribe to event that fires when trail is completed
        TrailController.CallOnTrailCompleted += OnTrailCompleted;

        // Show appropriate popup for the level
        switch (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name)
        {
            case "Level01":
                UIBehaviour.PopupText(Vector3.zero, "Small obstacles can be easily moved by gravitational attraction, use it to move them away!\n[Touch the screen]", Color.yellow);
                break;
            case "Level02":
                UIBehaviour.PopupText(Vector3.zero, "Moving platforms cannot be attracted by gravity - dodge them!", Color.yellow);
                break;
            case "Level03":
                UIBehaviour.PopupText(Vector3.zero, "Level not implemented - loaded Level01", Color.yellow);
                break;
        }
    }

    void Update()
    {
        // Do not run when paused or dead
        if (paused || !IsAlive)
        {
            rbd.isKinematic = true;
            return;
        }

        //************************************************
        // Input handling

        // Horizontal and vertical axis to control the ship movement
        horizontalAxis = 0;
        verticalAxis = 0;

        // Handheld
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            // Horizontal dead zone
            if (Mathf.Abs(Input.acceleration.x) > .1f)
                horizontalAxis = Input.acceleration.x * 6;

            // Vertical dead zone
            if (Mathf.Abs(Input.acceleration.y) > .2f)
            {
                // Stopping movement with accelerometer is 2 times more sensitive than accelerating, which is 2 times less sensitive than raw input
                if (Input.acceleration.y < 0)
                    verticalAxis += Input.acceleration.y * 2;
                else
                    verticalAxis += Input.acceleration.y * .5f;
            }

            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    Vector3 touchPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    gravityField.SpawnGravity(touchPosition, gravityMagnitude);
                }
                // Move gravity field's center according to finger movement
                else if (touch.phase == TouchPhase.Ended && gravityField.Activated)
                {
                    gravityField.Stop();
                }

                // Touch position
                newMousePosition = touch.position;

                break;
            }
        }
        // Desktop
        else if (SystemInfo.deviceType == DeviceType.Desktop)
        {
            // Left click
            if (Input.GetMouseButtonDown(0))
            {
                Vector3 clickPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                gravityField.SpawnGravity(clickPosition, gravityMagnitude);
            }
            // Gravity despawn
            if (Input.GetMouseButtonUp(0) && gravityField.Activated)
            {
                gravityField.Stop();
            }

            horizontalAxis = Input.GetAxisRaw("Horizontal");
            if (Input.GetAxisRaw("Vertical") < 0)
                verticalAxis += Input.GetAxisRaw("Vertical") * 2;
            else
                verticalAxis += Input.GetAxisRaw("Vertical") * .5f;

            // Mouse position
            newMousePosition = Input.mousePosition;
        }

        // Clamp movement to {-1;1}
        horizontalAxis = Mathf.Clamp(horizontalAxis, -1, 1);
        verticalAxis = Mathf.Clamp(verticalAxis, -2, 1);

        // DEBUG
        verticalAcc.text = "Y:" + verticalAxis.ToString();
        horizontalAcc.text = "X: " + horizontalAxis.ToString();
        verticalVel.text = "Vertical velocity: " + rbd.velocity.y;
        horizontalVel.text = "Horizontal velocity: " + rbd.velocity.x;
        //************************************************
    }

    void LateUpdate()
    {
        // Move active gravity field's center according to mouse/touch position
        if (gravityField.Activated)
            gravityField.Move(Camera.main.ScreenToWorldPoint(newMousePosition));
    }

    void FixedUpdate()
    {
        Vector3 inputMove = new Vector3(horizontalAxis, 1 + verticalAxis);
        inputMove.Scale(new Vector3(speedModifier, speedModifier, 0));

        Vector3 moveForce = inputMove * Time.deltaTime;
        rbd.AddForce(moveForce, ForceMode.VelocityChange);

        // Zero the horizontal force on rigidbody if sign of horizontal input changed
        float horizontalAxisSign = horizontalAxis == 0 ? 0 : Mathf.Sign(horizontalAxis);
        if (horizontalAxisSign != horizontalAxisSignInLastFrame)
        {
            Vector3 stoppingHorizontalForce = new Vector3(-rbd.velocity.x, 0, 0);
            rbd.AddForce(stoppingHorizontalForce, ForceMode.VelocityChange);
        }

        // Prevent movement back down the level clamping velocity.y to 1 if it goes lower than 1
        if (rbd.velocity.y < 1)
        {
            Vector3 stoppingForce = new Vector3(0, -rbd.velocity.y + 1, 0);
            rbd.AddForce(stoppingForce, ForceMode.VelocityChange);
        }

        // Cap velocity to maxSpeed
        if (Mathf.Abs(rbd.velocity.x) > maxSpeed || Mathf.Abs(rbd.velocity.y) > maxSpeed)
        {
            Vector3 stoppingForce = new Vector3();
            if (rbd.velocity.x > maxSpeed)
                stoppingForce.x = -moveForce.x;
            if (rbd.velocity.y > maxSpeed)
                stoppingForce.y = -moveForce.y;

            rbd.AddForce(stoppingForce, ForceMode.VelocityChange);
        }

        // There is need for distinguishing between negative, zero and positive sign: negative = -1, zero = 0, positive = 1
        horizontalAxisSignInLastFrame = horizontalAxis == 0 ? 0 : Mathf.Sign(horizontalAxis);

        // Freeze rigidbody rotation to 0
        rbd.MoveRotation(Quaternion.identity);
    }

    // Stop the game (time) after finishing the trail
    void OnTrailCompleted(ScoreType score)
    {
        // If gravity field is active despawn it
        gravityField.Stop();
        Time.timeScale = 0;
        paused = true;
    }

    #region Collision
    // Collision should happen only with walls, all the other obstacles are triggers
    void OnCollisionEnter(Collision col)
    {
        // If collided with wall apply force aligned with vertical direction of normal of the wall surface
        if (col.transform.tag == "Wall")
        {
            Vector3 colNormal = col.contacts[0].normal;
            Vector3 bounceForce = new Vector3(10 * Mathf.Sign(colNormal.x), 0, 0);
            rbd.AddForce(bounceForce, ForceMode.VelocityChange);
        }

        // Do not take damage if invincible
        if (invincibilityFrame)
            return;

        TakeDamage(10);
        StartInvincibilityFrame();
    }

    // Triggers are all obstacles except walls - hitting trigger obstacle makes player lose velocity
    void OnTriggerEnter(Collider col)
    {
        if (invincibilityFrame)
            return;

        if (col.CompareTag("Platform") || col.CompareTag("CubeObstacle"))
        {
            // Take damage
            if (col.CompareTag("Platform"))
                TakeDamage(10);
            else if (col.CompareTag("CubeObstacle"))
                TakeDamage(5);
            StartInvincibilityFrame();

            // Apply bounce force - slow player after hitting an obstacle
            Vector3 normalToObstalce = (transform.position - col.transform.position).normalized;
            Vector3 bounceForce = new Vector3(Mathf.Sign(normalToObstalce.x) * rbd.velocity.x, -rbd.velocity.y, 0);
            rbd.AddForce(bounceForce, ForceMode.VelocityChange);

            // Fire particle system
            // Find ParticleSystem in children
            ParticleSystem particles = col.gameObject.GetComponentInChildren<ParticleSystem>();
            // Play particle system
            particles.Play();
            // Unparent particle system
            particles.transform.SetParent(null);

            // Deactivate object that was triggered
            Debug.Log("Destroy");
            col.gameObject.SetActive(false);
        }
    }

    private void TakeDamage(int damage)
    {
        Health -= damage;
    }

    private void StartInvincibilityFrame()
    {
        // Set flag
        invincibilityFrame = true;
        // Change alpha of player to indicate being invincible
        rendererPlayer.material.color -= new Color(1, 1, 1, .5f);
        Invoke("EndInvincibilityFrame", invincibilityTime);
    }

    /// <summary>
    /// Enables collider and sets invincibility flag off
    /// </summary>
    private void EndInvincibilityFrame()
    {
        // Set flag back
        invincibilityFrame = false;
        rendererPlayer.material.color += new Color(1, 1, 1, .5f);
        // Change alpha of player back to normal
    }
    #endregion
}
