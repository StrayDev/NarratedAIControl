
using System;
using Otherworld.Core;
using UnityEngine;
using UnityEngine.Experimental.Video;
using UnityEngine.InputSystem;
using UnityEngine.Windows.Speech;

[CreateAssetMenu(fileName = "new PartyFollowBehaviour", menuName = "Input Behaviour/Party Follow Behaviour")]
public class PartyFollowBehaviour : InputProvider
{
    public Transform owner;
    public Transform target;
    
    [SerializeField] private float idleTrigger = .75f;
    [SerializeField] private float walkTrigger = 1.25f;

    private Vector3 _vector;
    
    public void Update()
    {
        if (target == null) return;

        // stash positions
        var p1 = owner.position; 
        var p2 = target.position; 
        
        // get the direction to the target 
        var _vector = (p2 - p1).normalized;
        
        // get the move speed
        var distance = Vector3.Distance(p1, p2); 
        
        // stop moving if close enough
        if ( distance < idleTrigger)
        {
            MoveEvent.Invoke(Vector3.zero);
            return;
        }

        // set walk / run speed
        _vector *= distance < walkTrigger ? .5f : .8f;
        
        // send the move information 
        MoveEvent.Invoke(ToVector2(_vector));
    }

    private Vector2 ToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }

}
