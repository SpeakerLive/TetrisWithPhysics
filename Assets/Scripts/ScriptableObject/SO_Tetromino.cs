using UnityEngine;

[CreateAssetMenu(fileName = "SO_Tetromino", menuName = "Scriptable Objects/SO_Tetromino")]
public class SO_Tetromino : ScriptableObject
{
    /// <summary>
    /// Prefab of the Tetromino that will be spawned in the game scene.
    /// </summary>
    [Header("TetrominoSettings")]
    [SerializeField] private GameObject _tetrominoPrefab; // The actual prefab of the Tetromino object
    
    /// <summary>
    /// Texture of the Tetromino image to be displayed in the UI (next Tetromino preview).
    /// </summary>
    [SerializeField] private Texture _tetrominoImage; // The image representing the Tetromino (for UI)

    /// <summary>
    /// The Tetromino prefab associated with this Scriptable Object.
    /// Used for spawning the Tetromino in the game.
    /// </summary>
    public GameObject TetrominoPrefab => _tetrominoPrefab;

    /// <summary>
    /// The image of the Tetromino used to preview the next Tetromino in the UI.
    /// </summary>
    public Texture TetrominoImage => _tetrominoImage;
}