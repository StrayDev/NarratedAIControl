using System;
using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// This class is for events related to game ticks
    /// </summary>
    
    [CreateAssetMenu(menuName = "Composite Channels/Tick")]
    public class TickChannel : ScriptableObject
    {
        /// <summary>
        /// Called 5 times a second at the default tick rate
        /// </summary>
        public event Action OnTickEvent = default;

        /// <summary>
        /// Called once a second at the default tick rate
        /// </summary>
        public event Action OnMajorTickEvent = default;
        
        internal void Tick()
        {
            OnTickEvent?.Invoke();
        }
        
        internal void TickMajor()
        {
            OnMajorTickEvent?.Invoke();
        }
    }
}