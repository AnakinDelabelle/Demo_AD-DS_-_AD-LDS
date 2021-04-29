
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;


namespace Lib
{
    public class Program
    {
        public DirectoryEntry RootOU { get; set; }
        public Connection Connection { get; set; }
        public DirectorySearcher Searcher { get; set; }

        public void Binding(Connection conn)
        {
            Connection = conn;
            //LDAP://localhost:50000/CN=Users,DC=anakin,DC=local  AD LDS Instance
            //LDAP://dc1.anakin.local/CN=Users,DC=anakin,DC=local AD DS Instance

            if (conn == Connection.LOCAL)
            {
                RootOU = new DirectoryEntry             //SSL on AD DS is standard
                    ("LDAP://dc1.anakin.local/CN=Users,DC=anakin,DC=local",
                    "Administrator",
                    "Student1",
                    AuthenticationTypes.Secure
                    );
            }
            if (conn == Connection.LDAP)
            {                                           //SSL connection on port 50001 with ldp.exe works; NOT HERE in C#;
                RootOU = new DirectoryEntry
                    ("LDAP://localhost:50001/CN=Users,CN=Users,DC=anakin,DC=local",
                    "Administrator",
                    "Student1",
                    AuthenticationTypes.Secure
                    );
            }

        }

        public static void Main(string[] args)
        {
            Debug.WriteLine("Start Program");
            Program p = new Program();

            Debug.WriteLine("Start binding with LDAP server");
            p.Binding(Connection.LOCAL);

            Debug.WriteLine("Create Test User");
            //p.CreateUser();

            Debug.WriteLine("Get all users from Active Directory");
            var lists = p.GetADUsers();

            lists.ForEach(x => Debug.WriteLine(x.ToString()));
        }


        public bool CreateUser(string name)
        {
            try
            {
                DirectoryEntry objUser;
                string displayName;
                string user;
                string userName;

                //Refresh the AD LDS object
                //RootOU.RefreshCache();

                //Create user info
                user = $"CN={name}";
                userName = $"{name}@anakin.be";
                displayName = $"{name}";

                //Create User object
                objUser = RootOU.Children.Add(user, "user");
                objUser.Properties["displayName"].Add(displayName);
                objUser.Properties["userPrincipalName"].Add(userName);
                Debug.WriteLine(objUser.Path);
                objUser.CommitChanges();

                Debug.WriteLine("User Creation Succeeded!");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool DeleteUser(string name)
        {
            try
            {
                DirectoryEntry objUser = RootOU.Children.Find($"CN={name.Substring(14)}");
                Console.WriteLine($"{objUser.SchemaClassName}: \"{objUser.Name}\" is found in the Container!");
                RootOU.Children.Remove(objUser);
                Console.WriteLine("User succesfully deleted!");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public bool UpdateUser(string name, string newName)
        {
            try
            {
                Searcher = new DirectorySearcher(RootOU);
                Searcher.Filter = $"(&(objectCategory=Person)(cn={name.Substring(14)}))";

                Searcher.PropertiesToLoad.Add("displayname");
                SearchResult sr = Searcher.FindOne();
                var objUser = sr.GetDirectoryEntry();

                Console.WriteLine($"{objUser.SchemaClassName}: \"{objUser.Name}\" is found in the Container!");

                objUser.Rename("CN="+newName);
                objUser.Properties["displayname"][0] = $"{newName}"; 

                objUser.UsePropertyCache = true;
                objUser.CommitChanges();

                Console.WriteLine("User succesfully updated!");
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public List<Users> GetADUsers()
        {
            try
            {
                List<Users> lstADUsers = new List<Users>();

                Searcher = new DirectorySearcher(RootOU);
                Searcher.Filter = "(&(objectCategory=Person)(objectClass=user))";

                Searcher.PropertiesToLoad.Add("displayname");

                SearchResult result;
                SearchResultCollection resultCol = Searcher.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];

                        if (result.Properties.Contains("displayname"))
                        {
                            Users objSurveyUsers = new Users();
                            //objSurveyUsers.DisplayName = (String) result.Properties["displayname"][0];
                            lstADUsers.Add(objSurveyUsers);

                        }
                    }
                }
                return lstADUsers;
            }

            catch (Exception ex)
            {
                return null;
            }
        }
    }
}