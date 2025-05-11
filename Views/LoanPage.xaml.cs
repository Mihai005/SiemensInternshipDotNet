using System.Windows;
using System.Windows.Controls;
using Microsoft.Extensions.DependencyInjection;
using SiemensInternship.ViewModel;

namespace SiemensInternship.Views
{
    /// <summary>
    /// Interaction logic for LoanPage.xaml
    /// </summary>
    public partial class LoanPage : Page
    {
        private LoanViewModel? _viewModel;
        public LoanPage()
        {
            InitializeComponent();
            _viewModel = App.ServiceProvider.GetService<LoanViewModel>();
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
            MainWindow mainWindow = (MainWindow)Application.Current.MainWindow;
            mainWindow.MainFrame.Navigate(new Dashboard());
        }
    }
}
