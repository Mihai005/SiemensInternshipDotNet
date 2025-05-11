using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SiemensInternship.Command;
using SiemensInternship.Model;
using SiemensInternship.Service;

namespace SiemensInternship.ViewModel
{
    public class LoanViewModel : INotifyPropertyChanged
    {
        private readonly ILoanService _loanService;

        public ObservableCollection<Book> Books { get; set; } = default!;
        public ObservableCollection<Book> FilteredBooks { get; set; } = default!;
        public ObservableCollection<Loan> Loans { get; set; } = default!;
        public Book? BookToLoan { get; set; }

        public Loan? SelectedLoan { get; set; }
        public ICommand LoanBookCommand { get; }
        public ICommand ReturnBookCommand { get; }

        private string _errorDialogVisibility = "Collapsed";
        public string ErrorDialogVisibility
        {
            get => _errorDialogVisibility;
            set
            {
                _errorDialogVisibility = value;
                OnPropertyChanged();
            }
        }

        private string _errorDialogMessage = string.Empty;
        public string ErrorDialogMessage
        {
            get => _errorDialogMessage;
            set
            {
                _errorDialogMessage = value;
                OnPropertyChanged();
            }
        }

        private string _searchText;
        public string SearchText
        {
            get => _searchText;
            set
            {
                _searchText = value;
                OnPropertyChanged();
                ApplyFilter();
            }
        }
        private void ApplyFilter()
        {
            FilteredBooks = _loanService.ApplyFilter(SearchText, FilteredBooks, Books);
            OnPropertyChanged(nameof(FilteredBooks));
        }
        public LoanViewModel(ILoanService loanService)
        {
            _loanService = loanService;
            LoanBookCommand = new RelayCommand(_ => _ = LoanBookAsync());
            ReturnBookCommand = new RelayCommand(_ => _ = ReturnBookAsync());
            _ = LoadBooksAsync();
            _ = LoadLoansAsync();
        }

        private async Task LoanBookAsync()
        {
            if (BookToLoan == null)
            {
                ErrorDialogMessage = "Please select a Book";
                ErrorDialogVisibility = "Visible";
                return;
            }
            try
            {
                await _loanService.LoanBookAsync(BookToLoan);
                await LoadLoansAsync();
                OnPropertyChanged(nameof(Loans));
            }

            catch (Exception ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
                return;
            }
        }

        private async Task ReturnBookAsync()
        {
            if (SelectedLoan == null)
            {
                ErrorDialogMessage = "Please select a Loan";
                ErrorDialogVisibility = "Visible";
                return;
            }

            try
            {
                await _loanService.ReturnBookAsync(SelectedLoan);
                await LoadLoansAsync();
                OnPropertyChanged(nameof(Loans));
            }

            catch (Exception ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
                return;
            }
        }

        private async Task LoadBooksAsync()
        {
            Books = new ObservableCollection<Book>(await _loanService.LoadBooksAsync());
            FilteredBooks = new ObservableCollection<Book>(Books);
        }

        private async Task LoadLoansAsync()
        {
            Loans = new ObservableCollection<Loan>(await _loanService.LoadLoansAsync());
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
