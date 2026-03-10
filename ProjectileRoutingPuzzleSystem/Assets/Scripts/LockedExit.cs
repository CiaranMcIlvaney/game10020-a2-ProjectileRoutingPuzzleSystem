using UnityEngine;
using UnityEngine.SceneManagement;

public class LockedExit : MonoBehaviour, IHittable
{
    // Sprite used when the door is locked
    public Sprite lockedSprite;
    
    // Sprite used when the door is unlocked
    public Sprite unlockedSprite;

    // Name of the scene to load when the player walks through the exit
    public string sceneName;

    // Collider that physically blocks the player from walking through the door 
    public Collider blockingCollider;

    // Tracks whether the door is currently locked
    bool lockedState = true;

    // Reference to the bridge that rises when the door unlocks
    public Bridge bridge;

    // Reference to the objects SpriteRenderer so the door image can be changed
    SpriteRenderer spriteRenderer;

    // Parent object that contains all directional tunnel pieces (Gets disabled when puzzle is solved)
    public GameObject directionalTunnels;

    void Awake()
    {
        // Get the SpriteRenderer attached to this object
        spriteRenderer = GetComponent<SpriteRenderer>();

        // Set the correct sprite and collider state at the start
        UpdateState();
    }

    // Updates the door apperance and collider based on whether it is locked
    void UpdateState()
    {
        // Change the sprite depending on the locked state
        spriteRenderer.sprite = lockedState ? lockedSprite : unlockedSprite;

        // Enable or disable the blocking collider
        if (blockingCollider != null)
        {
            blockingCollider.enabled = lockedState;
        }
    }

    // Called when the rock hits the door
    public void Hit(GameObject gameObject)
    {
        // If the door is already unlocked do nothing
        if (!lockedState)
        {
            return;
        }

        // Unlock the door
        lockedState = false;

        // Update the doors visuals and collider
        UpdateState();

        // Raise the bridge so the player can continue
        bridge.Move(false);

        // Disable all directional tunnels since the puzzle is solved
        directionalTunnels.SetActive(false);

        // Destroy the rock that hit the door
        Destroy(gameObject);
    }

    // When the player walks into the door trigger after it is unlocked
    private void OnTriggerEnter(Collider other)
    {
        // Check if the player entered the trigger
        if (!lockedState && other.CompareTag("character"))
        {
            // Load to the next scene
            SceneManager.LoadScene(sceneName);
        }
    }
}
