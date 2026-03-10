using UnityEngine;

public class DirectionalTunnel : MonoBehaviour
{
    // The direction the tunnel pushes when the switch is not pressed
    public Vector3 normalDirection;

    // The direction the tunnel pushes when the switch is pressed
    public Vector3 reversedDirection;

    // How strong the tunnel pushes the rock
    public float pushForce;
    
    // Renderer used to change the tunnels color
    public Renderer tunnelRenderer;

    // Color used when the tunnel is in normal state
    public Color normalColor;

    // Color used when the tunnel is reversed
    public Color reversedColor;

    // Arrow shown when the tunnel is in the normal direction
    public GameObject normalArrow;

    // Arrow shown when the tunnel is reversed
    public GameObject reversedArrow;

    // Stores the current direction the tunnel is pushing objects
    Vector3 currentDirection;

    // Tracks whether the tunnel is currently reversed or not
    bool isReversed = false;

    void Start()
    {
        // When the scene starts the tunnel begins in the normal direction
        currentDirection = normalDirection;

        // Update the visual apperance to match the starting state
        UpdateVisuals();
    }

    // This function tells the tunnel whether it should reverse direction 
    public void ReverseDirection(bool switchState)
    {
        // Save the current reversed state
        isReversed = switchState;

        // Change the push direction depending on the switch state
        if (isReversed)
        {
            currentDirection = reversedDirection;
        }
        else
        {
            currentDirection = normalDirection;
        }

        // Update the color and arrow visuals
        UpdateVisuals();
    }

    // Updates the tunnels color and arrow visuals
    void UpdateVisuals()
    {
        // Change the tunnel depending on its current state
        if (isReversed)
        {
            tunnelRenderer.material.color = reversedColor;
        }
        else
        {
            tunnelRenderer.material.color = normalColor;
        }

        // Show or hide the normal arrow
        if (normalArrow != null)
        {
            normalArrow.SetActive(!isReversed);
        }

        // Show or hide the reversed arrow
        if (reversedArrow != null)
        {
            reversedArrow.SetActive(isReversed);
        }
    }

    // This runs while another collider stays inside the tunnel trigger
    private void OnTriggerStay(Collider other)
    {
        // Try to get a Rigidbody from the object inside the tunnel
        Rigidbody rb = other.GetComponent<Rigidbody>();

        // If the object has a Rigidbody then push it in the tunnel direction
        if (rb != null)
        {
            rb.velocity = currentDirection * pushForce;
        }
    }

}

