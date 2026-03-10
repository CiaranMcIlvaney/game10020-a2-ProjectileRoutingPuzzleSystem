using UnityEngine;

public class Bridge : MonoBehaviour
{
    // How far the bridge should move upward when it opens
    public Vector3 upOffset;

    // How fast the bridge moves
    public float speed;

    // The position where the bridge starts in the scene
    Vector3 startPosition;

    // The position the bridge is trying to move towards
    Vector3 targetPosition;

    void Start()
    {
        // Save the starting position of the bridge when the scene begins
        startPosition = transform.position;

        // At the start the target position is the same as the starting position so the bridge stays closed
        targetPosition = startPosition;
    }

    void Update()
    {
        // Every frame the bridge moves toward the target position
        transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
    }
    
    // This function tells the bridge whether to move up or down
    public void Move(bool down)
    {
        if (down)
        {
            // If down is true then the bridge moves back to its starting position
            targetPosition = startPosition;
        }
        else
        {
            // If down is false then the bridge moves upward using the offset value
            targetPosition = startPosition + upOffset;
        }
    }
}
