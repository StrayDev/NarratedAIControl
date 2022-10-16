using System;
using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// This class is for events related to game ticks
    /// </summary>
    
    [CreateAssetMenu(menuName = "Event Channels/Tick Channel")]
    public class TickChannel : ScriptableObject
    {
        /// <summary>
        /// Called 5 times a second at the default tick rate
        /// </summary>
        public Action OnTick = default;

        /// <summary>
        /// Called once a second at the default tick rate
        /// </summary>
        public Action OnTickMajor = default;
        
        internal void Tick()
        {
            OnTick?.Invoke();
        }
        
        internal void TickMajor()
        {
            OnTickMajor?.Invoke();
        }
    }
}