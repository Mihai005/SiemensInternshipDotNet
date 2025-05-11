using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SiemensInternship.ViewModel;

namespace SiemensInternship.Views
{
    /// <summary>
    /// Interaction logic for BookCrudPage.xaml
    /// </summary>
    public partial class BookCrudPage : Page
    {
        private BookViewModel? _viewModel;
        public BookCrudPage()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<BookViewModel>();
            this.DataContext = _viewModel;
        }

        private void CloseErrorDialog_Click(object sender, RoutedEventArgs e)
        {
            if (_viewModel != null)
            {
                _viewModel.ErrorDialogVisibility = "Collapsed";
            }
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Dashboard());
        }
    }
}
