
using System;
using UnityEngine;

public class FollowTest : MonoBehaviour
{
    [SerializeField] private Transform self;
    [SerializeField] private Transform target;
    
    private float idleTrigger = .75f;
    private float walkTrigger = 1.25f;

    private CharacterController controller;
    
    private void Start()
    {
        TryGetComponent(out controller);
    }

    private void Update()
    {
        if (target == null) return;
        
        // stash positions
        var p1 = self.position;
        var p2 = target.position;

        // get the direction to the target 
        var vector = (p2 - p1).normalized;

        // get the move speed
        var distance = Vector3.Distance(p1, p2);

        // stop moving if close enough
        if (distance < idleTrigger)
        {
            //MoveEvent.Invoke(Vector3.zero);
            controller.OnMoveInput(Vector2.zero);
            self.position += Vector3.zero;
            return;
        }

        // set walk / run speed
        vector *= distance < walkTrigger ? .5f : .8f;

        // send the move information 
        //MoveEvent.Invoke(ToVector2(vector));
        controller.OnMoveInput(ToVector2(vector));
    }
    
    private Vector2 ToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}
