using System;
using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// This class is for events related to the MonoBehaviour update loop
    /// </summary>
    
    [CreateAssetMenu(menuName = "Events/Start Channel")]
    public class StartChannel : ScriptableObject
    {
        public event Action OnStartEvent = delegate {  };
        
        internal void Start()
        {
            OnStartEvent.Invoke();
        }

    }
}