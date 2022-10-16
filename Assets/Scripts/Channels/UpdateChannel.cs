using System;
using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// This class is for events related to the MonoBehaviour update loop
    /// </summary>
    
    [CreateAssetMenu(menuName = "Events/Update Channel")]
    public class UpdateChannel : ScriptableObject
    {
        public event Action UpdateEvent      = delegate {  };
        public event Action FixedUpdateEvent = delegate {  };
        public event Action LateUpdateEvent  = delegate {  };

        internal void Update()
        {
            UpdateEvent?.Invoke();
        }
        
        internal void FixedUpdate()
        {
            FixedUpdateEvent?.Invoke();
        }
        
        internal void LateUpdate()
        {
            LateUpdateEvent?.Invoke();
        }
    }
}