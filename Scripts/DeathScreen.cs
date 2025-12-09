using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [Header("Root UI object to show/hide")]
    public GameObject deathScreenRoot;

    // Call this when the enemy catches the player
    public void Show()
    {
        if (deathScreenRoot != null)
            deathScreenRoot.SetActive(true);

        // Unlock cursor when death screen shows (optional but recommended)
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // Option 1: Just hide the screen (not used for restart flow but kept in case)
    public void Hide()
    {
        if (deathScreenRoot != null)
            deathScreenRoot.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Option 2: Restart the level
    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        // Re-lock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}