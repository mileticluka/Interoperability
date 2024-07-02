package hr.algebra.jaxb_interoperability;

import jakarta.xml.bind.annotation.XmlElement;

public class BookType {

    private int id;
    private String title;
    private String author;
    private String originalLanguage;
    private int firstPublished;
    private int approximateSales;
    private String genre;

    @XmlElement(name = "Id")
    public int getId() {
        return id;
    }

    public void setId(int id) {
        this.id = id;
    }

    @XmlElement(name = "Title")
    public String getTitle() {
        return title;
    }

    public void setTitle(String title) {
        this.title = title;
    }

}
