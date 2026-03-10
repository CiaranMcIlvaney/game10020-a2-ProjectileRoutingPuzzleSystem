using UnityEngine;

public class Rock : MonoBehaviour
{
    // Reference to the trigger collider used for detecting hits
    Collider triggerCollider;

    // Rigidbody used for physics movement
    Rigidbody weaponRigidbody;

    private void Awake()
    {
        // Find the trigger collider attatched to the rock
        Collider[] colliderComponents = GetComponents<Collider>();
        foreach (Collider collider in colliderComponents)
        {
            if (collider.isTrigger)
            {
                triggerCollider = collider;
                break;
            }
        }

        // Get the Rigidbody component
        weaponRigidbody = GetComponent<Rigidbody>();

        // Start as kinematic so the rock stays attached to the player
        weaponRigidbody.isKinematic = true;
    }

    private void Start()
    {
        // Disable all hitboxes at the start so the rock cannot trigger hits while it is still in the players hand
        EnableHitbox(0);
    }

    // Called when the player throws the rock
    public void Throw(Vector3 direction, float force)
    {
        // Enable the hitbox so the rock can hit switches or doors
        EnableHitbox(1);

        // Detach the rock from the players hand
        transform.parent = null;

        // Adjust the height slightly so the rock launches cleanly
        Vector3 position = transform.position;
        transform.position = new Vector3(position.x, 1.5f, position.z);

        // Enable physics so the rock can move freely
        weaponRigidbody.isKinematic = false;

        // Reset any existing movement
        weaponRigidbody.velocity = Vector3.zero;
        weaponRigidbody.angularVelocity = Vector3.zero;

        // Apply velocity to launch the rock forward
        weaponRigidbody.velocity = direction.normalized * force;
    }

    // Enables or disables the trigger collider used for hitting objects
    public void EnableHitbox(int value)
    {
        triggerCollider.enabled = value == 1 ? true : false;
    }

    // Called when the rocks trigger collider touches another object
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object implements the IHittable interface
        if (other.GetComponent<IHittable>() != null)
        {
            // Call the hit function on that object
            IHittable toggle = other.GetComponent<IHittable>();
            toggle.Hit(gameObject);
        }
    }

    // Destroy the rock when it collides with something solid
    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}
