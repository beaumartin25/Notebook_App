using Notebook_App.Model;
using Notebook_App.ViewModel.Commands;
using Notebook_App.ViewModel.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Notebook_App.ViewModel
{
    public class LoginVM : INotifyPropertyChanged
    {
		private User user;
		public User User
		{
			get { return user; }
			set 
			{ 
				user = value;
				OnPropertyChanged("User");
			}
		}

        private string firstName;
        public string FirstName
        {
            get { return firstName; }
            set
            {
                firstName = value;
                User = new User
                {
					Firstname = firstName,
					Lastname = this.LastName,
                    Username = this.Username,
                    Password = this.Password
                };
                OnPropertyChanged("FirstName");
            }
        }

        private string lastName;
        public string LastName
        {
            get { return lastName; }
            set
            {
                lastName = value;
                User = new User
                {
                    Firstname = this.FirstName,
                    Lastname = lastName,
                    Username = this.Username,
                    Password = this.Password
                };
                OnPropertyChanged("LastName");
            }
        }

        private string username;
		public string Username
		{
			get { return username; }
			set 
			{ 
				username = value;
                User = new User
                {
                    Firstname = this.FirstName,
                    Lastname = this.LastName,
                    Username = username,
                    Password = this.Password
                };
                OnPropertyChanged("Username");
            }
		}

		private string password;
		public string Password
		{
			get { return password; }
			set 
			{ 
				password = value;
                User = new User
                {
                    Firstname = this.FirstName,
                    Lastname = this.LastName,
                    Username = this.Username,
                    Password = password
                };
                OnPropertyChanged("Password");
            }
		}

		private Visibility loginVis;
        public Visibility LoginVis
		{
			get { return loginVis; }
			set
			{
				loginVis = value;
				OnPropertyChanged("LoginVis");
			}
		}

        private Visibility registerVis;
        public Visibility RegisterVis
        {
            get { return registerVis; }
            set
            {
                registerVis = value;
                OnPropertyChanged("RegisterVis");
            }
        }


        public event PropertyChangedEventHandler? PropertyChanged;
        public event EventHandler Authenticated;

        public RegisterCommand RegisterCommand { get; set; }
        public LoginCommand LoginCommand { get; set; }
        public ShowRegisterCommand ShowRegisterCommand { get; set; }

        // Constructor
        public LoginVM()
		{
			LoginVis = Visibility.Visible;
			RegisterVis = Visibility.Collapsed;

			RegisterCommand = new RegisterCommand(this);
			LoginCommand = new LoginCommand(this);
			ShowRegisterCommand = new ShowRegisterCommand(this);

			User = new User();
		}

		public void SwitchViews()
		{

			if (RegisterVis != Visibility.Visible)
			{
				RegisterVis = Visibility.Visible;
				LoginVis = Visibility.Collapsed;
			}
			else
			{
				RegisterVis = Visibility.Collapsed;
				LoginVis = Visibility.Visible;
			}
		}

		public void Login()
		{
            bool result = DatabaseHelper.ValidateUser(User.Username, User.Password);

            if (result)
            {
                Authenticated?.Invoke(this, EventArgs.Empty);
            }
		}

		public void Register()
		{
            // would want to add bcrypt to passwords
            bool result = DatabaseHelper.Insert(User);

            if (result)
            {
                Authenticated?.Invoke(this, EventArgs.Empty);
            }
		}

		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
    }
}
