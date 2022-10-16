// Unity Engine
using UnityEngine;

// Otherworld
namespace Otherworld.Events
{
    /// <summary>
    /// Event channel for sending Int data
    /// </summary>
    
    [CreateAssetMenu(fileName = "new IntChannel", menuName = "Event Channels/Int")]
    public class IntChannel : GenericEventChannel<int>
    {
    }
}