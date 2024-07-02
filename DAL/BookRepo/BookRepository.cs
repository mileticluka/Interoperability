using DAL.Model;

namespace DAL.BookRepo;

public class BookRepository : IBookRepository
{
    private readonly DataContext _dataContext;

    public BookRepository()
    {
        _dataContext = DataContext.Instance;
    }

    public IList<Book> getAll()
    {
        return _dataContext.Books;
    }

    public Book? getById(int id)
    {
        return _dataContext.Books.FirstOrDefault(b => b.Id == id);
    }

    public void AddBook(Book newBook)
    {
        if (_dataContext.Books.Any(b => b.Id == newBook.Id))
        {
            throw new ArgumentException("A book with the same ID already exists.");
        }

        if (newBook.Id == 0 || newBook.Id == null)
        {
            newBook.Id = _dataContext.Books.Count > 0 ? _dataContext.Books.Max(b => b.Id) + 1 : 1;
        }

        _dataContext.Books.Add(newBook);
    }

    public void UpdateBook(Book updatedBook)
    {
        var existingBook = _dataContext.Books.FirstOrDefault(b => b.Id == updatedBook.Id);
        if (existingBook == null)
        {
            throw new ArgumentException("The book to be updated does not exist.");
        }

        existingBook.Title = updatedBook.Title;
        existingBook.Author = updatedBook.Author;
        existingBook.OriginalLanguage = updatedBook.OriginalLanguage;
        existingBook.FirstPublished = updatedBook.FirstPublished;
        existingBook.ApproxSalesInMillions = updatedBook.ApproxSalesInMillions;
        existingBook.Genre = updatedBook.Genre;
    }

    public void DeleteBook(int id)
    {
        var bookToRemove = _dataContext.Books.FirstOrDefault(b => b.Id == id);
        if (bookToRemove == null)
        {
            throw new ArgumentException("The book to be deleted does not exist.");
        }

        _dataContext.Books.Remove(bookToRemove);
    }
}
