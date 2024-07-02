from xmlrpc.server import SimpleXMLRPCServer
import requests
from lxml import etree

DEFAULT_CITY = "Zagreb-Maksimir"

def get_current_temperature(city=DEFAULT_CITY):
    try:
        url = "https://vrijeme.hr/hrvatska_n.xml"

        response = requests.get(url)
        if response.status_code != 200:
            return f"Error: Unable to fetch data, status code {response.status_code}"

        tree = etree.fromstring(response.content)

        for element in tree.findall(".//Grad"):
            grad_ime = element.find("GradIme")
            if grad_ime is not None and grad_ime.text == city:
                temp = element.find(".//Podatci/Temp")
                if temp is not None:
                    return temp.text.strip()
                else:
                    return "Error: Temperature data not found for city"

        return f"Error: City {city} not found"

    except Exception as e:
        return f"Error: {str(e)}"


server = SimpleXMLRPCServer(("localhost", 8000))
print("XML-RPC server listening on port 8000...")

server.register_function(get_current_temperature, "get_current_temperature")

server.serve_forever()
