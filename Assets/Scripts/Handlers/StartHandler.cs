using UnityEngine;

namespace Otherworld.Events
{
    /// <summary>
    /// This class calls an event for the MonoBehaviour start event 
    /// </summary>
    
    public class StartHandler : MonoBehaviour
    {
        [SerializeField] private StartChannel startChannel;

        private void Start() => startChannel.Start();
    }
}
