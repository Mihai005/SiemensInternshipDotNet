using System.Windows;
using System.Windows.Controls;

namespace SiemensInternship.Views
{
    /// <summary>
    /// Interaction logic for Dashboard.xaml
    /// </summary>
    public partial class Dashboard : Page
    {
        public Dashboard()
        {
            InitializeComponent();
        }

        private void ManageBooksButton_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new BookCrudPage());
        }

        private void ManageLoansButton_Clicked(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new LoanPage());
        }
    }
}
