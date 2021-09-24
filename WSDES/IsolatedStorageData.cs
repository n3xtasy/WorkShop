using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WSAPI.Models.User;

namespace WSDES
{
    public class IsolatedStorageData
    {
        public static void Isolate()
        {
            try
            {

                //First get the 'user-scoped' storage information location reference in the assembly
                IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
                //create a stream writer object to write content in the location
                using StreamWriter srWriter = new StreamWriter(new IsolatedStorageFileStream("user-data.isolated", FileMode.Create, isolatedStorage));
                //check the Application property collection contains any values.
                if (App.Current.Properties["email"] != null && App.Current.Properties["password"] != null)
                {
                    //wriet to the isolated storage created in the above code section.
                    srWriter.WriteLine($"{JsonConvert.SerializeObject(new AuthenticateUserViewModel(App.Current.Properties["email"].ToString(), App.Current.Properties["password"].ToString()))}");

                }

                srWriter.Flush();
                srWriter.Close();
            }
            catch (System.Security.SecurityException sx)
            {
                ApplicationMessages.Show(sx.Message);
                throw;
            }
        }
        public static AuthenticateUserViewModel GetAuthenticationData()
        {
            //First get the 'user-scoped' storage information location reference in the assembly
            IsolatedStorageFile isolatedStorage = IsolatedStorageFile.GetUserStoreForAssembly();
            //create a stream reader object to read content from the created isolated location
            StreamReader srReader = new StreamReader(new IsolatedStorageFileStream("user-data.isolated", FileMode.OpenOrCreate, isolatedStorage));

            //Open the isolated storage
            if (srReader == null)
            {
                return null;
            }
            else
            {
                string json = srReader.ReadToEnd();
                
                srReader.Close();
                srReader.Dispose();

                return JsonConvert.DeserializeObject<AuthenticateUserViewModel>(json);
            }
            //close reader
           
        }
    }
}
