using System;
using System.Windows.Input;
using Курсач.Singleton;
using Курсач.Methods;
using System.Security;
using System.Runtime.InteropServices;
using System.Windows;
using ToastNotifications;
using ToastNotifications.Position;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using Oracle.ManagedDataAccess.Client;
using Курсач.Models;

namespace Курсач.ViewModels
{
    public class SignInVM : BaseViewModel
    {
        #region Data
        SaltedHash sh;
        User currentUser;

        public User CurrentUser
        {
            get
            {
                return currentUser;
            }
            set
            {
                currentUser = value;
            }
        }
        private string email;
        public string Email
        {
            get { return email; }
            set
            {
                email = value;
                OnPropertyChanged("Email");
            }
        }
        private SecureString password;
        public SecureString Password
        {
            get { return password; }
            set
            {
                password = value;
                OnPropertyChanged("Password");
            }
        }
        private string code;
        #endregion

        #region Commands
        public ICommand OpenRegisterControlCommand { get; private set; } // Открыть user control с формой для регистрации
        public ICommand OpenWorkFrameCommand { get; private set; } // Успешная регистрация и открытие рабочего окна
        public ICommand ForgotPasswordCommand { get; private set; } // Успешная регистрация и открытие рабочего окна
        #endregion

        #region Command's Logic
        void OpenWorkFrame(object obj) // Открыть главную страницу
        {
            IntPtr passwordBSTR = default(IntPtr);
            string insecurePassword = "";


            // immediately use insecurePassword (in local variable) value after decrypting it:

            bool flag = true;
            try
            {
                if (Email == null || Email == "")
                {
                    App.notifier.ShowWarning("Поле для Email не должно быть пустым!");
                    if (Password == null)
                    {
                        App.notifier.ShowWarning("Поле для пароля не должно быть пустым!");
                    }
                    return;
                }
                else
                {
                    if (Password == null)
                    {
                        App.notifier.ShowWarning("Поле для пароля не должно быть пустым!");
                        return;
                    }
                }
                OracleCommand cmd = new OracleCommand();
                cmd.CommandText = "select * from system.USERS";
                cmd.Connection = con;
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        passwordBSTR = Marshal.SecureStringToBSTR(password);
                        insecurePassword = Marshal.PtrToStringBSTR(passwordBSTR);
                        if (SaltedHash.Verify(dr["PASSWORD"].ToString().Substring(44), dr["PASSWORD"].ToString().Substring(0, 44), insecurePassword) && dr["EMAIL"].ToString() == Email)
                        {
                            App.currentUser = new User(dr["EMAIL"].ToString(), dr["SECOND_NAME"].ToString(), dr["NAME"].ToString(), dr["PASSWORD"].ToString(), int.Parse(dr["ACCESS_LEVEL"].ToString()));
                            App.currentUser.USER_ID = int.Parse(dr["USER_ID"].ToString());
                            flag = false;
                            if (App.currentUser.ACCESS_LEVEL != 3)
                            {
                                AdminWindowSingleTone.GetInstance(new AdminVM());
                                AdminWindowSingleTone.GetInstance().AdminVM.CurrentPageViewModel = new ListOfBooksAdminVM(AdminWindowSingleTone.GetInstance().AdminVM.Books);
                                AdminWindowSingleTone.GetInstance().AdminVM.ButtonVisibility = App.currentUser.ACCESS_LEVEL == 1 ? "Visible" : "Collapsed";
                                AdminWindow adminWindow = new AdminWindow();
                                App.CreateNotifier(adminWindow);
                                adminWindow.Show();
                            }
                            else
                            {
                                Workframe workframe = new Workframe();
                                App.CreateNotifier(workframe);
                                workframe.Show();
                                WorkFrameSingleTone.GetInstance(new WorkframeViewModel());
                            }

                            var windows = Application.Current.Windows;
                            foreach (Window window in windows)
                                if (window != null && window is MainWindow)
                                    window.Close();
                            Password.Dispose();
                            insecurePassword = null;
                            break;
                        }
                    }
                }
                con.Close();
                if (flag)
                {
                    App.notifier.ShowWarning("Пользователя с таким Email или паролем не существует." +
                        "\nПроверьте правильность введенных данных");
                }
            }
            catch
            {
                insecurePassword = "";
                Password.Dispose();
            }

        }
        void OpenRegisterWindow(object obj) // Открыть UserControl с регистрацией
        {
            MainWindowViewModelSingleton.GetInstance().MainFrameViewModel.SelectedViewModel = new RegistrationViewModel();
        }
        #endregion
        public SignInVM()
        {
            OpenRegisterControlCommand = new DelegateCommand(OpenRegisterWindow);
            OpenWorkFrameCommand = new DelegateCommand(OpenWorkFrame);
            ForgotPasswordCommand = new DelegateCommand(ForgotPassword);        
        }

        private void ForgotPassword(object obj)
        {
            MainWindowViewModelSingleton.GetInstance().MainFrameViewModel.SelectedViewModel = new ForgotPasswordVM();
        }
    }
}
