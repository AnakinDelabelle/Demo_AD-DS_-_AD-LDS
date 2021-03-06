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
    public class XMLParser
    {
        public bool ReadXMLOperation(string xml)
        {
            return false;
        }
        public string ReadXMLFiletoString(string path)
        {
            XmlSerializer reader = new XmlSerializer(typeof(Users));
            StreamReader file = new StreamReader(path);
            var xml = WriteXMLfromObject((Users)reader.Deserialize(file));
            file.Close();
            return xml;
        }
        public Users XMLtoObject(string xml)
        {
            var schema = new XmlSchemaSet();
            var xmlDoc = XDocument.Parse(xml, LoadOptions.SetLineInfo);

            schema.Add("", @"C:\Users\Administrator\Source\Repos\AnakinDelabelle\Demo_AD-DS_-_AD-LDS\TestENV\xmlData\xsdcontrole.xsd"); //Can change

            xmlDoc.Validate(schema, (sender, e) =>
            {
                Debug.WriteLine("XML is ongeldig");
            });

            Debug.WriteLine("XML is geldig");
            var serializer = new XmlSerializer(typeof(Users));
            var reader = new StringReader(xml);
            var user = (Users)serializer.Deserialize(reader);

            return user;
        }

        public string WriteXMLfromObject(Users user)
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
