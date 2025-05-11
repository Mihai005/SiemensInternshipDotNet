using System.Collections.ObjectModel;
using Microsoft.EntityFrameworkCore;
using SiemensInternship.Data;
using SiemensInternship.Model;

namespace SiemensInternship.Service
{
    public class BookService : IBookService
    {
        private readonly LibraryContext _libraryContext;

        public BookService(LibraryContext libraryContext)
        {
            _libraryContext = libraryContext;
        }

        public async Task<Book> AddBookAsync(string title, string author, string quantity)
        {
            ValidateFields(title, author, quantity);

            int quantityInt = Int32.Parse(quantity);
            var existing = await _libraryContext.Books.FirstOrDefaultAsync(b => b.Title.ToLower().Equals(title.ToLower()) &&
                b.Author.ToLower().Equals(author.ToLower()));

            if (existing != null)
            {
                throw new Exception("Book with given title and author already exists");
            }

            else
            {
                var newBook = new Book
                {
                    Author = author,
                    Title = title,
                    Quantity = quantityInt,
                };

                await _libraryContext.Books.AddAsync(newBook);
                await _libraryContext.SaveChangesAsync();

                return newBook;
            }
        }

        public async Task DeleteBookAsync(int id)
        {
            var book = await _libraryContext.Books.FindAsync(id);

            int numberOfActiveRentals = await _libraryContext.Loans
                .Where(l => l.BookId == id && l.ReturnDate == null)
                .CountAsync();

            if (numberOfActiveRentals > 0)
            {
                throw new Exception("Cannot delete book: there are active loans");
            }

            if (book != null)
            {
                _libraryContext.Books.Remove(book);
                await _libraryContext.SaveChangesAsync();
            }

            else
            {
                throw new Exception("Book doesn't exist");
            }
        }

        public async Task UpdateBookAsync(int id, string newTitle, string newAuthor, string newQuantity)
        {
            ValidateFields(newTitle, newAuthor, newQuantity);

            var book = await _libraryContext.Books.FindAsync(id);
            int numberOfActiveRentals = await _libraryContext.Loans.Where(l => l.BookId == id &&
                l.ReturnDate == null).CountAsync();

            if (Int32.Parse(newQuantity) < numberOfActiveRentals)
            {
                throw new Exception("Cannot modify the quantity to be lower than the number of active rentals");
            }

            if (book != null)
            {
                book.Title = newTitle;
                book.Author = newAuthor;
                book.Quantity = Int32.Parse(newQuantity);

                await _libraryContext.SaveChangesAsync();
            }

            else
            {
                throw new Exception("Book doesn't exist");
            }
        }

        public ObservableCollection<Book> ApplyFilter(string text, ObservableCollection<Book> FilteredBooks, ObservableCollection<Book> Books)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                FilteredBooks.Clear();
                foreach (var book in Books)
                    FilteredBooks.Add(book);
            }
            else
            {
                var lower = text.ToLower();
                var filtered = Books.Where(b => b.Title.ToLower().Contains(lower));
                FilteredBooks.Clear();
                foreach (var book in filtered)
                    FilteredBooks.Add(book);
            }

            return FilteredBooks;
        }

        private static void ValidateFields(string title, string author, string quantity)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new Exception("Title can't be empty");
            }

            if (string.IsNullOrEmpty(author))
            {
                throw new Exception("Author can't be empty");
            }

            int quantityInt = -1;
            bool ok = Int32.TryParse(quantity, out quantityInt);

            if (!ok || quantityInt <= 0)
                throw new Exception("Quantity has to be a positive integer");
        }

        public async Task<List<Book>> LoadBooksAsync()
        {
            return await _libraryContext.Books.ToListAsync();
        }
    }
}
