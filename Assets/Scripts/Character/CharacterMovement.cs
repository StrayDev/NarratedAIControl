using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [Header("Variables")] 
    [SerializeField] private float walkSpeed = 1;
    [SerializeField] private float runSpeed = 3;

    private Vector3 _vector = Vector3.zero;
    private float _speed = 0;
    
    public void SetMovementState(Vector3 inputVector)
    {
        _vector = inputVector.normalized;
        
        _speed = inputVector.magnitude switch
        {
            > .75f => runSpeed,
            > .01f => walkSpeed,
            _      => 0,
        };
    }

    private void Update()
    {
        transform.position += _vector * (_speed * Time.deltaTime);
    }
}
