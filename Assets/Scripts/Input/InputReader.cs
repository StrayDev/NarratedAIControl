using UnityEngine;
using UnityEngine.InputSystem;

namespace Otherworld.Core
{
    /// <summary>
    /// Reads input from the GameInput asset
    /// </summary>
    
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : InputProvider, GameInput.IMovementActions
    {
        private GameInput _gameInput;

        private void OnEnable()
        {
            if (_gameInput != null) return;
            
            _gameInput = new GameInput();
            _gameInput.Movement.SetCallbacks(this);
            _gameInput.Movement.Enable(); // TODO : remove from here
        }
        
        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }
        
    }
}