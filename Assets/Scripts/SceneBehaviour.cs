using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles scene-related behaviors such as restarting scenes, loading specific scenes, and quitting the application.
/// </summary>
public class SceneBehaviour : MonoBehaviour
{
    /// <summary>
    /// Restarts the current scene.
    /// </summary>
    public void RestartScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex);
    }

    /// <summary>
    /// Loads the specified scene by name.
    /// </summary>
    /// <param name="sceneName">Name of the scene to load.</param>
    public void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }

    private void Update()
    {
        // Quit the application when the escape key is pressed.
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
