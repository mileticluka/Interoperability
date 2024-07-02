using System;
using System.Globalization;
using DAL.Model;

namespace DAL
{
    public class DataContext
    {
        private static DataContext _instance;
        private static readonly object _lock = new object();

        public List<Book> Books { get; set; }

        public List<User> users { get; set; }

        private DataContext()
        {
            Books = new();
            Books.AddRange(LoadBooksFromCsv("Resources/best-selling-books.csv"));

            users = new()
            {
                new User() {username="user",password="user"}
            };
        }

        public static DataContext Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        _instance = new DataContext();
                    }
                }
                return _instance;
            }
        }

        private List<Book> LoadBooksFromCsv(string filePath)
        {
            var books = new List<Book>();
            var lines = File.ReadAllLines(filePath);

            int idIterator = 1;

            foreach (var line in lines.Skip(1))
            {
                var columns = ParseCsvLine(line);
                books.Add(new Book
                {
                    Id = idIterator,
                    Title = columns[0],
                    Author = columns[1],
                    OriginalLanguage = columns[2],
                    FirstPublished = int.Parse(columns[3]),
                    ApproxSalesInMillions = float.Parse(columns[4]),
                    Genre = columns[5]
                });
                idIterator++;
            }
            return books;
        }

        private string[] ParseCsvLine(string line)
        {
            var inQuotes = false;
            var value = string.Empty;
            var values = new List<string>();

            foreach (var c in line)
            {
                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ',' && !inQuotes)
                {
                    values.Add(value);
                    value = string.Empty;
                    continue;
                }

                value += c;
            }

            values.Add(value);
            return values.ToArray();
        }
    }
}

