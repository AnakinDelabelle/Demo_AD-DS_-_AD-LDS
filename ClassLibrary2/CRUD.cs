
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;


namespace Lib
{
    public class CRUD
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
                    ("LDAP://AD-S1-Desiderius-Hogeschool.desideriushogeschool.be/CN=Users,DC=desideriushogeschool,DC=be",
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
            Debug.WriteLine("Start CRUD");
            CRUD p = new CRUD();

            Debug.WriteLine("Start binding with LDAP server");
            p.Binding(Connection.LOCAL);

            Debug.WriteLine("Create Test User");
            p.CreateUser(new Users { UserData = new UserData { FirstName = "Test", LastName = "123" } });
        }


        public bool CreateUser(Users user)
        {


            try
            {   //Create user info
                DirectoryEntry objUser;
                var objName = $"CN={user.UserData.FirstName} {user.UserData.LastName}";
                var name = $"{user.UserData.FirstName} {user.UserData.LastName}";

                //Check in AD and UUID for duplicates
                Searcher = new DirectorySearcher(RootOU);
                Searcher.Filter = $"(&(objectCategory=Person)({objName}))";

                if (Searcher.FindAll().Count != 0)
                {
                    throw new Exception();
                }

                //Create User object
                //Set: GivenName, Name, CN,
                //Not Set:  SN, sAMAccountName, Email, Role, DisplayName
                objUser = RootOU.Children.Add(objName, "user");
                objUser.Properties["displayName"].Add(name);
                objUser.Properties["sn"].Add(user.UserData.LastName);
                objUser.Properties["mail"].Add(user.UserData.Email);
                objUser.Properties["role"].Add(user.UserData.Role);
                objUser.Properties["sAMAccountName"].Add($"{user.UserData.FirstName.ToLowerInvariant()}.{user.UserData.LastName.ToLowerInvariant()}");
               

                Debug.WriteLine(objUser.Path);
                objUser.CommitChanges();

                Debug.WriteLine("User Creation Succeeded!");
                return true;
            }
            catch (Exception)
            {
                throw new Exception();
                
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

                throw new Exception();
            }
        }

        public bool UpdateUser(Users user)
        {
            try
            {
                Searcher = new DirectorySearcher(RootOU);
                Searcher.Filter = $"(&(objectCategory=Person)(cn={user.UserData.FirstName} {user.UserData.LastName}))";

                Searcher.PropertiesToLoad.Add("displayname");
                Searcher.PropertiesToLoad.Add("mail");
                Searcher.PropertiesToLoad.Add("role");
                Searcher.PropertiesToLoad.Add("samaccountname");
                Searcher.PropertiesToLoad.Add("sn");
                Searcher.PropertiesToLoad.Add("cn");
                Searcher.PropertiesToLoad.Add("givenname");
                Searcher.PropertiesToLoad.Add("name");
                SearchResult sr = Searcher.FindOne();
                var objUser = sr.GetDirectoryEntry();

                Console.WriteLine($"{objUser.SchemaClassName}: \"{objUser.Name}\" is found in the Container!");

                var objName = $"CN={user.UserData.FirstName} {user.UserData.LastName}";
                var name = $"{user.UserData.FirstName} {user.UserData.LastName}";

                objUser.Rename(objName);
                objUser.Properties["name"][0] = name;
                objUser.Properties["givenname"][0] = user.UserData.FirstName;
                objUser.Properties["displayName"][0] = name;
                objUser.Properties["sn"][0] = user.UserData.LastName;
                objUser.Properties["mail"][0] = user.UserData.Email;
                objUser.Properties["role"][0] = user.UserData.Role;
                objUser.Properties["sAMAccountName"][0] = $"{user.UserData.FirstName.ToLowerInvariant()}.{user.UserData.LastName.ToLowerInvariant()}";


                objUser.UsePropertyCache = true;
                objUser.CommitChanges();

                Console.WriteLine("User succesfully updated!");
                return true;
            }
            catch (Exception)
            {

                throw new Exception();
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
                            objSurveyUsers.UserData.FirstName = (String) result.Properties["displayname"][0];
                            lstADUsers.Add(objSurveyUsers);

                        }
                    }
                }
                return lstADUsers;
            }

            catch (Exception ex)
            {
                throw new Exception();
            }
        }
    }
}