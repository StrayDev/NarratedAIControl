using System.Collections.Generic;
using UnityEngine;

public class PartySystem : MonoBehaviour
{
    [SerializeField] private List<Transform> transforms;

    [SerializeField] private Transform currentLeader;

    [SerializeField] private List<PartyFollowBehaviour> _partyBehaviours;

    private void Start()
    {
        // skip 0
        for (var i = 1; i < transforms.Count; i++)
        {
            var cc = transforms[i].GetComponent<CharacterController>();
            var input = ScriptableObject.CreateInstance<PartyFollowBehaviour>();

            //input.target = transforms[i - 1];
            //input.self = transforms[i];
            
            cc.SetInputBehaviour(input);
            _partyBehaviours.Add(input);
        }
    }

    private int count = 0;
    
    private void Update()
    {
        count++;
        
        if (count < 15) return;
        count = 0;
        
        foreach (var behaviour in _partyBehaviours)
        {
            //behaviour.Update();
        }
    }
}
