using System;
using System.Collections.Generic;
using System.Text;

namespace LDAP_Testing
{
    public class Users
    {
        public string DisplayName { get; set; }

        public override string ToString()
        {
            return $"Account Name: {DisplayName}";
        }
    }   
}
