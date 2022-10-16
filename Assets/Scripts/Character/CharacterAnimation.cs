using System;
using UnityEngine;

public enum Movement
{
    Idle,
    Walk,
    Run,
}

public enum Direction
{
    Left,
    Right,
    Up,
    Down,
}

public class CharacterAnimation : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    
    private Movement _movement = Movement.Idle;
    private Direction _direction = Direction.Right;
    
    private const float VerticalDeadzone = .75f;
    
    public void SetAnimationState(Vector3 inputVector)
    {
        var om = _movement;
        var od = _direction;

        _movement  = GetMovement(inputVector);
        _direction = GetDirection(inputVector);

        var newAnim = om != _movement;
        var newDir  = od != _direction;
        
        // add override 
        
        if(newAnim || newDir) SetAnimation(_direction, _movement);
    }
    
    private Direction GetDirection(Vector3 inputVector)
    {
        return inputVector.normalized.z switch
        {
            >  VerticalDeadzone => Direction.Up,
            < -VerticalDeadzone => Direction.Down,
            _ => inputVector.x switch
            {
                > 0 => Direction.Right,
                < 0 => Direction.Left,
                _ => _direction,
            }
        };
    }

    private Movement GetMovement(Vector3 inputVector)
    {
        return inputVector.magnitude switch
        {
            > .75f  => Movement.Run,
            > .01f => Movement.Walk,
            _      => Movement.Idle
        };
    }
    
    private void SetAnimation(Direction direction, Movement movement)
    {
        spriteRenderer.flipX = false;
        
        switch (movement)
        {
            case Movement.Idle:
                switch (direction)
                {
                    case Direction.Left:
                        animator.Play("Character_IdleLeft");
                        break;
                    case Direction.Right:
                        animator.Play("Character_IdleRight");
                        spriteRenderer.flipX = true;
                        break;
                    case Direction.Up:
                        animator.Play("Character_Idle_Up");
                        break;
                    case Direction.Down:
                        animator.Play("Character_Idle_Down");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }

                break;
            
            case Movement.Walk:
                switch (direction)
                {
                    case Direction.Left:
                        animator.Play("Character_WalkLeft");
                        break;
                    case Direction.Right:
                        animator.Play("Character_WalkRight");
                        spriteRenderer.flipX = true;
                        break;
                    case Direction.Up:
                        animator.Play("Character_Walk_Up");
                        break;
                    case Direction.Down:
                        animator.Play("Character_Walk_Down");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }

                break;
            
            case Movement.Run:
                switch (direction)
                {
                    case Direction.Left:
                        animator.Play("Character_Run_Left");
                        break;
                    case Direction.Right:
                        animator.Play("Character_Run_Right");
                        spriteRenderer.flipX = true;
                        break;
                    case Direction.Up:
                        animator.Play("Character_Run_Up");
                        break;
                    case Direction.Down:
                        animator.Play("Character_Run_Down");
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }

                break;
        }
    }
    
}
