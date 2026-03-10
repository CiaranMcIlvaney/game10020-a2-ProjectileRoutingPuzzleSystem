using UnityEngine;
using UnityEngine.SceneManagement;

public class FallReset : MonoBehaviour
{
    // This function runs when another collider enters this trigger
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object that entered the trigger is the player
        if (other.CompareTag("character"))
        {
            // Reload the level so the player can try the puzzle again
            SceneManager.LoadScene("Level1");
        }
    }
}
