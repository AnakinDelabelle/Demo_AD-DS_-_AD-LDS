using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Lib;

namespace TestENV
{
    public class XMLtoObject
    {
        public Users ReadandValidateXML(string xml)
        {
            var schema = new XmlSchemaSet();
            var xmlDoc = XDocument.Parse(xml, LoadOptions.SetLineInfo);

            schema.Add("", @"C:\Users\Administrator\Source\Repos\AnakinDelabelle\Demo_AD-DS_-_AD-LDS\TestENV\xmlData\xsdcontrole.xsd"); //Can change

            xmlDoc.Validate(schema, (sender, e) =>
            {
                Debug.WriteLine("XML is ongeldig");
                throw new Exception();
            });

            Debug.WriteLine("XML is geldig");
            var serializer = new XmlSerializer(typeof(Users));
            var reader = new StringReader(xml);
            var user = (Users)serializer.Deserialize(reader);

            return user;
        }

        public string WriteXML(Users user)
        {
            // First write something so that there is something to read ...  
            //var user = new Users { UserData = new UserData { FirstName = "Anakin", LastName = "Delabelle", Email = "anakin.delabelle@student.ehb.be", Role = "student" } };
            var serializer = new XmlSerializer(typeof(Users));
            var writer = new StringWriter();

            serializer.Serialize(writer, user);
            Debug.WriteLine(writer.ToString());

            return writer.ToString();
        }
    }
}
