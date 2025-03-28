using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Tetris.GameLogic
{
    public class LineChecker : MonoBehaviour
    {
        /// <summary>
        /// Event triggered when a line is cleared.
        /// Parameters: 
        /// bool = isPlayerOne (true if Player 1 cleared the line),
        /// int = points for clearing the line.
        /// </summary>
        public event Action<bool, int> OnLineCleared;
        
        [SerializeField] private int lineWidth = 10;     // Number of blocks required to complete a line
        [SerializeField] private float blockSize = 1.0f; // Size of a single block in world units
        
        // Stores the last known Y positions of all blocks to detect downward movement
        private Dictionary<TetrominoBlock, float> _lastBlockPositions = new Dictionary<TetrominoBlock, float>();

        private void Start()
        {
            // Register this LineChecker in the GameManager
            GameManager.Instance.RegisterLineChecker(this);
        }

        private void Update()
        {
            bool hasBlockMovedDown = false;

            // Check if any Player 1 block moved down
            foreach (var block in GameManager.Instance.GetPlayerBlocks(true)) 
            {
                if (CheckBlockMovedDown(block)) hasBlockMovedDown = true;
            }

            // Check if any Player 2 block moved down
            foreach (var block in GameManager.Instance.GetPlayerBlocks(false)) 
            {
                if (CheckBlockMovedDown(block)) hasBlockMovedDown = true;
            }

            // If any block moved down, check for complete lines for both players
            if (hasBlockMovedDown)
            {
                CheckForCompleteLines(true);
                CheckForCompleteLines(false);
            }
        }

        /// <summary>
        /// Checks if a block has moved down compared to its last recorded position.
        /// </summary>
        /// <param name="block">Block to check.</param>
        /// <returns>True if block moved down, otherwise false.</returns>
        private bool CheckBlockMovedDown(TetrominoBlock block)
        {
            float currentY = Mathf.Round(block.transform.position.y / blockSize) * blockSize;

            if (_lastBlockPositions.TryGetValue(block, out float lastY))
            {
                if (currentY < lastY) // Block moved down
                {
                    _lastBlockPositions[block] = currentY;
                    return true;
                }
            }
            else // First time tracking this block
            {
                _lastBlockPositions[block] = currentY;
            }
            return false;
        }

        /// <summary>
        /// Checks for complete lines for the specified player.
        /// </summary>
        /// <param name="isPlayerOne">True if checking lines for Player 1, false for Player 2.</param>
        public void CheckForCompleteLines(bool isPlayerOne)
        {
            List<TetrominoBlock> allBlocks = GameManager.Instance.GetPlayerBlocks(isPlayerOne);
            Dictionary<float, List<TetrominoBlock>> blocksByRow = new Dictionary<float, List<TetrominoBlock>>();

            // Group blocks by their Y position (rows)
            foreach (var block in allBlocks)
            {
                float yPos = Mathf.Round(block.transform.position.y / blockSize) * blockSize;

                if (!blocksByRow.ContainsKey(yPos))
                    blocksByRow[yPos] = new List<TetrominoBlock>();
                blocksByRow[yPos].Add(block);
            }

            // Check each row if it is complete
            foreach (var row in blocksByRow)
            {
                if (row.Value.Count >= lineWidth)
                {
                    RemoveLine(row.Key, row.Value);
                    OnLineCleared?.Invoke(isPlayerOne, 100); // Trigger score update
                }
            }
        }

        /// <summary>
        /// Removes a complete line and shifts down blocks above it.
        /// </summary>
        /// <param name="yPos">The Y position of the cleared line.</param>
        /// <param name="blocks">The blocks that form the cleared line.</param>
        private void RemoveLine(float yPos, List<TetrominoBlock> blocks)
        {
            // Detect which player's blocks are affected
            bool isPlayerOne = GameManager.Instance.GetPlayerBlocks(true).Contains(blocks[0]); 

            // Destroy the blocks from the completed line
            foreach (var block in blocks)
            {
                GameManager.Instance.GetPlayerBlocks(isPlayerOne).Remove(block);
                Destroy(block.gameObject);
            }

            // Move down all blocks that are above the cleared line
            List<TetrominoBlock> allBlocks = GameManager.Instance.GetPlayerBlocks(isPlayerOne);
            foreach (var block in allBlocks)
            {
                if (block.transform.position.y > yPos)
                {
                    block.transform.position += Vector3.down * blockSize;
                }
            }

            // Check again for possible cascaded line clears
            CheckForCompleteLines(isPlayerOne);
        }
    }
}
