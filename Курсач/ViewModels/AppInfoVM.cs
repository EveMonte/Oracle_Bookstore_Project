﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Курсач.Singleton;

namespace Курсач.ViewModels
{
    public class AppInfoVM : BaseViewModel
    {
        public ICommand CancelCommand { get; private set; } // cancel dialog window

        public AppInfoVM()
        {
            CancelCommand = new DelegateCommand(Cancel);
            if(App.currentUser.ACCESS_LEVEL == 3)
            {
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Visibility = "Visible";
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Blur = 3;
            }
            else
            {
                AdminWindowSingleTone.GetInstance().AdminVM.Visibility = "Visible";
                AdminWindowSingleTone.GetInstance().AdminVM.Blur = 3;

            }
        }
        private void Cancel(object obj) // cancel dialog window
        {
            if (App.currentUser.ACCESS_LEVEL == 3)
            {
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.AddCreditCardViewModel = null;
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Visibility = "Collapsed";
                WorkFrameSingleTone.GetInstance().WorkframeViewModel.Blur = 0;
            }
            else
            {
                AdminWindowSingleTone.GetInstance().AdminVM.Visibility = "Collapsed";
                AdminWindowSingleTone.GetInstance().AdminVM.AddCreditCardViewModel = null;
                AdminWindowSingleTone.GetInstance().AdminVM.Blur = 0;

            }
        }

    }
}
