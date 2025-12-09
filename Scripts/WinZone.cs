using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [Header("Assign the Win Panel from Canvas here")]
    public GameObject winPanel;

    private void OnTriggerEnter(Collider other)
    {
        // Check if it's the player entering
        if (other.CompareTag("Player"))
        {
            // Show the win panel
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            // Pause the game so player can read and click
            Time.timeScale = 0f;

            // Unlock & show mouse for clicking button (useful in FPS games)
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("You escaped the Semester with all A's. You win!");
        }
    }
}
