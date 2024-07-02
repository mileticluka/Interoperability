using System;
using DAL.Model;

namespace DAL.BookRepo
{
	public interface IBookRepository
	{
		IList<Book> getAll();
		Book? getById(int id);

		public void AddBook(Book newBook);

		public void UpdateBook(Book updatedBook);

		public void DeleteBook(int id);

    }
}

