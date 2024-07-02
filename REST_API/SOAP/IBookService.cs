using System.ServiceModel;

namespace REST_API.SOAP
{
    [ServiceContract]
    public interface IBookService
    {
        [OperationContract]
        string GenerateAndSearchBooksXml(string searchTerm);
    }
}
