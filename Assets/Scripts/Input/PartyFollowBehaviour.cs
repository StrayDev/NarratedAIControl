// System
using System.Collections;

// Unity Engine
using UnityEngine;

// Otherworld
using Otherworld.Core;

[CreateAssetMenu(fileName = "new PartyFollowBehaviour", menuName = "Input Behaviour/Party Follow Behaviour")]
public class PartyFollowBehaviour : InputProvider
{
    [SerializeField] private UpdateChannel updateChannel;
    [SerializeField] private StartChannel startChannel;

    [SerializeField] private CharacterList party;

    public Transform self;
    private Transform target;

    private float idleTrigger = .75f;
    private float walkTrigger = 1.25f;

    private void OnEnable()
    {
        startChannel.OnStartEvent += OnStart;
        updateChannel.OnUpdateEvent += OnUpdate;
    }

    [SerializeField] private int selfId = 0;
    [SerializeField] private int targetId = 0;

    private void OnStart()
    {
        target = party[targetId];
        self = party[selfId];
    }

    private void OnUpdate()
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
            MoveEvent.Invoke(Vector3.zero);
            return;
        }

        // set walk / run speed
        vector *= distance < walkTrigger ? .5f : .8f;

        // send the move information 
        Debug.Log($"{self.name} : {vector}");
        MoveEvent.Invoke(ToVector2(vector));
    }

    private Vector2 ToVector2(Vector3 vector)
    {
        return new Vector2(vector.x, vector.z);
    }
}