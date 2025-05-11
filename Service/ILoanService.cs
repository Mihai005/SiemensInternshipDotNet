using System.Collections.ObjectModel;
using SiemensInternship.Model;

namespace SiemensInternship.Service
{
    public interface ILoanService
    {
        Task<List<Book>> LoadBooksAsync();
        Task<List<Loan>> LoadLoansAsync();
        Task LoanBookAsync(Book selectedBook);
        Task ReturnBookAsync(Loan selectedLoan);
        ObservableCollection<Book> ApplyFilter(string text, ObservableCollection<Book> FilteredBooks, ObservableCollection<Book> Books);
    }
}
