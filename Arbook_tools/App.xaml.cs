using Arbook_tools.ViewModels;
using System;
using System.Windows;
using System.Windows.Media.Imaging;

namespace Arbook_tools
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public GlobalDataViewModel? GlobalData { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            GlobalData = new GlobalDataViewModel();

            foreach (Window window in Application.Current.Windows)
            {
                Uri iconUri = new Uri("pack://application:,,,/Arbook_tools;component/icon.png", UriKind.RelativeOrAbsolute);
                window.Icon = BitmapFrame.Create(iconUri);
            }
        }
    }
}
