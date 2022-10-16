// System
using System.Collections;

// Unity Engine
using UnityEngine;

// Otherworld
using Otherworld.Core;

[CreateAssetMenu]
public class CombatBehaviour : InputProvider
{
    // Members
    public CharacterList party;
    
    private CharacterList _enemies;
    private MonoBehaviour _system;
    
    public Transform _transform;
    private Vector3 _moveTarget;
    
    // Initialize Combat Brain
    public void Initialize(Transform transform, CharacterList party, CharacterList enemies, MonoBehaviour system)
    {
        _transform = transform;
        this.party = party;
        _enemies = enemies;

        _system = system;
        
        MoveToClosestCell();
    }

    private void FaceClosestEnemy()
    {
        var distance = float.MaxValue;
        var target = Vector3.zero;
        
        foreach (var combatant in _enemies)
        {
            var newDistance = Vector3.Distance(_transform.position, combatant.transform.position);
            if (!(newDistance < distance)) continue;
            
            distance = newDistance;
            target = combatant.transform.position;
        }
        
        var vector = target - _transform.position;
        MoveEvent.Invoke(new Vector2(vector.x, vector.z).normalized * 0.005f);
    }

    private void MoveToClosestCell()
    {
        var pos = _transform.position;
        var cell = new Vector3(Mathf.RoundToInt(pos.x), 0, Mathf.RoundToInt(pos.z));

        _system.StartCoroutine(MoveToCellCoroutine(cell));
    }
    
    // Functions
    private void MoveToCell(Vector3 cell)
    {
        _system.StartCoroutine(MoveToCellCoroutine(cell));
    }

    private IEnumerator MoveToCellCoroutine(Vector3 cell)
    {
        // Move until you get to the target 
        while (Mathf.Abs(cell.x - _transform.position.z) > .01 && Mathf.Abs(cell.z - _transform.position.z) > .01)
        {
            var vector = cell - _transform.position;
            MoveEvent.Invoke(new Vector2(vector.x, vector.z));
            yield return null;
        }
        
        // Stop moving
        MoveEvent.Invoke(Vector3.zero);
        yield return null;
        
        FaceClosestEnemy();
    }

    public void StartTurn()
    {
        Debug.Log("Start turn");
        SelectEvent.Invoke(true);
    }

    public void EndTurn()
    {
        SelectEvent.Invoke(false);
    }
    
    public bool IsTurnComplete()
    {
        return false;
    }
}
