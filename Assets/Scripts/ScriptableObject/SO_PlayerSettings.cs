using UnityEngine;

[CreateAssetMenu(fileName = "SO_PlayerSettings", menuName = "Scriptable Objects/SO_PlayerSettings")]
public class SO_PlayerSettings : ScriptableObject
{
    /// <summary>
    /// The horizontal speed at which the Tetromino moves when the player controls it.
    /// </summary>
    [Header("Player Settings")]
    [SerializeField] private float _horizontalSpeed; 
    
    /// <summary>
    /// The vertical speed of the Tetromino controlled by the player.
    /// This value adds to the default falling speed when the player holds the button for downward movement.
    /// </summary>
    [SerializeField] private float _verticalSpeed;
    
    /// <summary>
    /// The default falling speed of the Tetromino when no player input is applied.
    /// </summary>
    [SerializeField] private float _defaultFallingSpeed; 
    
    /// <summary>
    /// The maximum falling speed the Tetromino can reach.
    /// This speed is clamped within the range [3f, 6f] for balance.
    /// </summary>
    [Range(3f, 6f)] [SerializeField] private float _maxFallingSpeed; 

    /// <summary>
    /// The horizontal speed at which the Tetromino moves when the player controls it.
    /// </summary>
    public float HorizontalSpeed => _horizontalSpeed;
    
    /// <summary>
    /// The vertical speed of the Tetromino controlled by the player.
    /// </summary>
    public float VerticalSpeed => _verticalSpeed;
    
    /// <summary>
    /// The default falling speed of the Tetromino.
    /// </summary>
    public float DefaultFallingSpeed => _defaultFallingSpeed;
    
    /// <summary>
    /// The maximum falling speed of the Tetromino.
    /// </summary>
    public float MaxFallingSpeed => _maxFallingSpeed;
}