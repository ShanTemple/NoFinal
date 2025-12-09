using UnityEngine;
using UnityEngine.SceneManagement;

public class WinZone : MonoBehaviour
{
    [Header("Assign the Win Panel from Canvas here")]
    public GameObject winPanel;

    private void OnTriggerEnter(Collider other)
    {
        // Check if player is entering
        if (other.CompareTag("Player"))
        {
            // Show win panel
            if (winPanel != null)
            {
                winPanel.SetActive(true);
            }

            // Pause the game so player can read and click
            Time.timeScale = 0f;

            // Unlock & show mouse for clicking button
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Debug.Log("You escaped the Semester with all A's. You win!");
        }
    }
}
