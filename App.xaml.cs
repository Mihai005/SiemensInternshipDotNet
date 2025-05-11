using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using SiemensInternship.Data;
using SiemensInternship.Service;
using SiemensInternship.ViewModel;

namespace SiemensInternship
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            ServiceCollection services = new ServiceCollection();

            services.AddDbContext<LibraryContext>();
            services.AddSingleton<IBookService, BookService>();
            services.AddTransient<BookViewModel>();

            services.AddSingleton<ILoanService, LoanService>();
            services.AddTransient<LoanViewModel>();

            ServiceProvider = services.BuildServiceProvider();

        }
    }
}
