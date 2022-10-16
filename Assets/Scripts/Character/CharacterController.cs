using Otherworld.Core;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    [Header("Input")] [SerializeField] private InputProvider defaultInput;

    [Header("References")] 
    [SerializeField] private CharacterAnimation animator;
    [SerializeField] private CharacterMovement movement;
    [SerializeField] private CharacterEffects effects;

    private InputProvider Input;

    private Vector3 _inputVector;

    private void OnEnable()
    {
        if (defaultInput == null) return;

        // set default behaviour
        Input = defaultInput;

        // set up additional behaviours
        Input.OnMoveEvent += OnMoveInput;
    }

    private void OnDisable()
    {
        if (Input == null) return;
        Input.OnMoveEvent -= OnMoveInput;
        Input.OnSelectEvent -= effects.OnSelected;
    }

    public void OnMoveInput(Vector2 vector)
    {
        _inputVector = new Vector3(vector.x, 0, vector.y);

        animator.SetAnimationState(_inputVector);
        movement.SetMovementState(_inputVector);
    }

    public void SetInputBehaviour(InputProvider input)
    {
        defaultInput = input; // todo remove

        if (Input != null)
        {
            Input.OnMoveEvent -= OnMoveInput;
            Input.OnSelectEvent += effects.OnSelected;
        }

        Input = input;
        
        if (input != null)
        {
            Input.OnMoveEvent += OnMoveInput;
            Input.OnSelectEvent += effects.OnSelected;
        }
    }

    public void SetDefaultBehaviour()
    {
    }
}