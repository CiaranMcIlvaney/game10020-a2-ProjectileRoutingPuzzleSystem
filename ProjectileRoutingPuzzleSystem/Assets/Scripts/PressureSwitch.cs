using UnityEngine;
using UnityEngine.Events;

public class PressureSwitch : MonoBehaviour
{
    // Color used when the switch is not pressed
    public Color offColor;

    // Color used when the switch is pressed
    public Color onColor;

    // Custom event that other scripts can see, it send a bool value true = switch pressed, false = switch released
    public UnityEvent<bool> OnSwitchChanged;

    // Stores the current state of the switch
    bool switchState = false;

    // Reference to the renderer component of the object. Used to change the color of the material
    Renderer objectRenderer;

    void Start()
    {
        // Get the renderer component attached to the object
        objectRenderer = GetComponent<Renderer>();

        // Set up correct staring color based on the switch state
        UpdateColor();
    }

    void UpdateColor()
    {
        if (switchState)
        {
            // If the switch is pressed change it to the on color
            objectRenderer.material.color = onColor;
        }
        else
        {
            // If the switch is not pressed then change it to the off color
            objectRenderer.material.color = offColor;
        }
    }

    // Updates the switch state
    void SetSwitchState(bool newState)
    {
        // If the switch is already in this state then do nothing
        if (switchState == newState)
        {
            return;
        }

        // Update the stored state
        switchState = newState;

        // Update the color so the players see the change
        UpdateColor();

        // Fire the event so other systems know th switch changed
        OnSwitchChanged.Invoke(switchState);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("character"))
        {
            // If the player stepped on the switch then set the switch state to pressed
            SetSwitchState(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Again check if the object leaving is the player
        if (other.CompareTag("character"))
        {
            // If the player stepped off the switch then set the switch state back to not being pressed
            SetSwitchState(false);
        }
    }
}
