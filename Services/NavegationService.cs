using Independiente.Model;
using Independiente.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Independiente.Services
{
    public interface INavigationService
    {
        void NavigateTo<TViewModel>(object parameter = null);
        void GoBack();
    }

    public class FrameNavigationService : INavigationService
    {
        private readonly Frame _frame;
        private readonly Func<Type, object, Page> _viewFactory;

        public FrameNavigationService(Frame frame, Func<Type, object, Page> viewFactory)
        {
            _frame = frame;
            _viewFactory = viewFactory;
        }

        public void NavigateTo<TViewModel>(object parameter = null)
        {
            var viewModelType = typeof(TViewModel);
            var page = _viewFactory(viewModelType, parameter);
            _frame.Navigate(page);
        }

        public void GoBack()
        {
            if (_frame.CanGoBack)
                _frame.GoBack();
        }

    }

    public class PageBase<TViewModel> : Page where TViewModel : BaseViewModel
    {
        public TViewModel ViewModel => (TViewModel)DataContext;

        public PageBase(TViewModel viewModel)
        {
            DataContext = viewModel;
        }
    }

    public class PersonDataParams
    {
        public IPerson Person { get; set; }
        public PageMode Mode { get; set; }
        public RegistrationType RegistrationType { get; set; }

        public PersonDataParams(PageMode mode, RegistrationType registrationType, IPerson person)
        {
            Mode = mode;
            RegistrationType = registrationType;
            Person = person;
        }

      

        public PersonDataParams(PageMode mode)
        {
            Mode = mode;
        }

        public PersonDataParams(PageMode mode, IPerson person)
        {
            Mode = mode;
            Person = person;
        }

    }



}
