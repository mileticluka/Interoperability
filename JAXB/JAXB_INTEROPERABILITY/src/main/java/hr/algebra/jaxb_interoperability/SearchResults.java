package hr.algebra.jaxb_interoperability;

import hr.algebra.jaxb_interoperability.BookType;
import jakarta.xml.bind.annotation.XmlElement;
import jakarta.xml.bind.annotation.XmlRootElement;
import java.util.List;

@XmlRootElement(name = "SearchResults")
public class SearchResults {

    private List<BookType> books;

    @XmlElement(name = "Book")
    public List<BookType> getBooks() {
        return books;
    }

    public void setBooks(List<BookType> books) {
        this.books = books;
    }
}

