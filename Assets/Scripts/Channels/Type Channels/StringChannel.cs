// Unity Engine
using UnityEngine;

// Otherworld
namespace Otherworld.Events
{
    [CreateAssetMenu(fileName = "new StringChannel", menuName = "Event Channels/String")]
    public class StringChannel : GenericEventChannel<string>
    {
    }
}