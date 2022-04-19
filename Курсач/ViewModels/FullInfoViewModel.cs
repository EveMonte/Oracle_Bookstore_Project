using Курсач.Models;

namespace Курсач.ViewModels
{
    public class FullInfoViewModel : BaseViewModel
    {
        private Book currentBook;
        public Book CurrentBook
        {
            get
            {
                return currentBook;
            }
            set
            {
                currentBook = value;
                OnPropertyChanged("CurrentBook");
            }
        }
        public FullInfoViewModel()
        {

        }
    }
}
