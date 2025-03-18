using Notebook_App.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Notebook_App.ViewModel.Commands
{
    public class RegisterCommand : ICommand
    {
        public LoginVM VM { get; set; }
        public event EventHandler? CanExecuteChanged;

        // Constructor
        public RegisterCommand(LoginVM vm)
        {
            VM = vm;
        }

        public bool CanExecute(object? parameter)
        {
            User user = parameter as User;

            if (user == null)
                return false;
            if (string.IsNullOrEmpty(user.Firstname)) 
                return false;
            if (string.IsNullOrEmpty(user.Lastname)) 
                return false;
            if (string.IsNullOrEmpty(user.Username)) 
                return false;
            if (string.IsNullOrEmpty(user.Password)) 
                return false;
            return true;
        }

        public void Execute(object? parameter)
        {
            VM.Register();
        }
    }
}
