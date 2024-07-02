package hr.algebra.jaxb_interoperability;

import javax.xml.bind.annotation.*;

@XmlRootElement(name = "Book")
@XmlAccessorType(XmlAccessType.FIELD)
public class Book {

    @XmlElement(name = "Id")
    private int id;

    @XmlElement(name = "Title")
    private String title;

    @XmlElement(name = "Author")
    private String author;

    @XmlElement(name = "OriginalLanguage")
    private String originalLanguage;

    @XmlElement(name = "FirstPublished")
    private int firstPublished;

    @XmlElement(name = "ApproximateSales")
    private int approximateSales;

    @XmlElement(name = "Genre")
    private String genre;

    // Getters and Setters

    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

    public String getAuthor() {
        return author;
    }

    public void setAuthor(String author) {
        this.author = author;
    }

    public String getOriginalLanguage() {
        return originalLanguage;
    }

    public void setOriginalLanguage(String originalLanguage) {
        this.originalLanguage = originalLanguage;
    }

    public int getFirstPublished() {
        return firstPublished;
    }

    public void setFirstPublished(int firstPublished) {
        this.firstPublished = firstPublished;
    }

    public int getApproximateSales() {
        return approximateSales;
    }

    public void setApproximateSales(int approximateSales) {
        this.approximateSales = approximateSales;
    }

    public String getGenre() {
        return genre;
    }

    public void setGenre(String genre) {
        this.genre = genre;
    }

    @Override
    public String toString() {
        return "Book{" +
                "id=" + id +
                ", title='" + title + '\'' +
                ", author='" + author + '\'' +
                ", originalLanguage='" + originalLanguage + '\'' +
                ", firstPublished=" + firstPublished +
                ", approximateSales=" + approximateSales +
                ", genre='" + genre + '\'' +
                '}';
    }
}
