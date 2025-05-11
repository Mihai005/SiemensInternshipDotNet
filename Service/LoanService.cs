using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using SiemensInternship.Data;
using SiemensInternship.Model;

namespace SiemensInternship.Service
{
    public class LoanService : ILoanService
    {
        private readonly LibraryContext _libraryContext;

        public LoanService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public async Task LoanBookAsync(Book selectedBook)
        {
            int activeLoansCount = await _libraryContext.Loans
                .Where(l => l.BookId == selectedBook.Id && l.ReturnDate == null)
                .CountAsync();

            if (activeLoansCount >= selectedBook.Quantity)
            {
                throw new LogicalException("Cannot loan Book: Out of stock");
            }

            Loan loan = new Loan
            {
                Book = selectedBook,
                BookId = selectedBook.Id,
                LoanDate = DateTime.Now,
            };

            await _libraryContext.Loans.AddAsync(loan);
            await _libraryContext.SaveChangesAsync();
        }

        public async Task ReturnBookAsync(Loan selectedLoan)
        {
            if (selectedLoan.ReturnDate != null)
            {
                throw new LogicalException("Cannot return Book: Already returned");
            }

            selectedLoan.ReturnDate = DateTime.Now;

            await _libraryContext.SaveChangesAsync();
        }

        public ObservableCollection<Book> ApplyFilter(string text, ObservableCollection<Book> FilteredBooks, ObservableCollection<Book> Books)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                FilteredBooks.Clear();
                foreach (Book book in Books)
                    FilteredBooks.Add(book);
            }

            else
            {
                string lower = text.ToLower();
                IEnumerable<Book> filtered = Books.Where(b => b.Title.ToLower().Contains(lower));
                FilteredBooks.Clear();
                foreach (Book book in filtered)
                    FilteredBooks.Add(book);
            }

            return FilteredBooks;
        }

        public async Task<List<Book>> LoadBooksAsync()
        {
            return await _libraryContext.Books.ToListAsync();
        }

        public async Task<List<Loan>> LoadLoansAsync()
        {
            return await _libraryContext.Loans.ToListAsync();
        }
    }
}
