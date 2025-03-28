using UnityEngine;
using UnityEngine.UI;
using Tetris.GameLogic;

namespace Tetris.Presentation
{
    /// <summary>
    /// Displays the next tetromino preview for both Player One and Player Two.
    /// Updates the UI whenever a new tetromino is queued.
    /// </summary>
    public class NextTetrominoUI : MonoBehaviour
    {
        // UI element showing the next tetromino for Player One
        [SerializeField] private RawImage _playerOneNextTetromino;

        // UI element showing the next tetromino for Player Two
        [SerializeField] private RawImage _playerTwoNextTetromino;

        /// <summary>
        /// Subscribes to tetromino queue update events and initializes the UI with current next tetrominoes.
        /// </summary>
        private void OnEnable()
        {
            GameManager.Instance.OnNextTetrominoP1Changed += UpdatePlayerOneUI;
            GameManager.Instance.OnNextTetrominoP2Changed += UpdatePlayerTwoUI;

            // Initialize UI with currently queued tetrominoes
            UpdatePlayerOneUI(GameManager.Instance.PeekNextTetromino(true));
            UpdatePlayerTwoUI(GameManager.Instance.PeekNextTetromino(false));
        }

        /// <summary>
        /// Unsubscribes from tetromino queue update events when disabled.
        /// </summary>
        private void OnDisable()
        {
            GameManager.Instance.OnNextTetrominoP1Changed -= UpdatePlayerOneUI;
            GameManager.Instance.OnNextTetrominoP2Changed -= UpdatePlayerTwoUI;
        }

        /// <summary>
        /// Updates the next tetromino preview for Player One.
        /// </summary>
        /// <param name="nextTetromino">The new next tetromino for Player One.</param>
        private void UpdatePlayerOneUI(SO_Tetromino nextTetromino)
        {
            _playerOneNextTetromino.texture = nextTetromino.TetrominoImage;
        }

        /// <summary>
        /// Updates the next tetromino preview for Player Two.
        /// </summary>
        /// <param name="nextTetromino">The new next tetromino for Player Two.</param>
        private void UpdatePlayerTwoUI(SO_Tetromino nextTetromino)
        {
            _playerTwoNextTetromino.texture = nextTetromino.TetrominoImage;
        }
    }
}
