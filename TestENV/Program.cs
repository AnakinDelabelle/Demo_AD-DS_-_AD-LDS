using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lib;

namespace TestENV
{
    class Program
    {
        static void Main(string[] args)
        {
            var user = new Users {
                UserData = new UserData
                {
                    FirstName = "Anakin",
                    LastName = "Delabelle",
                    Email = "anakin.delabelle@student.ehb.be",
                    Role = "student"
                },
                MetaData = new MetaData
                {
                    UUIDMaster = "rando-mstrin-ggene-rated",
                    Methode = CRUDMethode.CREATE,
                    Origin = "CANVAS",
                    TimeStamp = DateTime.Now
                }
            };

            var x = new XMLtoObject();
            
            //Create XML from User Object 
            var xmlString = x.WriteXML(user);
            Debug.WriteLine(xmlString);

            //Create User Object from XML
            var newUser = x.ReadandValidateXML(xmlString);
            Debug.WriteLine(newUser);
        }
    }
}
