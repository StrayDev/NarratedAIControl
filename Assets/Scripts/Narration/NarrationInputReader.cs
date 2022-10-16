// Unity
using System;
using UnityEngine;

// Otherworld
using Otherworld.Core;
using Otherworld.Command;

namespace Otherworld.Narration
{
    /// <summary>
    /// Provides input from the narration system
    /// </summary>
    
    [CreateAssetMenu(menuName = "Input/Narration Input Reader")]
    public class NarrationInputReader : InputProvider
    {
        private void OnEnable()
        {
            // set up commands
            LeaveTheForestCommand.Callback += MoveToPosition;
            MoveToPositionCommand.Callback += MoveToPosition;
            
            TravelPathCommand.Callback += NavigateWaypoints;
        }

        private void NavigateWaypoints(Vector3[] obj)
        {
            NavigateWaypointsEvent.Invoke(obj);
        }

        private void MoveToPosition(Vector3 obj)
        {
            Debug.Log("In the controller");
            MoveToPositionEvent.Invoke(obj);
        }
    }

    public class MoveToPositionCommand : ICommand
    {
        private Vector3 vector;

        public MoveToPositionCommand(Vector3 vector)
        {
            this.vector = vector;
        }

        internal static Action<Vector3> Callback; 
        
        public void Execute()
        {
            Callback.Invoke(vector);
        }

        public void Undo()
        {
            
        }
    }

    public class TravelPathCommand : ICommand
    {
        internal static Action<Vector3[]> Callback; 
        
        public void Execute()
        {
            Callback.Invoke(Waypoint.All.ToArray());
        }

        public void Undo()
        {
            
        }
    }

    // todo remove
    public class LeaveTheForestCommand : ICommand
    {
        private Vector3 vector;

        internal static Action<Vector3> Callback;

        public LeaveTheForestCommand()
        {
            vector = new Vector3(4, 0, 6.5f);
        }
        
        public void Execute()
        {
            Debug.Log("EXECUTE");
            Callback.Invoke(vector);
        }

        public void Undo()
        {
            throw new NotImplementedException();
        }
    }
}