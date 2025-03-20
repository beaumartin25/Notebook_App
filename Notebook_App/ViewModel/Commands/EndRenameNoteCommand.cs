using Notebook_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notebook_App.ViewModel.Commands
{
    public class EndRenameNoteCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public NotesVM VM { get; set; }

        public EndRenameNoteCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            Note note = parameter as Note;
            if (note != null)
                VM.StopRenamingNote(note);
        }
    }
}
