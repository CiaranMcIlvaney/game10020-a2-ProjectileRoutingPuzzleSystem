using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    // Reference to the CharacterController component used for movement 
    CharacterController controller;

    // Stores the players velocity (for gravity)
    Vector3 velocity;
    
    // Reference to the Animator component to control character animations
    Animator animator;

    // How fast the player moves
    public float moveSpeed = 5.0f;

    // Gravity strength applied to the player
    public float gravity = -9.81f;

    // How strong the rock is thrown
    public float throwForce;
    
    // The rock prefab that will be spawned when throwing
    public Rock rockPrefab;

    // Input reference for player movement (WASD)
    public InputActionReference moveInput;

    // Input reference for the attack / throw button
    public InputActionReference attackInput;

    // The rock held by player
    public Rock rock;

    // The players right arm where the rock spawns from
    public GameObject armRight;

    // Controls whether a new rock should be spawned after throwing
    bool startRockSpawn = false;

    // Timer used to delay spawning the next rock
    float rockTimer = 0.0f;

    void Awake()
    {
        // Get required components attached to the player
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Make sure the rock visual is active when the game starts
        UpdateRockVisual();
    }

    private void Update()
    {
        // Handle player movement every frame
        PlayerMotion();
        
        // Check if the attack button was pressed this frame
        bool attack = attackInput.action.WasPressedThisFrame();

        if (attack)
        {
            // Only throw a rock if the player currently has one
            if (rock != null)
            {
                ThrowRock();

                // Trigger the attack animation
                animator.SetTrigger("StartAttack");
            }
        }

        // If the player has thrown a rock, start the respawn timer
        if (startRockSpawn)
        {
            SpawnRockDelay();
        }
    }

    void PlayerMotion()
    {
        // If the player is grounded keep them slightly stuck to the ground
        if (controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Read movement input from the input system
        Vector2 moveDirection = moveInput.action.ReadValue<Vector2>();

        // Convert input into movement direction in world space
        Vector3 move = Vector3.right * moveDirection.x + Vector3.forward * moveDirection.y;

        // Apply movement speed
        Vector3 moveVelocity = move * moveSpeed;

        // Apply gravity over time
        velocity.y += gravity * Time.deltaTime;
        moveVelocity.y = velocity.y;

        // Move the player using the CharacterController
        controller.Move(moveVelocity * Time.deltaTime);

        // Ignore vertical movement when calculating rotation
        Vector3 horizontalVelocity = new Vector3(moveVelocity.x, 0f, moveVelocity.z);

        // If the player is moving rotate them in the direction they are walking
        if (horizontalVelocity.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(horizontalVelocity);

            // Smoothly rotate toward the movement direction
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 15f * Time.deltaTime);
        }

        // Update the animation speed parameter 
        animator.SetFloat("Speed", horizontalVelocity.magnitude);
    }

    void ThrowRock()
    {
        if (rock != null)
        {
            // The rock will be thrown forward in the direction the player is facing
            Vector3 direction = transform.forward;

            // Tell the rock to launch itself
            rock.Throw(direction, throwForce);

            // The player no longer has a rock after throwing
            rock = null;

            // Start the timer to spawn a new rock
            startRockSpawn = true;
        }
    }

    void SpawnRockDelay()
    {
        // Increase the timer every frame
        rockTimer += Time.deltaTime;

        // Wait half a second before giving the player another rock
        if (rockTimer >= 0.5f)
        {
            rockTimer = 0.0f;
            startRockSpawn = false;

            // Spawn a new rock and attach it to the players arm
            rock = Instantiate(rockPrefab, armRight.transform);

            rock.gameObject.SetActive(true);
        }
    }

    void UpdateRockVisual()
    {
        // Make sure the rock is visible when the player has one
        if (rock != null)
        {
            rock.gameObject.SetActive(true);
        }
    }
}
