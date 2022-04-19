﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;

namespace Курсач.ViewModels
{
    public class UsersPageVM : BaseViewModel
    {
        private ObservableCollection<USERS> users;

        public ObservableCollection<USERS> Users
        {
            get { return users; }
            set
            {
                users = value;
                OnPropertyChanged("Users");
            }
        }
        private BaseViewModel currentUserControl;

        public BaseViewModel CurrentUserControl
        {
            get { return currentUserControl; }
            set
            {
                currentUserControl = value;
                OnPropertyChanged("CurrentUserControl");
            }
        }

        public ICommand RemoveCommand { get; private set; }
        public ICommand ChangeCommand { get; private set; }

        public UsersPageVM()
        {
            RemoveCommand = new DelegateCommand(RemoveUser);
            ChangeCommand = new DelegateCommand(ChangeUser);
            //Users = new ObservableCollection<USERS>(App.db.USERS.Where(n => n.ACCOUNT == "Пользователь"));
            //foreach(USERS user in Users)
            //{
            //    SUBSCRIPTIONS sub = App.db.SUBSCRIPTIONS.FirstOrDefault(n => n.SUBSCRIPTION_ID == user.SUBSCRIPTION);
            //    if(sub != null)
            //    {
            //        //user.SUBSCRIPTION_DATE = sub.SUBSCRIPTION_DATE;
            //        //user.SUBSCRIPTION_LENGTH = (int)sub.LENGTH;
            //    }
            //}
            Items = CollectionViewSource.GetDefaultView(Users);
            Items.Filter = Search;
        }

        private void ChangeUser(object obj)
        {
            USERS changedUser = Users.FirstOrDefault(n => n.USER_ID == (int)obj);
            //var user = App.db.USERS.FirstOrDefault(n => n.USER_ID == changedUser.USER_ID);
            //user = changedUser;
            //user.ACCOUNT = changedUser.ACCOUNT;
            //var sub = App.db.SUBSCRIPTIONS.FirstOrDefault(n => n.SUBSCRIPTION_ID == changedUser.SUBSCRIPTION);
            //App.db.SaveChanges();
            ////if (sub != null)
            //{
            //    sub.SUBSCRIPTION_DATE = changedUser.SUBSCRIPTION_DATE;
            //    sub.LENGTH = changedUser.SUBSCRIPTION_LENGTH;
            //}
            //else
            //{
            //    SUBSCRIPTIONS subscr = new SUBSCRIPTIONS();
            //    subscr.SUBSCRIPTION_DATE = changedUser.SUBSCRIPTION_DATE;
            //    subscr.LENGTH = changedUser.SUBSCRIPTION_LENGTH;
            //    App.db.SUBSCRIPTIONS.Add(subscr);
            //    changedUser.SUBSCRIPTION = subscr.SUBSCRIPTION_ID;
            //}
            //App.db.SaveChangesAsync().GetAwaiter();
        }

        private void RemoveUser(object obj)
        {
            //USERS userToRemove = App.db.USERS.FirstOrDefault(n => n.USER_ID == (int)obj);
            //Users.Remove(userToRemove);
            //var shelf = App.db.YOUR_BOOKS.Where(n => n.USER_ID == userToRemove.USER_ID);
            //App.db.YOUR_BOOKS.RemoveRange(shelf);
            //var basket = App.db.BASKETS.Where(n => n.USER_ID == userToRemove.USER_ID);
            //App.db.BASKETS.RemoveRange(basket);
            //var sub = App.db.SUBSCRIPTIONS.FirstOrDefault(n => n.SUBSCRIPTION_ID == userToRemove.SUBSCRIPTION);
            //App.db.YOUR_BOOKS.RemoveRange(shelf);
            //var marks = App.db.MARKS.Where(n => n.USER_ID == userToRemove.USER_ID);
            //var newMarks = App.db.MARKS.Where(n => n.USER_ID != userToRemove.USER_ID);
            //foreach (var mark in marks)
            //{
            //    var book = App.db.BOOKS.FirstOrDefault(n => n.BOOK_ID == mark.BOOK_ID);
            //    if(book != null)
            //    {
            //        int? sum = 0;
            //        foreach (MARKS newMark in newMarks.Where(n => n.BOOK_ID == book.BOOK_ID))
            //        {
            //            sum += newMark.MARK;
            //        }
            //        int count = newMarks.Where(n => n.BOOK_ID == book.BOOK_ID).Count();
            //        book.RATING = count != 0 ? (decimal)sum / (decimal)count : 0;
            //    }

            //}
            ////App.db.MARKS.RemoveRange(marks);
            //App.db.USERS.Remove(userToRemove);
            //App.db.SaveChangesAsync().GetAwaiter();
        }
        #region Filter
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(UsersPageVM), new PropertyMetadata("", TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as UsersPageVM;
            if (current != null)
            {
                current.Items.Filter = null;
                current.Items.Filter = current.Search;
            }
        }

        public ICollectionView Items
        {
            get { return (ICollectionView)GetValue(ItemsProperty); }
            set { SetValue(ItemsProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty ItemsProperty =
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(UsersPageVM), new PropertyMetadata(null));
        private bool Search(object obj)
        {
            bool result = true;
            USERS current = obj as USERS;

            if (current != null && !string.IsNullOrWhiteSpace(Text) && !current.EMAIL.ToLower().Contains(Text.ToLower()) && !current.NAME.ToLower().Contains(Text.ToLower()) && !current.SECOND_NAME.ToLower().Contains(Text.ToLower()))
            {
                result = false;
            }
            return result;
        }
        #endregion

    }
}
