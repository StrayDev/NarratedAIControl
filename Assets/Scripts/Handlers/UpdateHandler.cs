using UnityEngine;

namespace Otherworld.Core
{
    /// <summary>
    /// This class calls an event for each MonoBehaviour update event 
    /// </summary>
    
    public class UpdateHandler : MonoBehaviour
    {
        [SerializeField] private UpdateChannel updateChannel;

        private void Update() => updateChannel.Update();
        private void FixedUpdate() => updateChannel.FixedUpdate();
        private void LateUpdate() => updateChannel.LateUpdate();
    }
}
