using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using SiemensInternship.Command;
using SiemensInternship.Model;
using SiemensInternship.Service;

namespace SiemensInternship.ViewModel
{
    public class BookViewModel : INotifyPropertyChanged
    {
        private readonly IBookService _bookService;
        public ObservableCollection<Book> Books { get; set; } = default!;

        public ObservableCollection<Book> FilteredBooks { get; set; } = default!;

        private Stack<UndoAction> _undoStack = new Stack<UndoAction>();
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand UndoCommand { get; }

        private string _title = string.Empty;
        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        private string _author = string.Empty;
        public string Author
        {
            get => _author;
            set
            {
                _author = value;
                OnPropertyChanged();
            }
        }

        private string _quantity = string.Empty;
        public string Quantity
        {
            get => _quantity;
            set
            {
                _quantity = value;
                OnPropertyChanged();
            }
        }

        private Book? _selectedBook;
        public Book? SelectedBook
        {
            get => _selectedBook;
            set
            {
                _selectedBook = value;
                OnPropertyChanged();
                if (_selectedBook != null)
                {
                    Title = _selectedBook.Title;
                    Author = _selectedBook.Author;
                    Quantity = _selectedBook.Quantity.ToString();
                }
            }
        }

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

        public BookViewModel(IBookService bookService)
        {
            _bookService = bookService;
            AddCommand = new RelayCommand(_ => _ = AddBookAsync());
            UpdateCommand = new RelayCommand(_ => _ = UpdateBookAsync());
            DeleteCommand = new RelayCommand(_ => _ = DeleteBookAsync());
            UndoCommand = new RelayCommand(_ => _ = UndoAsync());
            _ = LoadBooksAsync();
        }

        private async Task AddBookAsync()
        {
            try
            {
                Book book = await _bookService.AddBookAsync(Title, Author, Quantity);
                Books.Add(book);
                FilteredBooks.Add(book);
                _undoStack.Push(new UndoAction { ActionType = UndoActionType.Create, Book = book });

                Title = "";
                Author = "";
                Quantity = "";

                OnPropertyChanged(nameof(Books));
            }
            catch (LogicalException ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
            }

            catch (Exception ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
                Console.WriteLine("This shouldn't happen");
            }
        }

        private async Task UpdateBookAsync()
        {
            if (SelectedBook == null)
            {
                ErrorDialogMessage = "Please select a Book";
                ErrorDialogVisibility = "Visible";
                return;
            }

            try
            {
                Book originalState = new Book
                {
                    Title = SelectedBook.Title,
                    Author = SelectedBook.Author,
                    Quantity = SelectedBook.Quantity
                };

                await _bookService.UpdateBookAsync(SelectedBook.Id, Title, Author, Quantity);

                SelectedBook.Title = Title;
                SelectedBook.Author = Author;
                SelectedBook.Quantity = Int32.Parse(Quantity);
                Book updatedState = new Book
                {
                    Id = SelectedBook.Id,
                    Title = SelectedBook.Title,
                    Author = SelectedBook.Author,
                    Quantity = SelectedBook.Quantity
                };

                _undoStack.Push(new UndoAction { ActionType = UndoActionType.Update, Book = updatedState, PreviousBook = originalState });

                SelectedBook = null;

                Title = "";
                Author = "";
                Quantity = "";
            }

            catch (LogicalException ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
            }

            catch (Exception ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
                Console.WriteLine("This shouldn't happen");
            }
        }

        private async Task DeleteBookAsync()
        {
            if (SelectedBook == null)
            {
                ErrorDialogMessage = "Please select a Book";
                ErrorDialogVisibility = "Visible";
                return;
            }

            try
            {
                Book bookCopy = new Book
                {
                    Title = SelectedBook.Title,
                    Author = SelectedBook.Author,
                    Quantity = SelectedBook.Quantity
                };
                await _bookService.DeleteBookAsync(SelectedBook.Id);
                Books.Remove(SelectedBook);
                FilteredBooks.Remove(SelectedBook);
                _undoStack.Push(new UndoAction { ActionType = UndoActionType.Delete, Book = bookCopy });
                SelectedBook = null;
            }

            catch (LogicalException ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
            }

            catch (Exception ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
                Console.WriteLine("This shouldn't happen");
            }
        }

        private void ApplyFilter()
        {
            FilteredBooks = _bookService.ApplyFilter(SearchText, FilteredBooks, Books);
            OnPropertyChanged(nameof(FilteredBooks));
        }

        private async Task LoadBooksAsync()
        {
            Books = new ObservableCollection<Book>(await this._bookService.LoadBooksAsync());
            FilteredBooks = new ObservableCollection<Book>(Books);
        }

        public async Task UndoAsync()
        {
            if (_undoStack.Count == 0)
            {
                ErrorDialogMessage = "There is nothing to undo";
                ErrorDialogVisibility = "Visible";
                return;
            }

            UndoAction lastAction = _undoStack.Pop();
            Book book = lastAction.Book;
            Book previousBook = lastAction.PreviousBook;

            try
            {
                switch (lastAction.ActionType)
                {
                    case UndoActionType.Create:
                        await _bookService.DeleteBookAsync(book.Id);
                        Books.Remove(book);
                        FilteredBooks.Remove(book);
                        break;
                    case UndoActionType.Update:
                        await _bookService.UpdateBookAsync(book.Id, previousBook.Title, previousBook.Author, previousBook.Quantity.ToString());
                        lastAction.Book.Title = lastAction.PreviousBook.Title;
                        lastAction.Book.Author = lastAction.PreviousBook.Author;
                        lastAction.Book.Quantity = lastAction.PreviousBook.Quantity;
                        break;
                    case UndoActionType.Delete:
                        Book addedBook = await _bookService.AddBookAsync(book.Title, book.Author, book.Quantity.ToString());
                        Books.Add(addedBook);
                        FilteredBooks.Add(addedBook);
                        break;
                }
            }

            catch (LogicalException ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
                return;
            }

            catch (Exception ex)
            {
                ErrorDialogMessage = ex.Message;
                ErrorDialogVisibility = "Visible";
                Console.WriteLine("This shouldn't happen");
                return;
            }

            OnPropertyChanged(nameof(Books));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
