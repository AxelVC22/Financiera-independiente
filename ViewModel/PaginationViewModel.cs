using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Independiente.ViewModel
{
    public class PageLink
    {
        public int? PageNumber { get; }   
        public bool IsCurrent { get; }    
        public PageLink(int? number, bool current = false)
        {
            PageNumber = number;
            IsCurrent = current;
        }
    }
    public interface IPaginationViewModel
    {
        int PageNumber { get; set; }
        int PageSize { get; set; }
        int TotalItems { get; set; }
        int TotalPages { get; }

        bool CanGoFirst { get; }
        bool CanGoPrevious { get; }
        bool CanGoNext { get; }
        bool CanGoLast { get; }

        ObservableCollection<PageLink> PageLinks { get; }

        void Refresh();
    }

    public class PaginationViewModelBase : IPaginationViewModel, INotifyPropertyChanged
    {
        private int _pageNumber = 1;

        public int PageNumber
        {
            get => _pageNumber;
            set
            {
                if (value == _pageNumber) return;
                _pageNumber = value;
                OnPropertyChanged();                               
                OnPropertyChanged(nameof(CanGoNext));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(NextPageNumber));
                OnPropertyChanged(nameof(PreviousPageNumber));
                Refresh();                                          
            }
        }
        public int PageSize { get; set; } = 2;

        int _totalItems;

        public int NextPageNumber => CanGoNext ? PageNumber + 1 : PageNumber;
        public int PreviousPageNumber => CanGoPrevious ? PageNumber - 1 : PageNumber;

        public int TotalItems
        {
            get => _totalItems;
            set
            {
                if (value == _totalItems) return;
                _totalItems = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(TotalPages));
                OnPropertyChanged(nameof(CanGoNext));
                OnPropertyChanged(nameof(CanGoPrevious));
                OnPropertyChanged(nameof(NextPageNumber));
                OnPropertyChanged(nameof(PreviousPageNumber));
            }
        }

        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);

        public bool CanGoFirst => PageNumber > 1;
        public bool CanGoPrevious => PageNumber > 1;
        public bool CanGoNext => PageNumber < TotalPages;
        public bool CanGoLast => PageNumber < TotalPages;

        public ObservableCollection<PageLink> PageLinks { get; } = new ObservableCollection<PageLink>();

        public event PropertyChangedEventHandler PropertyChanged;

        public PaginationViewModelBase(int totalItems)
        {
            TotalItems = totalItems;
            Refresh();
        }
        protected void OnPropertyChanged([CallerMemberName] string p = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(p));

        public void Refresh()
        {
            PageLinks.Clear();
            int total = TotalPages;

            if (total <= 5)
            {
                for (int i = 1; i <= total; i++)
                    PageLinks.Add(new PageLink(i, i == PageNumber));
            }
            else
            {
                if (PageNumber <= 3)
                {
                    for (int i = 1; i <= 5; i++) PageLinks.Add(new PageLink(i, i == PageNumber));
                    PageLinks.Add(new PageLink(null));
                    PageLinks.Add(new PageLink(total));
                }
                else if (PageNumber >= total - 2)
                {
                    PageLinks.Add(new PageLink(1));
                    PageLinks.Add(new PageLink(null));
                    for (int i = total - 4; i <= total; i++) PageLinks.Add(new PageLink(i, i == PageNumber));
                }
                else
                {
                    PageLinks.Add(new PageLink(1));
                    PageLinks.Add(new PageLink(null));
                    for (int i = PageNumber - 1; i <= PageNumber + 1; i++) PageLinks.Add(new PageLink(i, i == PageNumber));
                    PageLinks.Add(new PageLink(null));
                    PageLinks.Add(new PageLink(total));
                }
            }

            OnPropertyChanged(nameof(CanGoFirst));
            OnPropertyChanged(nameof(CanGoPrevious));
            OnPropertyChanged(nameof(CanGoNext));
            OnPropertyChanged(nameof(CanGoLast));
        }
    }

}
