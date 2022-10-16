using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// Contains data about the current tick and tick rate
    /// </summary>
    
    [CreateAssetMenu(menuName = "Data/Tick Data")]
    public class TickData : ScriptableObject
    {
        public uint Count { get; internal set; } = 0;
        public float Rate { get; internal set; } = 1;
    }
}
