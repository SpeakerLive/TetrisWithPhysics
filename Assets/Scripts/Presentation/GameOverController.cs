using Tetris.GameLogic;
using TMPro;
using UnityEngine;

namespace Tetris.Presentation
{/// <summary>
    /// Activates GameOver Canvas and sets correct player text
    /// </summary>
    public class GameOverController : MonoBehaviour
    {
        [SerializeField] private GameObject gameOverPanel;
        [SerializeField] private TextMeshProUGUI gameOverText;

        private void Start()
        {
            gameOverPanel.SetActive(false);
            
            GameManager.Instance.OnGameOver += ShowGameOver;
        }

        private void OnDestroy()
        {
            
            GameManager.Instance.OnGameOver -= ShowGameOver;
        }

        /// <summary>
        /// Activates gameOverPanel and set text to the winner name.
        /// </summary>
        /// <param name="winner">Winner name.</param>
        private void ShowGameOver(string winner)
        {
            gameOverPanel.SetActive(true);
            gameOverText.text = $"{winner} Wins!";
        }
    }
}