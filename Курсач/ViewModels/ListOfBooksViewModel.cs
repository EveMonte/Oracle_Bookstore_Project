﻿using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ToastNotifications;
using ToastNotifications.Lifetime;
using ToastNotifications.Messages;
using ToastNotifications.Position;
using Курсач.Methods;
using Курсач.Models;
using Курсач.Singleton;

namespace Курсач.ViewModels
{
    public class ListOfBooksViewModel : BaseViewModel
    {
        #region Data
        private ObservableCollection<Book> books;
        public ObservableCollection<Book> Books
        {
            get
            {
                return books;
            }
            set
            {
                books = value;
                OnPropertyChanged("Books");
            }
        }
        public ObservableCollection<GENRES> Genres { get; private set; }
        GENRES selectedGenre;
        public GENRES SelectedGenre
        {
            get { return selectedGenre; }
            set
            {
                selectedGenre = value;
                OnPropertyChanged("SelectedGenre");
            }
        }
        BOOKS selectedBook;
        public BOOKS SelectedBook
        {
            get { return selectedBook; }
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }

        private double opacityAnimationUp = 1;

        public double OpacityAnimationUp
        {
            get { return opacityAnimationUp; }
            set
            {
                opacityAnimationUp = value;
                OnPropertyChanged("OpacityAnimationUp");
            }
        }

        private double opacityAnimationDown = 0;

        public double OpacityAnimationDown
        {
            get { return opacityAnimationDown; }
            set
            {
                opacityAnimationDown = value;
                OnPropertyChanged("OpacityAnimationDown");
            }
        }

        private string imageSourceUp;

        public string ImageSourceUp
        {
            get { return imageSourceUp; }
            set
            {
                imageSourceUp = value;
                OnPropertyChanged("ImageSourceUp");
            }
        }

        bool animation = true;
        int phase = 0;

        private string imageSourceDown;

        public string ImageSourceDown
        {
            get { return imageSourceDown; }
            set
            {
                imageSourceDown = value;
                OnPropertyChanged("ImageSourceDown");
            }
        }
        private int currentIndex = -1;
        private int count = 0;

        //List<ADVERTISEMENT> ListOfAdvertisement;
        #endregion

        #region Commands
        public ICommand OpenFullInfo { get; private set; }
        public ICommand FindByGenreCommand { get; private set; }
        public ICommand ClearCommand { get; private set; }
        #endregion

        #region Commands' Logic
        private void FindByGenre(object obj)
        {
            //Books = new ObservableCollection<BOOKS>(Books.Where(n => n.GENRE == SelectedGenre.GENRE_ID));
            Items = CollectionViewSource.GetDefaultView(Books);
            Items.Filter = Search;
        }

        private void OpenFullInfoUserControl(object obj) // Open page with extended info
        {
            //foreach (GENRES genre in App.db.GENRES.ToList()) //we are looking for our book in GENRES...
            //{
            //    if (genre.GENRE_ID == SelectedBook.GENRE)
            //        SelectedBook.Genre = genre.GENRE; //... and when we find it we write it in the notmapped property
            //}
            //SelectedBook.NUMBEROFVOICES = App.db.MARKS.Where(n => n.BOOK_ID == SelectedBook.BOOK_ID).Count(); //counting marks to write in notmapped property
            //MARKS mark = App.db.MARKS.FirstOrDefault(n => (n.USER_ID == App.currentUser.USER_ID) && (n.BOOK_ID == SelectedBook.BOOK_ID));
            //SelectedBook.Mark = mark != null ? (int)mark.MARK : 0;
            //FullInfoViewModelSingleTone.GetInstance(new FullInfoViewModel()).FullInfoViewModel.CurrentBook = SelectedBook;
            //WorkFrameSingleTone.GetInstance().WorkframeViewModel.CurrentPageViewModel = new AdditionalInfoViewModel();
        }
        #endregion

        //Constructor
        public ListOfBooksViewModel()
        {
            try 
            {
                OracleCommand cmd = new OracleCommand();
                Books = new ObservableCollection<Book>();
                cmd.CommandText = "select * from system.BOOKS b left join system.SHELVES s on b.BOOK_id = s.book_id where s.user_id != " + App.currentUser.USER_ID + " or s.user_id is null";
                cmd.Connection = con;
                con.Open();
                OracleDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Books.Add(new Book(dr["TITLE"].ToString(), dr["AUTHOR"].ToString(), null, decimal.Parse(dr["PRICE"].ToString()), dr["COVER"].ToString(),
                            int.Parse(dr["BY_SUBSCRIPTION"].ToString()), decimal.Parse(dr["RATING"].ToString()), dr["DESCRIPTION"].ToString(), dr["LINK"].ToString()));
                    }
                }
                //Books = new ObservableCollection<BOOKS>(App.db.BOOKS);
                        //var shelfBooks = App.db.YOUR_BOOKS.Where(n => n.USER_ID == App.currentUser.USER_ID);
                        //foreach (var book in shelfBooks)
                        //{
                        //    var bookToRemove = Books.FirstOrDefault(n => n.BOOK_ID == book.BOOK_ID);
                        //    if (bookToRemove != null)
                        //    {
                        //        Books.Remove(bookToRemove);
                        //    }
                        //}
                        //var basketBooks = App.db.BASKETS.Where(n => n.USER_ID == App.currentUser.USER_ID);
                        //foreach (var book in Books)
                        //{
                        //    //if (basketBooks.FirstOrDefault(n => n.BOOK_ID == book.BOOK_ID) != null)
                        //    //{
                        //    //    book.IsInBasket = 1;
                        //    //}
                        //    //else
                        //    //{
                        //    //    book.IsInBasket = 0;
                        //    //}
                        //}
                        //Genres = new ObservableCollection<GENRES>(App.db.GENRES.OrderBy(n => n.GENRE));
                    }
            catch (Exception ex)
            {
                App.notifier.ShowError(ex.Message);
            }
            OpenFullInfo = new DelegateCommand(OpenFullInfoUserControl);
            Items = CollectionViewSource.GetDefaultView(Books);
            Items.Filter = Search;
            FindByGenreCommand = new DelegateCommand(FindByGenre);
            ClearCommand = new DelegateCommand(ClearFilter);

