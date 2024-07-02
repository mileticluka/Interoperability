using System;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using DAL.BookRepo;
using DAL.Model;
using Microsoft.AspNetCore.Mvc;
using Commons.Xml.Relaxng;
using System.Xml.Serialization;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization;

namespace REST_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BookController : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookController(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var books = _bookRepository.getAll();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            var book = _bookRepository.getById(id);
            if (book == null)
            {
                return NotFound(new { Error = "Book not found", ErrorCode = 404 });
            }
            return Ok(book);
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody] Book updatedBook)
        {
            if (updatedBook == null || updatedBook.Id != id)
            {
                return BadRequest(new { Error = "Invalid book data or mismatched ID", ErrorCode = 400 });
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(new { Error = "Invalid book data", Errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage) });
            }

            try
            {
                _bookRepository.UpdateBook(updatedBook);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Error = ex.Message, ErrorCode = 404 });
            }

            return Ok(new { Message = "Book updated successfully", Data = updatedBook });
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            try
            {
                _bookRepository.DeleteBook(id);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { Error = ex.Message, ErrorCode = 404 });
            }

            return Ok(new { Message = "Book deleted successfully" });
        }

        [HttpPost("post-xsd")]
        public IActionResult PostWithXsdValidation([FromBody] XElement xml)
        {
            if (xml == null)
            {
                return BadRequest(new { Error = "XML data is required", ErrorCode = 400 });
            }

            var validationResult = ValidateXSD(xml);
            if (validationResult.IsValid)
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(Book));
                    using (var reader = xml.CreateReader())
                    {
                        if (serializer.Deserialize(reader) is Book book)
                        {
                            _bookRepository.AddBook(book);
                            return Ok(new { Message = "XML validated and book added successfully" });
                        }
                        else
                        {
                            return BadRequest(new { Error = "Failed to deserialize XML to Book object", ErrorCode = 400 });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message, ErrorCode = 400 });
                }
            }
            else
            {
                return BadRequest(new { Error = "XML validation failed against XSD schema", ErrorCode = 400, ValidationErrors = validationResult.ValidationErrors });
            }
        }

        private ValidationResult ValidateXSD(XElement xml)
        {
            try
            {
                var validationErrors = string.Empty;

                XmlDocument doc = new XmlDocument();
                doc.Load(xml.CreateReader());

                XmlSchemaSet schema = new XmlSchemaSet();
                schema.Add("", "Resources/books.xsd");

                doc.Schemas.Add(schema);
                doc.Validate((sender, args) =>
                {
                    validationErrors += args.Message + "\n";
                });

                if (!string.IsNullOrEmpty(validationErrors))
                {
                    return new ValidationResult { IsValid = false, ValidationErrors = validationErrors };
                }
            }
            catch (Exception ex)
            {
                return new ValidationResult { IsValid = false, ValidationErrors = ex.Message };
            }

            return new ValidationResult { IsValid = true };
        }

        [HttpPost("post-rng")]
        public IActionResult PostWithRngValidation([FromBody] XElement xml)
        {
            if (xml == null)
            {
                return BadRequest(new { Error = "XML data is required", ErrorCode = 400 });
            }

            var validationResult = ValidateRNG(xml);
            if (validationResult.IsValid)
            {
                try
                {
                    var serializer = new XmlSerializer(typeof(Book));
                    using (var reader = xml.CreateReader())
                    {
                        if (serializer.Deserialize(reader) is Book book)
                        {
                            _bookRepository.AddBook(book);
                            return Ok(new { Message = "XML validated and book added successfully" });
                        }
                        else
                        {
                            return BadRequest(new { Error = "Failed to deserialize XML to Book object", ErrorCode = 400 });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return BadRequest(new { Error = ex.Message, ErrorCode = 400 });
                }
            }
            else
            {
                return BadRequest(new { Error = "XML validation failed against RNG schema", ErrorCode = 400, ValidationErrors = validationResult.ValidationErrors });
            }
        }

        private ValidationResult ValidateRNG(XElement element)
        {
            var validationErrors = string.Empty;
            XmlReader instance = element.CreateReader();
            XmlReader schema = new XmlTextReader("Resources/books.rng");
            using (RelaxngValidatingReader reader = new RelaxngValidatingReader(instance, schema))
            {
                try
                {
                    while (!reader.EOF)
                    {
                        reader.Read();
                    }
                }
                catch (Exception ex)
                {
                    validationErrors = ex.Message;
                }
            }

            if (!string.IsNullOrEmpty(validationErrors))
            {
                return new ValidationResult { IsValid = false, ValidationErrors = validationErrors };
            }

            return new ValidationResult { IsValid = true };
        }

        public class ValidationResult
        {
            public bool IsValid { get; set; }
            public string ValidationErrors { get; set; }
        }
    }
}
