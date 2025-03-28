using UnityEngine;
using Tetris.GameLogic;

namespace Tetris.Presentation
{
    public class TetrominoSpawner : MonoBehaviour
    {
        // Spawn points for player one and player two tetrominos
        [SerializeField] private Transform _spawnPointP1; // The spawn point for player one Tetrominos
        [SerializeField] private Transform _spawnPointP2; // The spawn point for player two Tetrominos
        
        // Input handlers for player one and player two
        [SerializeField] private PlayerInputHandler _inputHandlerPlayerOne; // Input handler for player 1
        [SerializeField] private PlayerInputHandler _inputHandlerPlayerTwo; // Input handler for player 2

        private void Awake()
        {
            // Subscribe the SpawnTetromino method to the OnSpawnTetrominoRequested event in GameManager.
            // This allows the tetromino spawning to be triggered when requested by the game manager.
            GameManager.Instance.OnSpawnTetrominoRequested += SpawnTetromino;
        }

        private void OnDestroy()
        {
            // Unsubscribe the SpawnTetromino method to avoid potential issues with event handling
            // when this object is destroyed (e.g., if the GameManager instance is destroyed).
            if (GameManager.Instance != null)
                GameManager.Instance.OnSpawnTetrominoRequested -= SpawnTetromino;
        }

        /// <summary>
        /// Spawns a new tetromino at the appropriate spawn point for player 1 or player 2.
        /// </summary>
        /// <param name="isPlayerOne">Determines whether the tetromino belongs to player 1 (true) or player 2 (false).</param>
        public void SpawnTetromino(bool isPlayerOne)
        {
            // Selects the appropriate spawn point based on the player
            Transform spawnPoint = isPlayerOne ? _spawnPointP1 : _spawnPointP2;

            // Selects the appropriate input handler based on the player
            PlayerInputHandler inputHandler = isPlayerOne ? _inputHandlerPlayerOne : _inputHandlerPlayerTwo;

            // Retrieves the next tetromino for the corresponding player from the GameManager
            SO_Tetromino tetromino = GameManager.Instance.GetNextTetromino(isPlayerOne);
            
            // Instantiates a copy of the tetromino prefab at the selected spawn point
            GameObject tetrominoClone = Instantiate(tetromino.TetrominoPrefab, spawnPoint.position, Quaternion.identity);
            
            // Initializes the newly spawned tetromino (sets references to the spawner and player)
            tetrominoClone.GetComponent<Tetromino>().Init(this, isPlayerOne);
            
            // Assigns the newly instantiated tetromino's Rigidbody to the corresponding input handler
            // This allows the input handler to manage the tetromino's movements and interactions
            inputHandler.TetrominoRb = tetrominoClone.GetComponent<Rigidbody>();
        }
    }
}