            //ListOfAdvertisement = App.db.ADVERTISEMENT.ToList();
            //if(ListOfAdvertisement.Count() > 1)
            //{
            //    ImageSourceUp = ListOfAdvertisement[0].IMAGE_SOURCE;
            //    ImageSourceDown = ListOfAdvertisement[1].IMAGE_SOURCE;

            //    System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            //    count = ListOfAdvertisement.Count();
            //    currentIndex = 1;
            //    timer.Tick += new EventHandler(timerTick);
            //    timer.Interval = new TimeSpan(0, 0, 1);
            //    timer.Start();
            //}
            //else
            //    ImageSourceUp = ListOfAdvertisement[0].IMAGE_SOURCE;
        }

        private void ClearFilter(object obj) //удалить все фильтры
        {
            Text = "";
            try
            {
                //Books = new ObservableCollection<BOOKS>(App.db.BOOKS);
                //var shelfBooks = App.db.YOUR_BOOKS.Where(n => n.USER_ID == App.currentUser.USER_ID);
                //foreach (var book in shelfBooks)
                //{
                //    var bookToRemove = Books.FirstOrDefault(n => n.BOOK_ID == book.BOOK_ID);
                //    if (bookToRemove != null)
                //    {
                //        Books.Remove(bookToRemove);
                //    }
                //}
                //var basketBooks = App.db.BASKETS.Where(n => n.USER_ID == App.currentUser.USER_ID);
                //foreach (var book in Books)
                //{
                //    //if (basketBooks.FirstOrDefault(n => n.BOOK_ID == book.BOOK_ID) != null)
                //    //{
                //    //    book.IsInBasket = 1;
                //    //}
                //    //else
                //    //{
                //    //    book.IsInBasket = 0;
                //    //}
                //}
            }
            catch(Exception ex)
            {
                App.notifier.ShowError(ex.Message);
            }
        }

        private void timerTick(object sender, EventArgs e)
        {
            //if (animation)
            //{
            //    if(phase >= 200)
            //    {
            //        if (OpacityAnimationUp < 0.02)
            //        {
            //            phase = 0;
            //            if(currentIndex == count - 1)
            //            {
            //                ImageSourceUp = ListOfAdvertisement[0].IMAGE_SOURCE;
            //                currentIndex = 0;
            //            }
            //            else
            //            {
            //                currentIndex++;
            //                ImageSourceUp = ListOfAdvertisement[currentIndex].IMAGE_SOURCE;
            //            }
            //            animation = !animation;
            //        }
            //        else
            //        {
            //            OpacityAnimationUp -= 0.05;
            //            OpacityAnimationDown += 0.05;
            //        }
            //    }
            //}
            //else
            //{
            //    if (phase >= 200)
            //    {
            //        if (OpacityAnimationUp > 0.98)
            //        {
            //            phase = 0;
            //            if (currentIndex == count - 1)
            //            {
            //                ImageSourceDown = ListOfAdvertisement[0].IMAGE_SOURCE;
            //                currentIndex = 0;
            //            }
            //            else
            //            {
            //                currentIndex++;
            //                ImageSourceDown = ListOfAdvertisement[currentIndex].IMAGE_SOURCE;
            //            }
            //            animation = !animation;
            //        }
            //        else
            //        {
            //            OpacityAnimationUp += 0.05;
            //            OpacityAnimationDown -= 0.05;
            //        }
            //    }
            //}
            phase++;
        }
        #region Filter
        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Text.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(ListOfBooksViewModel), new PropertyMetadata("", TextChanged));

        private static void TextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var current = d as ListOfBooksViewModel;
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
            DependencyProperty.Register("Items", typeof(ICollectionView), typeof(ListOfBooksViewModel), new PropertyMetadata(null));
        private bool Search(object obj)
        {
            bool result = true;
            BOOKS current = obj as BOOKS;

            if (current != null && !string.IsNullOrWhiteSpace(Text) && !current.TITLE.ToLower().Contains(Text.ToLower()) && !current.AUTHOR.ToLower().Contains(Text.ToLower()))
            {
                result = false;
            }
            return result;
        }
        #endregion
    }
}
