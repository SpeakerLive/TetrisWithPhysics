using UnityEngine;
using UnityEngine.InputSystem;
using Tetris.GameLogic;

namespace Tetris.Presentation
{
    public class PlayerInputHandler : MonoBehaviour
    {
        const string PlayerOneTag = "PlayerOne"; // Tag for Player 1 object
        const string KeyboardOneScheme = "Keyboard1"; // Control scheme for Player 1
        const string KeyboardTwoScheme = "Keyboard2"; // Control scheme for Player 2
        
        [SerializeField] private PlayerInput _playerInput; // Handles player input actions
        [SerializeField] private SO_PlayerSettings playerSettings; // Stores player movement settings
        [SerializeField] private Rigidbody _tetrominoRb; // Rigidbody of the controlled tetromino
        
        private float _horizontalSpeed; // Speed for horizontal movement
        private float _verticalSpeed; // Speed for vertical movement
        private float _defaultFallingSpeed; // Default falling speed of the tetromino
        private Vector2 _moveInput; // Stores movement input values

        public Rigidbody TetrominoRb
        {
            set => _tetrominoRb = value; // Setter for the tetromino Rigidbody
        }
        
        private void Start()
        {
            // Determines the control scheme based on the object's tag (Player 1 or Player 2)
            if (gameObject.transform.CompareTag(PlayerOneTag))
            {
                _playerInput.SwitchCurrentControlScheme(KeyboardOneScheme, Keyboard.current); // Switches to Player 1's control scheme
            }
            else
            {
                _playerInput.SwitchCurrentControlScheme(KeyboardTwoScheme, Keyboard.current); // Switches to Player 2's control scheme
            }

            // Retrieves movement settings from the PlayerSettings ScriptableObject
            _horizontalSpeed = playerSettings.HorizontalSpeed;
            _verticalSpeed = playerSettings.VerticalSpeed;
            _defaultFallingSpeed = playerSettings.DefaultFallingSpeed;
        }
        
        private void Update()
        {
            // If no tetromino Rigidbody is assigned, exit early
            if (!_tetrominoRb) return;

            // Calculate fall speed based on player input (vertical movement) and settings
            float fallSpeed = -_defaultFallingSpeed + (_moveInput.y * _verticalSpeed);
            fallSpeed = Mathf.Max(fallSpeed, -playerSettings.MaxFallingSpeed); // Ensure the falling speed doesn't exceed max limit

            // Apply movement velocity to the tetromino
            _tetrominoRb.linearVelocity = new Vector2(_moveInput.x * _horizontalSpeed, fallSpeed);
        }

        private void OnEnable()
        {
            // Subscribe to input actions to handle player inputs
            _playerInput.actions["Move"].performed += OnMove;
            _playerInput.actions["Move"].canceled += OnMove;
            _playerInput.actions["Rotate"].performed += OnRotate;
        }

        private void OnDisable()
        {
            // Unsubscribe from input actions to prevent memory leaks when the object is disabled or destroyed
            _playerInput.actions["Move"].performed -= OnMove;
            _playerInput.actions["Move"].canceled -= OnMove;
            _playerInput.actions["Rotate"].performed -= OnRotate;
        }

        private void OnMove(InputAction.CallbackContext context)
        {
            // Reads the player's movement input values (left/right or up/down)
            _moveInput = context.ReadValue<Vector2>();
        }

        private void OnRotate(InputAction.CallbackContext context)
        {
            // Calls the rotation handler when rotation input is received
            HandleRotate();
        }

        private void HandleRotate()
        {
            // Rotates the tetromino by 90 degrees (clockwise)
            Quaternion newRotation = _tetrominoRb.rotation * Quaternion.Euler(0, 0, 90);
            _tetrominoRb.MoveRotation(newRotation);
        }
    }
}
