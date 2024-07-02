using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.XPath;
using DAL.BookRepo;
using DAL.Model;

namespace REST_API.SOAP
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public string GenerateAndSearchBooksXml(string searchTerm)
        {
            var books = _bookRepository.getAll();
            var xmlFilePath = GenerateBooksXml(books);

            var searchResults = SearchBooksXml(xmlFilePath, searchTerm);

            return searchResults;
        }

        private string GenerateBooksXml(IEnumerable<Book> books)
        {
            var xmlFilePath = Path.Combine(Path.GetTempPath(), "books.xml");

            var xml = new XDocument(
                new XElement("Books",
                    from book in books
                    select new XElement("Book",
                        new XElement("Id", book.Id),
                        new XElement("Title", book.Title),
                        new XElement("Author", book.Author),
                        new XElement("OriginalLanguage", book.OriginalLanguage),
                        new XElement("FirstPublished", book.FirstPublished),
                        new XElement("ApproximateSales", book.ApproxSalesInMillions),
                        new XElement("Genre", book.Genre)
                    )
                )
            );

            xml.Save(xmlFilePath);
            return xmlFilePath;
        }

        private string SearchBooksXml(string xmlFilePath, string searchTerm)
        {
            var xml = XDocument.Load(xmlFilePath);

            var results = xml.XPathSelectElements($"//Book[contains(translate(Title, 'ABCDEFGHIJKLMNOPQRSTUVWXYZ', 'abcdefghijklmnopqrstuvwxyz'), '{searchTerm.ToLower()}')]");

            if (results.Any())
            {
                var searchResultsXml = new XDocument(new XElement("SearchResults", results));

                return searchResultsXml.ToString();
            }
            else
            {
                return "No results found";
            }
        }
    }
}
