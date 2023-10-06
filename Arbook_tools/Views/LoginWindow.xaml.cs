using Arbook_tools.ViewModels;
using System.Windows;

namespace Arbook_tools.View
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        private LoginWindowViewModel _loginWindowViewModel;
        private GlobalDataViewModel _globalDataViewModel;
        public LoginWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;

            _loginWindowViewModel = new LoginWindowViewModel(this);
            _globalDataViewModel = ((App)Application.Current).GlobalData;

            _loginWindowViewModel.Email = "content@arbook.info";
            _loginWindowViewModel.Password = "k2SY^B_k55;p";

            DataContext = _loginWindowViewModel;
            LoginContainer.DataContext = _globalDataViewModel;
        }
    }
}
