using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
    [Header("Root UI object to show/hide")]
    public GameObject deathScreenRoot;

    // Call when the enemy catches the player
    public void Show()
    {
        if (deathScreenRoot != null)
            deathScreenRoot.SetActive(true);

        // Unlock cursor when death screen shows
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // We can just hide the screen OR
    public void Hide()
    {
        if (deathScreenRoot != null)
            deathScreenRoot.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // We can restart the level
    public void RestartLevel()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);

        // Relock cursor for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}