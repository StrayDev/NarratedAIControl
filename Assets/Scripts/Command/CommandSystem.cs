// System
using System;
using System.Collections.Generic;

// Unity Engine
using UnityEngine;

// Otherworld
namespace Otherworld.Command
{
    /// <summary>
    /// The Command System processes and records commands
    /// </summary>
    
    public class CommandSystem : MonoBehaviour
    {
        [SerializeField] private CommandChannel CommandChannel;

        private Stack<ICommand> _commands; // this needs to be saved and cleared every so often

        private void OnEnable()
        {
            _commands = new Stack<ICommand>();
            
            // set the undo action to be able to call undo
            UndoCommand.UndoCallback = Undo;
            
            // register callbacks
            CommandChannel.Callback += OnCommand;
        }

        private void OnDisable()
        {
            // unregister callbacks
            CommandChannel.Callback -= OnCommand;
        }

        private void OnCommand(ICommand command)
        {
            Record(command);
        }

        private void Record(ICommand command)
        {
            _commands.Push(command);
            command.Execute();
        }

        private void Undo()
        {
            var command = _commands.Pop();
            command.Undo();
        }
        
    }




}

