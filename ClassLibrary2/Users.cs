using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Lib
{
    [XmlRoot("user")]
    public class Users
    {
        [XmlElement("header")]
        public MetaData MetaData { get; set; }
        [XmlElement("body")]
        public UserData UserData { get; set; }

        public Users()
        {
            UserData = new UserData { FirstName = "", LastName = "", Email = "", Role = "" };
        }

        public override string ToString()
        {
            return $"Account Name: {UserData.FirstName} {UserData.LastName} - {UserData.Email} - {UserData.Role}";
        }
    }

    public class MetaData
    {
        [XmlElement("UUID")]
        public string UUIDMaster { get; set; }
        [XmlElement("method")]
        public CRUDMethode Methode { get; set; }
        [XmlElement("origin")]
        public string Origin { get; set; }
        [XmlElement("timestamp")]
        public DateTime TimeStamp { get; set; } 
    }

    public class UserData
    {
        [XmlElement("firstname")]
        public string FirstName { get; set; }
        [XmlElement("lastname")]
        public string LastName { get; set; }
        [XmlElement("email")]
        public string Email { get; set; }
        [XmlElement("role")]
        public string Role { get; set; }
    }

    public enum CRUDMethode
    {
        CREATE,UPDATE,DELETE
    }
}
