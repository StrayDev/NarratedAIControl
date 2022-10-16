using System.Collections;
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
        Input.OnMoveToPositionEvent += MoveToPosition;
        Input.OnNavigateWaypointsEvent += OnNavigateWaypointsEvent;
    }

    private void OnDisable()
    {
        if (Input == null) return;
        Input.OnMoveEvent -= OnMoveInput;
        Input.OnMoveToPositionEvent -= MoveToPosition;
        Input.OnNavigateWaypointsEvent -= OnNavigateWaypointsEvent;
    }

    private void OnNavigateWaypointsEvent(Vector3[] waypoints)
    {
        StartCoroutine(NavigateWaypointsCoroutine(waypoints));
    }

    private IEnumerator NavigateWaypointsCoroutine(Vector3[] waypoints)
    {
        foreach(var w in waypoints)
        {
            var character = transform;
            var loop = true;
            while (loop)
            {
                var distance = Vector3.Distance(character.position, w);
                var vector = (w - character.position).normalized;
                
                // send the move information 
                OnMoveInput(new Vector2(vector.x, vector.z));
            
                // break if distance 
                if (distance < .5) loop = false;
                
                yield return null;
            }        
        }
        
        OnMoveInput(Vector2.zero);
    }

    private void OnMoveInput(Vector2 vector)
    {
        _inputVector = new Vector3(vector.x, 0, vector.y);

        animator.SetAnimationState(_inputVector);
        movement.SetMovementState(_inputVector);
    }

    private void MoveToPosition(Vector3 pos)
    {
        StartCoroutine(MoveToPositionCoroutine(pos));
    }

    private IEnumerator MoveToPositionCoroutine(Vector3 target)
    {
        var character = transform;

        while (true)
        {
            var distance = Vector3.Distance(character.position, target);
            var vector = (target - character.position).normalized;
            
            // set walk / run speed
            vector *= distance < 1.25 ? .5f : .8f;

            // send the move information 
            OnMoveInput(new Vector2(vector.x, vector.z));
            
            // break if distance 
            if (distance < .01) break;
            
            yield return null;
        }
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