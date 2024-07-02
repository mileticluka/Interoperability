using System;
using CookComputing.XmlRpc;

namespace FRONTEND.Data.XML_RPC;

public interface IDHMZXmlRpc : IXmlRpcProxy
{
    [XmlRpcMethod("get_current_temperature")]
    string GetCurrentTemperature(string city);
}

public class DHMZClient
{
    private readonly IDHMZXmlRpc _proxy;

    public DHMZClient()
    { 
        _proxy = XmlRpcProxyGen.Create<IDHMZXmlRpc>();
        _proxy.Url = "http://localhost:8000";
    }

    public string GetTemperature(string city)
    {

        double temp;
        try
        {

            string response = _proxy.GetCurrentTemperature(city);
            if (Double.TryParse(response,out temp))
            {
                return $"The current temperature in {city} is: {response} °C";
            }
            else
            {
                return response;
            }
        }
        catch (Exception ex)
        {
            return $"Error: {ex.Message}";
        }
    }
}
