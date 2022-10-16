// Unity Engine
using UnityEngine;

// Otherworld
namespace Otherworld.Command
{
    /// <summary>
    /// This channel is used to send new commands to the Command System
    /// </summary>
    
    [CreateAssetMenu(menuName = "Composite Channels/Command")]
    public class CommandChannel : GenericEventChannel<ICommand>
    {
    }
}