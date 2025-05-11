using System.Collections.ObjectModel;
using SiemensInternship.Model;

namespace SiemensInternship.Service
{
    public interface IBookService
    {
        Task<Book> AddBookAsync(string title, string author, string quantity);
        Task UpdateBookAsync(int id, string newTitle, string newAuthor, string newQuantity);
        Task DeleteBookAsync(int id);
        Task<List<Book>> LoadBooksAsync();
        ObservableCollection<Book> ApplyFilter(string text, ObservableCollection<Book> FilteredBooks, ObservableCollection<Book> Books);
    }
}
