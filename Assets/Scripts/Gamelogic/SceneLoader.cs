using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Tetris.GameLogic
{
    /// <summary>
    /// Utility class responsible for loading and activating the presentation scene.
    /// </summary>
    public static class SceneLoader
    {
        /// <summary>
        /// Asynchronously loads the "PresentationScene" additively and sets it as the active scene.
        /// </summary>
        public static async Task LoadPresentationSceneAsync()
        {
            // Starts asynchronous scene loading
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("PresentationScene", LoadSceneMode.Additive);

            if (asyncLoad == null)
            {
                Debug.LogWarning("Failed to load PresentationScene. Scene not found");
                return;
            }

            // Waits until the scene is fully loaded
            while (!asyncLoad.isDone)
            {
                await Task.Yield();
            }

            // Sets the newly loaded scene as active
            SceneManager.SetActiveScene(SceneManager.GetSceneByName("PresentationScene"));
        }
    }
}