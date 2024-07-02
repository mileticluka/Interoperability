using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace DAL.Model
{
    public class Book
    {
        public int? Id { get; set; }

        [Required(ErrorMessage = "Title is required.")]
        [StringLength(100, ErrorMessage = "Title can't be longer than 100 characters.")]
        [XmlElement(ElementName = "Title")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Author is required.")]
        [StringLength(50, ErrorMessage = "Author can't be longer than 50 characters.")]
        [XmlElement(ElementName = "Author")]

        public string Author { get; set; }

        [Required(ErrorMessage = "Original Language is required.")]
        [StringLength(50, ErrorMessage = "Original Language can't be longer than 50 characters.")]
        [XmlElement(ElementName = "OriginalLanguage")]
        public string OriginalLanguage { get; set; }

        [Range(0, 2024, ErrorMessage = "First Published year must be a valid year.")]
        [XmlElement(ElementName = "FirstPublished")]
        public int FirstPublished { get; set; }

        [Range(0, float.MaxValue, ErrorMessage = "Approximate Sales in Millions must be a positive number.")]
        [XmlElement(ElementName = "ApproximateSales")]
        public float ApproxSalesInMillions { get; set; }

        [Required(ErrorMessage = "Genre is required.")]
        [StringLength(30, ErrorMessage = "Genre can't be longer than 30 characters.")]
        [XmlElement(ElementName = "Genre")]
        public string Genre { get; set; }
    }
}
