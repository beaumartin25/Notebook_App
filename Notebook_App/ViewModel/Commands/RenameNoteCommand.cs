﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notebook_App.ViewModel.Commands
{
    public class RenameNoteCommand : ICommand
    {
        public event EventHandler? CanExecuteChanged;

        public NotesVM VM { get; set; }

        public RenameNoteCommand(NotesVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.StartRenamingNote();
        }
    }
}
