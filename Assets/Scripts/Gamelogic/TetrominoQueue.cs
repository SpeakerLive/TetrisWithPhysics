using System;

namespace Tetris.GameLogic
{
     /// <summary>
    /// Handles the tetromino queue logic for a player.
    /// Generates random tetrominoes and notifies when the next tetromino changes.
    /// </summary>
    public class TetrominoQueue
    {
        // Array of available tetromino prefabs.
        private readonly SO_Tetromino[] _tetrominoPrefabs;

        // The next tetromino that will be spawned.
        private SO_Tetromino _nextTetromino;

        /// <summary>
        /// Event triggered when the next tetromino has been changed.
        /// </summary>
        public event Action<SO_Tetromino> OnNextTetrominoChanged;

        /// <summary>
        /// Initializes a new instance of the TetrominoQueue class with a given set of tetromino prefabs.
        /// </summary>
        /// <param name="tetrominoPrefabs">An array of available tetromino prefabs.</param>
        public TetrominoQueue(SO_Tetromino[] tetrominoPrefabs)
        {
            _tetrominoPrefabs = tetrominoPrefabs;
            _nextTetromino = ChooseRandomTetromino();
        }

        /// <summary>
        /// Randomly selects a tetromino from the array of available prefabs.
        /// </summary>
        /// <returns>A randomly selected tetromino.</returns>
        private SO_Tetromino ChooseRandomTetromino()
        {
            return _tetrominoPrefabs[UnityEngine.Random.Range(0, _tetrominoPrefabs.Length)];
        }

        /// <summary>
        /// Gets the current tetromino and updates the queue to the next random tetromino.
        /// Also triggers the OnNextTetrominoChanged event.
        /// </summary>
        /// <returns>The current tetromino to be used.</returns>
        public SO_Tetromino GetAndUpdateNextTetromino()
        {
            SO_Tetromino currentTetromino = _nextTetromino;
            _nextTetromino = ChooseRandomTetromino();
            OnNextTetrominoChanged?.Invoke(_nextTetromino);
            return currentTetromino;
        }

        /// <summary>
        /// Returns the next tetromino without removing it from the queue.
        /// </summary>
        /// <returns>The next tetromino.</returns>
        public SO_Tetromino PeekNextTetromino() => _nextTetromino;
    }
}