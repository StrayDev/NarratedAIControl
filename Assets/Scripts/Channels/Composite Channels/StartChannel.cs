// System
using System;

// Unity Engine
using UnityEngine;

// Otherworld
namespace Otherworld.Events
{
    /// <summary>
    /// This class is for events related to the MonoBehaviour update loop
    /// </summary>
    
    [CreateAssetMenu(menuName = "Composite Channels/Start")]
    public class StartChannel : ScriptableObject
    {
        public event Action OnStartEvent = delegate {  };
        
        internal void Start()
        {
            OnStartEvent.Invoke();
        }
    }
}