using Arbook_tools.Infastructure.Commands;
using Arbook_tools.Model;
using Arbook_tools.Services;
using Arbook_tools.ViewModels.Base;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Arbook_tools.ViewModels
{
    public class GlobalDataViewModel : ViewModel
    {
        private string _token = "";
        public GraphQLService graphQl;

        public string Token
        {
            get => _token;
            set
            {
                if (_token != value)
                {
                    _token = value;
                    OnPropertyChanged(nameof(Token));
                }
            }
        }

        public ICommand GlobalDataCommand { get; }

        private  void OnGlobalDataCommandExecuted(object p)
        {
        }
        private bool CanGlobalDataCommandCommand(object p) => true;

        public GlobalDataViewModel()
        {
            GlobalDataCommand = new LambdaCommand(OnGlobalDataCommandExecuted, CanGlobalDataCommandCommand);
            graphQl = new GraphQLService("https://api.lk.flexreality.pro/graphql");
        }
    }
}
