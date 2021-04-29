using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Lib
{
    class XMLtoObject
    {
        public Users User { get; set; }
        public void ReadXML()
        {
            // Now we can read the serialized book ...  
             XmlSerializer reader = new XmlSerializer(typeof(Users));
            StreamReader file = new System.IO.StreamReader(
                @"C:\Users\Administrator\Source\Repos\Demo_AD-DS_-_AD-LDS\Testing\xmlData\dummydata.xml");
            Users overview = (Users)reader.Deserialize(file);
            file.Close();

            Console.WriteLine(overview);

        }

        public void WriteXML()
        {
            // First write something so that there is something to read ...  
            var user = new Users { UserData = new UserData { FirstName = "Anakin", LastName = "Delabelle", Email = "anakin.delabelle@student.ehb.be", Role = "student" } };
            var writer = new XmlSerializer(typeof(Users));
            var wfile = new StreamWriter(@"C:\Users\Administrator\Source\Repos\Demo_AD-DS_-_AD-LDS\Testing\xmlData\SerializationOverview.xml");
            writer.Serialize(wfile, user);
            wfile.Close();
        }

        public static void Main(string[] args)
        {
            XMLtoObject x = new XMLtoObject();
           
            x.WriteXML();
        }
    }
}
