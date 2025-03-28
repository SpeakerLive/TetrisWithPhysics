using UnityEngine;
using System;
using System.Collections.Generic;

namespace Tetris.GameLogic
{
    public class GameManager : MonoBehaviour
{
    // Singleton instance of GameManager
    public static GameManager Instance { get; private set; }
    
    // Reference to the LineChecker for line clearing logic
    public LineChecker LineChecker { get; private set; }

    // Events for when the next tetromino is updated for each player
    public event Action<SO_Tetromino> OnNextTetrominoP1Changed;
    public event Action<SO_Tetromino> OnNextTetrominoP2Changed;
    
    // Event triggered when a tetromino spawn is requested
    public event Action<bool> OnSpawnTetrominoRequested;
    
    // Event triggered when the game ends
    public event Action<string> OnGameOver;

    // Tetromino queues for each player
    private TetrominoQueue _queuePlayerOne;
    private TetrominoQueue _queuePlayerTwo;
    
    // Lists to store blocks placed by each player
    private List<TetrominoBlock> _playerOneBlocks = new List<TetrominoBlock>();
    private List<TetrominoBlock> _playerTwoBlocks = new List<TetrominoBlock>();
    
    [SerializeField] private SO_Tetromino[] tetrominoPrefabs; // Array of available tetromino prefabs
    private int _gameOverHeight = 20; // Height threshold for game over condition
    
    public int GameOverHeight => _gameOverHeight;

    private void Awake()
    {
        // Implement Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Ensure persistence across scenes
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private async void Start()
    {
        // Initialize tetromino queues for each player
        _queuePlayerOne = new TetrominoQueue(tetrominoPrefabs);
        _queuePlayerTwo = new TetrominoQueue(tetrominoPrefabs);

        // Subscribe to tetromino update events
        _queuePlayerOne.OnNextTetrominoChanged += (t) => OnNextTetrominoP1Changed?.Invoke(t);
        _queuePlayerTwo.OnNextTetrominoChanged += (t) => OnNextTetrominoP2Changed?.Invoke(t);
        
        // Load the presentation scene asynchronously
        await SceneLoader.LoadPresentationSceneAsync();
        
        // Request initial tetromino spawn for both players
        RequestSpawnTetromino(true);  
        RequestSpawnTetromino(false);
    }

    /// <summary>
    /// Gets and updates the next tetromino for the specified player.
    /// </summary>
    /// <param name="isPlayerOne">True if player one, false if player two.</param>
    /// <returns>The next tetromino.</returns>
    public SO_Tetromino GetNextTetromino(bool isPlayerOne)
    {
        return isPlayerOne ? _queuePlayerOne.GetAndUpdateNextTetromino() : _queuePlayerTwo.GetAndUpdateNextTetromino();
    }

    /// <summary>
    /// Peeks at the next tetromino without updating the queue.
    /// </summary>
    /// /// <param name="isPlayerOne">Determines whether the tetromino belongs to player 1 (true) or player 2 (false).</param>
    /// <returns>The next tetromino.</returns>
    public SO_Tetromino PeekNextTetromino(bool isPlayerOne)
    {
        return isPlayerOne ? _queuePlayerOne.PeekNextTetromino() : _queuePlayerTwo.PeekNextTetromino();
    }
    
    /// <summary>
    /// Requests spawning of a tetromino for the specified player.
    /// </summary>
    /// /// <param name="isPlayerOne">Determines whether the tetromino belongs to player 1 (true) or player 2 (false).</param>
    public void RequestSpawnTetromino(bool isPlayerOne)
    {
        OnSpawnTetrominoRequested?.Invoke(isPlayerOne);
    }
    
    /// <summary>
    /// Registers the LineChecker instance for line clearing.
    /// </summary>
    /// <param name="checker">The LineChecker instance.</param>
    public void RegisterLineChecker(LineChecker checker)
    {
        LineChecker = checker;
    }
    
    /// <summary>
    /// Registers a placed tetromino block for a specific player.
    /// </summary>
    /// <param name="block">The tetromino block.</param>
    /// <param name="isPlayerOne">Determines whether the tetromino belongs to player 1 (true) or player 2 (false).</param>
    public void RegisterBlock(TetrominoBlock block, bool isPlayerOne)
    {
        if (isPlayerOne)
            _playerOneBlocks.Add(block);
        else
            _playerTwoBlocks.Add(block);
    }

    /// <summary>
    /// Retrieves the list of blocks placed by a specific player.
    /// </summary>
    /// <param name="isPlayerOne">Determines whether the tetromino belongs to player 1 (true) or player 2 (false).</param>
    /// <returns>List of placed tetromino blocks.</returns>
    public List<TetrominoBlock> GetPlayerBlocks(bool isPlayerOne)
    {
        return isPlayerOne ? _playerOneBlocks : _playerTwoBlocks;
    }

    /// <summary>
    /// Triggers the game over event with the winning player's name.
    /// </summary>
    /// <param name="winner">The name of the winning player.</param>
    public void TriggerGameOver(string winner)
    {
        OnGameOver?.Invoke(winner);
    }
}
}