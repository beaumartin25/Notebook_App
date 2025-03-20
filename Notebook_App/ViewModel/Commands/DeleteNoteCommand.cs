using Notebook_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notebook_App.ViewModel.Commands
{
    public class DeleteNoteCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public NotesVM VM { get; set; }

        public DeleteNoteCommand(NotesVM vm)
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
                VM.DeleteNote(note);
        }
    }
}
