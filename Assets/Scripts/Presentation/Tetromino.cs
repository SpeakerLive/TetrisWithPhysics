using Tetris.GameLogic;
using UnityEngine;

namespace Tetris.Presentation
{
    /// <summary>
    /// Controls the behavior of a falling Tetromino.
    /// Handles landing, line checking, spawner notification, and game over detection.
    /// </summary>
    public class Tetromino : MonoBehaviour
    {
        // Indicates whether the tetromino has already landed
        private bool _hasLanded = false;

        // True if the tetromino belongs to Player One
        private bool _playerOne = false;

        // Reference to the TetrominoSpawner that spawned this tetromino
        private TetrominoSpawner _spawner;

        /// <summary>
        /// Initializes the tetromino with a reference to its spawner and player ownership.
        /// </summary>
        /// <param name="spawnerRef">Reference to the spawner.</param>
        /// <param name="playerOne">True if it is Player One's tetromino.</param>
        public void Init(TetrominoSpawner spawnerRef, bool playerOne)
        {
            _spawner = spawnerRef;
            _playerOne = playerOne;

            // Assigns the player ownership to each block in the tetromino
            foreach (Transform child in transform)
            {
                TetrominoBlock block = child.GetComponent<TetrominoBlock>();
                if (block != null)
                {
                    block.SetPlayerOne(playerOne);
                }
            }
        }

        /// <summary>
        /// Called when the tetromino collides with another object.
        /// Checks if it has landed and triggers line checks and game over detection.
        /// </summary>
        /// <param name="collision">The collision information.</param>
        private void OnCollisionEnter(Collision collision)
        {
            if (!_hasLanded && (collision.gameObject.CompareTag("Ground") || collision.gameObject.CompareTag("Tetromino")))
            {
                ContactPoint contact = collision.contacts[0];

                // Checks if the tetromino landed from the top side
                bool landedFromBelow = contact.normal.y > 0.5f;

                // Ensures it lands either by contact normal or by almost stopping
                bool shouldLand = landedFromBelow || GetComponent<Rigidbody>().linearVelocity.y < 0.01f;

                if (shouldLand)
                {
                    _hasLanded = true;
                    GameManager.Instance.LineChecker.CheckForCompleteLines(_playerOne);
                    CheckForGameOver();
                    Invoke(nameof(NotifySpawner), 0.2f);
                }
            }
        }

        /// <summary>
        /// Notifies the spawner to spawn the next tetromino after a short delay.
        /// </summary>
        private void NotifySpawner()
        {
            _spawner.SpawnTetromino(_playerOne);
            Destroy(this);
        }

        /// <summary>
        /// Checks if any blocks exceed the game over height and triggers game over if necessary.
        /// </summary>
        private void CheckForGameOver()
        {
            var playerBlocks = _playerOne ? GameManager.Instance.GetPlayerBlocks(true) : GameManager.Instance.GetPlayerBlocks(false);
            foreach (var block in playerBlocks)
            {
                if (block.transform.position.y >= GameManager.Instance.GameOverHeight)
                {
                    GameManager.Instance.TriggerGameOver(!_playerOne ? "Player 1" : "Player 2");
                    Time.timeScale = 0;
                    return;
                }
            }
        }
    }
}
