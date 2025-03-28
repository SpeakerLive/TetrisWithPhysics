using UnityEngine;
using Tetris.GameLogic;
using TMPro;

namespace Tetris.Presentation
{
    /// <summary>
    /// Manages the score display for both players.
    /// Updates scores when lines are cleared.
    /// </summary>
    public class ScoreManager : MonoBehaviour
    {
        // UI reference for Player 1's score text
        [SerializeField] private TextMeshProUGUI _scoreTextP1;

        // UI reference for Player 2's score text
        [SerializeField] private TextMeshProUGUI _scoreTextP2;

        // Internal score tracking
        private int scoreP1 = 0;
        private int scoreP2 = 0;

        /// <summary>
        /// Subscribes to the line cleared event when the object is initialized.
        /// </summary>
        private void Start()
        {
            GameManager.Instance.LineChecker.OnLineCleared += UpdateScore;
        }

        /// <summary>
        /// Unsubscribes from the event to prevent memory leaks.
        /// </summary>
        private void OnDestroy()
        {
            if (GameManager.Instance != null && GameManager.Instance.LineChecker != null)
            {
                GameManager.Instance.LineChecker.OnLineCleared -= UpdateScore;
            }
        }

        /// <summary>
        /// Updates the score when a player clears a line.
        /// </summary>
        /// <param name="isPlayerOne">Indicates which player cleared the line.</param>
        /// <param name="score">Score gained from clearing the line.</param>
        private void UpdateScore(bool isPlayerOne, int score)
        {
            if (isPlayerOne)
            {
                scoreP1 += score;
                _scoreTextP1.text = $"{scoreP1}";
            }
            else
            {
                scoreP2 += score;
                _scoreTextP2.text = $"{scoreP2}";
            }
        }
    }
}