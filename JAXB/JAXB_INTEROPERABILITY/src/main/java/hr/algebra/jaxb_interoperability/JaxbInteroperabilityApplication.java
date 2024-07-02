package hr.algebra.jaxb_interoperability;

import com.sun.net.httpserver.HttpExchange;
import com.sun.net.httpserver.HttpHandler;
import com.sun.net.httpserver.HttpServer;
import jakarta.xml.bind.JAXBContext;
import jakarta.xml.bind.JAXBException;
import jakarta.xml.bind.Unmarshaller;
import org.xml.sax.SAXException;

import javax.xml.XMLConstants;
import javax.xml.transform.stream.StreamSource;
import javax.xml.validation.Schema;
import javax.xml.validation.SchemaFactory;
import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.InetSocketAddress;

public class JaxbInteroperabilityApplication {

	public static void main(String[] args) throws Exception {
		int serverPort = 8001;

		HttpServer server = HttpServer.create(new InetSocketAddress(serverPort), 0);

		server.createContext("/validate", new ValidationHandler());

		server.start();

		System.out.println("Server started on port " + serverPort);
	}

	static class ValidationHandler implements HttpHandler {

		@Override
		public void handle(HttpExchange exchange) throws IOException {
			try {
				String xsdFilePath = "books.xsd";
				InputStream xsdInputStream = getClass().getClassLoader().getResourceAsStream(xsdFilePath);
				if (xsdInputStream == null) {
					throw new IOException("XSD file not found: " + xsdFilePath);
				}

				// Create JAXBContext and Unmarshaller
				JAXBContext jaxbContext = JAXBContext.newInstance(SearchResults.class);
				Unmarshaller unmarshaller = jaxbContext.createUnmarshaller();

				// Set schema for validation
				SchemaFactory schemaFactory = SchemaFactory.newInstance(XMLConstants.W3C_XML_SCHEMA_NS_URI);
				Schema schema = schemaFactory.newSchema(new StreamSource(xsdInputStream));
				unmarshaller.setSchema(schema);

				// Read XML from request and unmarshal
				InputStream requestBody = exchange.getRequestBody();
				SearchResults searchResults = (SearchResults) unmarshaller.unmarshal(requestBody);

				// If unmarshalling succeeds, XML is valid
				String response = "XML is valid against XSD.\n";
				exchange.sendResponseHeaders(200, response.getBytes().length);
				OutputStream responseBody = exchange.getResponseBody();
				responseBody.write(response.getBytes());
				responseBody.close();

			} catch (Exception e) {
				// Extract the first line of the exception message
				String errorMessage = "XML validation error: " + e.getCause();
				exchange.sendResponseHeaders(400, errorMessage.getBytes().length);
				OutputStream responseBody = exchange.getResponseBody();
				responseBody.write(errorMessage.getBytes());
				responseBody.close();
				e.printStackTrace();
			}
		}
	}
}
