using Arbook_tools.Infastructure.Commands;
using Arbook_tools.Model;
using Arbook_tools.Services;
using Arbook_tools.ViewModels.Base;
using Arbook_tools;
using System;
using System.Windows;
using System.Windows.Input;

namespace Arbook_tools.ViewModels
{
    internal class LoginWindowViewModel : ViewModel
    {
        private Window _currentWindow;
        private UserModel _user;

        public string Email
        {
            get => _user.Email;
            set
            {
                if (_user.Email != value)
                {
                    _user.Email = value;
                    OnPropertyChanged(nameof(Email));
                }
            }
        }

        public string Password
        {
            get => _user.Password;
            set
            {
                if (_user.Password != value)
                {
                    _user.Password = value;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        public string Token
        {
            get => _user.Token;
            set
            {
                if (_user.Token != value)
                {
                    _user.Token = value;
                    OnPropertyChanged(nameof(Token));
                }
            }
        }

        public ICommand AuthApplocationCommand { get; }

        private async void OnAuthApplocationCommandExecuted(object p)
        {
            var globalData = ((App)Application.Current).GlobalData;
            var graphQl = globalData.graphQl;
            var response = await graphQl.LoginAuth(_user?.Email, _user?.Password, "LK_TEACHER");
            if (response != null)
            {
                globalData.Token = response;

                /* _user.Token = response;
                 OnPropertyChanged(nameof(Token));*/

                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();
                _currentWindow.Close();
            }
            else
            {
                MessageBox.Show("Error, login or password wrong!!!");
            }
        }
        private bool CanAuthApplocationCommand(object p) => true;

        public LoginWindowViewModel(Window currentWindow)
        {
            _currentWindow = currentWindow;
            _user = new UserModel();
            AuthApplocationCommand = new LambdaCommand(OnAuthApplocationCommandExecuted, CanAuthApplocationCommand);
        }
    }
}
