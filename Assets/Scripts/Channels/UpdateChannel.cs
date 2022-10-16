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
        public event Action OnUpdateEvent      = delegate {  };
        public event Action OnFixedUpdateEvent = delegate {  };
        public event Action OnLateUpdateEvent  = delegate {  };

        internal void Update()
        {
            OnUpdateEvent?.Invoke();
        }
        
        internal void FixedUpdate()
        {
            OnFixedUpdateEvent?.Invoke();
        }
        
        internal void LateUpdate()
        {
            OnLateUpdateEvent?.Invoke();
        }
    }
}