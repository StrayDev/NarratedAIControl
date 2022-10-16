// System
using System;

// Otherworld
namespace Otherworld.Command
{
    /// <summary>
    /// Command that will undo the last command
    /// </summary>
    
    public class UndoCommand : ICommand
    {
        internal static Action UndoCallback;

        public void Execute() => UndoCallback.Invoke();
        public void Undo() => UndoCallback.Invoke();
    }
}