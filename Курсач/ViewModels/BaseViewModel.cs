using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Oracle.ManagedDataAccess.Client;
using System.Configuration;
namespace Курсач.ViewModels
{
    public class BaseViewModel :  DependencyObject, INotifyPropertyChanged
    {
        protected OracleConnection con = new OracleConnection(ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
