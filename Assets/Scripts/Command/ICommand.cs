// Otherworld
namespace Otherworld.Command
{
    /// <summary>
    /// This interface is a base for all commands in the application
    /// </summary>
    
    public interface ICommand
    {
        public void Execute();
        public void Undo();
    }
}