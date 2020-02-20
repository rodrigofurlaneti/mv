using System.IO;
using System.Xml.Serialization;

namespace Aplicacao.apipayzen
{
    public static class Serialization
    {
        public static string Serialize<T>(T dataToSerialize)
        {
            var stringwriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringwriter, dataToSerialize);
            return stringwriter.ToString()
                .Replace("<?xml version=\"1.0\" encoding=\"utf-16\"?>", string.Empty)
                .Replace(" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\"", string.Empty);
        }

        public static T LoadFromXmlString<T>(string xmlText)
        {
            var stringReader = new StringReader(xmlText);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
    }
}
