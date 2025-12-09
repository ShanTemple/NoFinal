using UnityEngine;
using UnityEngine.SceneManagement;

public class GameUI : MonoBehaviour
{
    // Called by the Play Again button
    public void PlayAgain()
    {
        // Unpause time in case it was stopped
        Time.timeScale = 1f;

        // Reload the current active scene
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    // Optional quit button we can use later
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Quit Game pressed (this only closes builds, not the editor).");
    }
}
