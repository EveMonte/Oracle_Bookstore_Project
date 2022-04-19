using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;
using System.Windows;
using System.Windows.Input;
using ToastNotifications.Messages;
using Курсач.Methods;
using Курсач.Models;
using Курсач.Singleton;

namespace Курсач.ViewModels
{
    public class SendMessageViewModel : BaseViewModel
    {
        public User newUser;
        public string code;
        public string Code
        {
            get { return code; }
            set
            {
                code = value;
                OnPropertyChanged("Code");
            }
        }
        private string generatedCode;



        public ICommand SendMessageCommand { get; private set; }
        public ICommand SendNewMessageCommand { get; private set; }
        public ICommand OpenRegistrationCommand { get; private set; }
        private void OpenRegistration(object obj)
        {
            MainWindowViewModelSingleton.GetInstance().MainFrameViewModel.SelectedViewModel = new RegistrationViewModel();
            code = "";
        }
        public void OpenWorkframe(object parameter)
        {
            try 
            {
                if (code == generatedCode)
                {
                    using (OracleCommand command = new OracleCommand("system.CREATE_USER", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.Add("EMAIL", OracleDbType.Varchar2).Value = newUser.EMAIL;
                        command.Parameters.Add("SECOND_NAME", OracleDbType.Varchar2).Value = newUser.SECOND_NAME;
                        command.Parameters.Add("NAME", OracleDbType.Varchar2).Value = newUser.NAME;
                        command.Parameters.Add("PASSWORD", OracleDbType.Varchar2).Value = newUser.PASSWORD;
                        command.Parameters.Add("ACCESS_LVL", OracleDbType.Decimal).Value = 3m;
                        command.Parameters.Add("OUT_ID", OracleDbType.Int32).Direction = ParameterDirection.Output;
                        con.Open();
                        command.ExecuteNonQuery();
                        App.currentUser = newUser;

                        App.currentUser.USER_ID = int.Parse(command.Parameters["OUT_ID"].Value.ToString());
                        //con.Close();
                    }
                    //App.currentUser.USER_ID = int.Parse(dr["USER_ID"].ToString()); --get user id , it's better to get id from out parameter

                    Workframe workframe = new Workframe();
                    App.CreateNotifier(workframe);
                    workframe.Show();
                    WorkFrameSingleTone.GetInstance(new WorkframeViewModel());
                    var windows = Application.Current.Windows;
                    foreach (Window window in windows)
                        if (window != null && window is MainWindow)
                            window.Close();
                }
                else 
                {
                    App.notifier.ShowWarning("Неправильный код");
                }
            }
            
            catch(Exception ex)
            {
                App.notifier.ShowError(ex.Message);
            }
        }
        private void SendNewMessage(object obj)
        {
            generatedCode = MessageSender.GenerateCode();
            string message = $"Введите символьный код, расположенный ниже, в приложение:\n{generatedCode}\nНикому не давайте его!";
            MessageSender.SendEmailAsync(newUser.EMAIL, generatedCode, message, "Код подтверждения").GetAwaiter();
        }

        //Constructors
        public SendMessageViewModel(User user)
        {
            newUser = user;
            SendMessageCommand = new DelegateCommand(OpenWorkframe);
            OpenRegistrationCommand = new DelegateCommand(OpenRegistration);
            SendNewMessageCommand = new DelegateCommand(SendNewMessage);
            generatedCode = MessageSender.GenerateCode();
            string message = $"Введите символьный код, расположенный ниже, в приложение:\n{generatedCode}\nНикому не давайте его!";
            MessageSender.SendEmailAsync(newUser.EMAIL, generatedCode, message, "Код подтверждения").GetAwaiter();
        }
    }
}
