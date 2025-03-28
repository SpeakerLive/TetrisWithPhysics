using UnityEngine;
using System.Collections.Generic;

namespace Tetris.GameLogic
{
    /// <summary>
    /// Represents a single block that makes up a Tetromino.
    /// Keeps track of all active blocks and stores player ownership information.
    /// </summary>
    public class TetrominoBlock : MonoBehaviour
    {
        // Static list to keep track of all existing blocks in the scene.
        private static List<TetrominoBlock> _allBlocks = new List<TetrominoBlock>();

        // Determines if this block belongs to player one.
        private bool _isPlayerOne;

        /// <summary>
        /// Adds this block to the static list when enabled.
        /// </summary>
        private void OnEnable()
        {
            _allBlocks.Add(this);
        }

        /// <summary>
        /// Removes this block from the static list when disabled.
        /// </summary>
        private void OnDisable()
        {
            _allBlocks.Remove(this);
        }

        /// <summary>
        /// Assigns the player ownership of this block and registers it to the GameManager.
        /// </summary>
        /// <param name="isPlayerOne">True if the block belongs to player one, false otherwise.</param>
        public void SetPlayerOne(bool isPlayerOne)
        {
            _isPlayerOne = isPlayerOne;
            GameManager.Instance.RegisterBlock(this, _isPlayerOne); 
        }
    }
}