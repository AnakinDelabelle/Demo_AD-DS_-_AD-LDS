
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.DirectoryServices;


namespace LDAP_Testing
{
    public class Program
    {
        public DirectoryEntry RootOU { get; set; }

        public void Binding()
        {
            //LDAP://localhost:50000/CN=Users,DC=anakin,DC=local  AD LDS Instance
            //LDAP://dc1.anakin.local/CN=Users,DC=anakin,DC=local AD DS Instance


            RootOU = new DirectoryEntry
                ("LDAP://localhost:50000/CN=Users,CN=Users,DC=anakin,DC=local",
                    "Administrator",
                    "Student1",
                    AuthenticationTypes.Secure
                );
        }

        public void GetAllUsers()
        {
            SearchResultCollection results;

            DirectorySearcher ds = new DirectorySearcher(RootOU);
            ds.Filter = "(&(objectCategory=User)(objectClass=person))";

            results = ds.FindAll();

            Debug.WriteLine("Get all users");
            Debug.WriteLine(results.Count);
            foreach (SearchResult sr in results)
            {
                //Using the index zero 9à0 is required1
                Debug.WriteLine(sr.Properties["name"][0].ToString());
            }
        }


        public static void Main(string[] args)
        {
            Debug.WriteLine("Start Program");
            Program p = new Program();

            Debug.WriteLine("Start binding with LDAP server");
            p.Binding();

            Debug.WriteLine("Create Test User");
            //p.CreateUser();

            Debug.WriteLine("Get all users from Active Directory");
            var lists = p.GetADUsers();

            lists.ForEach(x => Debug.WriteLine(x.ToString()));
        }


        public void CreateUser()
        {
            DirectoryEntry objUser;
            string displayName;
            string user;
            string userName;

            //Refresh the AD LDS object
            //RootOU.RefreshCache();

            //Create user info
            user = "CN=User3";
            userName = "TestUser3@anakin.be";
            displayName = "Test User3";

            //Create User object
            objUser = RootOU.Children.Add(user, "user");
            objUser.Properties["displayName"].Add(displayName);
            objUser.Properties["userPrincipalName"].Add(userName);
            Debug.WriteLine(objUser.Path);
            objUser.CommitChanges();

            Debug.WriteLine("User Creation Succeeded!");
        }



        public List<Users> GetADUsers()
        {
            try
            {
                List<Users> lstADUsers = new List<Users>();

                DirectorySearcher search = new DirectorySearcher(RootOU);
                search.Filter = "(&(objectCategory=Person)(objectClass=user))";

                search.PropertiesToLoad.Add("displayname");

                SearchResult result;
                SearchResultCollection resultCol = search.FindAll();
                if (resultCol != null)
                {
                    for (int counter = 0; counter < resultCol.Count; counter++)
                    {
                        string UserNameEmailString = string.Empty;
                        result = resultCol[counter];

                        if (result.Properties.Contains("displayname"))
                        {
                            Users objSurveyUsers = new Users();
                            objSurveyUsers.DisplayName = (String) result.Properties["displayname"][0];
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