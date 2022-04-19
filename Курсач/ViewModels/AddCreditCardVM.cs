﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Position;
using Курсач.Singleton;
using ToastNotifications.Messages;
using System.Text.RegularExpressions;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Курсач.ViewModels
{
    public class AddCreditCardVM : BaseViewModel
    {
        #region Data

        private string creditCard;
        public string CREDIT_CARD
        {
            get
            {
                return creditCard;
            }
            set
            {
                creditCard = value;
                OnPropertyChanged("CREDIT_CARD");
            }
        }
        private string validity = "";
        public string Validity
        {
            get
            {
                return validity;
            }
            set
            {
                validity = value;
                OnPropertyChanged("Validity");
            }
        }
        private string cvv = "";
        public string CVV
        {
            get
            {
                return cvv;
            }
            set
            {
                cvv = value;
                OnPropertyChanged("CVV");
            }
        }

        private string cardNumberValidation;

        public string CardNumberValidation
        {
            get { return cardNumberValidation; }
            set 
            { 
                cardNumberValidation = value;
                OnPropertyChanged("CardNumberValidation");
            }
        }

        private string validityValidation;

        public string ValidityValidation
        {
            get { return validityValidation; }
            set 
            { 
                validityValidation = value;
                OnPropertyChanged("ValidityValidation");
            }
        }

        private string cvvValidation;

        public string CVVValidation
        {
            get { return cvvValidation; }
            set 
            { 
                cvvValidation = value;
                OnPropertyChanged("CVVValidation");
            }
        }


        #endregion

        #region Commands
        public ICommand CloseUserPageCommand { get; private set; } //Close User Control when user click arrow back or 
        public ICommand AddCardCommand { get; private set; } // Add new card to user or change
        public ICommand RemoveCardCommand { get; private set; } // Add new card to user or change
        #endregion

        //Constructor
        public AddCreditCardVM()
        {
            if(App.currentUser.ACCESS_LEVEL == 3)
            {
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Visibility = "Visible";
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Blur = 3;
            }
            else
            {
                AdminWindowSingleTone.GetInstance().AdminVM.Visibility = "Visible"; // deactivate dark area
                AdminWindowSingleTone.GetInstance().AdminVM.Blur = 3; // deactivate dark area

            }

            //Delegate Command
            CloseUserPageCommand = new DelegateCommand(Close);
            AddCardCommand = new DelegateCommand(AddCard);
            RemoveCardCommand = new DelegateCommand(RemoveCard);
        }

        private void RemoveCard(object obj)
        {
            App.currentUser.CARD = null;
            Close(obj);
        }

        #region Commands' Logic
        private void AddCard(object obj) // Change credit card of current user in DB
        {
            try
            {
                bool flag = true;
                Regex creditCardNumber = new Regex(@"\d{16}");
                Regex creditCardCVV = new Regex(@"\d{3}");
                Regex creditCardValidity = new Regex(@"^(0[1-9]|1[0-2])\/?([0-9]{4}|[0-9]{2})$");
                if (!creditCardNumber.IsMatch(CREDIT_CARD))
                {
                    CardNumberValidation = "Номер карты не корректен!";
                    flag = false;
                }
                else
                {
                    CardNumberValidation = "";
                }
                if (!creditCardCVV.IsMatch(CVV))
                {
                    CVVValidation = "Введенный CVV не корректен!";
                    flag = false;
                }
                else
                {
                    CVVValidation = "";
                }
                if (!creditCardValidity.IsMatch(Validity))
                {
                    ValidityValidation = "Срок действия карты не корректен!";
                    flag = false;
                }
                else
                {
                    ValidityValidation = "";
                }
                if (!flag)
                {
                    return;
                }
                using (OracleCommand command = new OracleCommand("system.ADD_CARD", con))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.Add("USER", OracleDbType.Varchar2).Value = App.currentUser.USER_ID;
                    command.Parameters.Add("C_NUMBER", OracleDbType.Varchar2).Value = CREDIT_CARD;
                    command.Parameters.Add("EXP_DATE", OracleDbType.Varchar2).Value = Validity;
                    command.Parameters.Add("CVV", OracleDbType.Varchar2).Value = CVV;
                    con.Open();
                    command.ExecuteNonQuery();
                    con.Close();
                }

            }
            catch (Exception ex)
            {
                App.notifier.ShowError(ex.Message);
            }
            Close(obj); // Close User control
            App.notifier.ShowSuccess("Карта успешно добавлена");
        }

        private void Close(object obj) // Close user control
        {
            if (App.currentUser.ACCESS_LEVEL == 3)
            {
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.AddCreditCardViewModel = null; //viewmodel in content control = null
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Visibility = "Collapsed"; // deactivate dark area
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Blur = 0; // deactivate dark area
            }
            else
            {
                AdminWindowSingleTone.GetInstance().AdminVM.AddCreditCardViewModel = null;
                AdminWindowSingleTone.GetInstance().AdminVM.Visibility = "Collapsed"; // deactivate dark area
                AdminWindowSingleTone.GetInstance().AdminVM.Blur = 0; // deactivate dark area
                AdminWindowSingleTone.GetInstance().AdminVM.CurrentPageViewModel = new UserPageVM();
            }
        }
        #endregion
    }
}
