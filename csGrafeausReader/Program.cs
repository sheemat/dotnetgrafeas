using System;
using System.IO;
using System.Net;
using System.Text.Json;


namespace csGrafeausReader
{
       
    class Program
    {
        static string jsonResponse;
        static string grafeasURL= "ec2-3-236-176-96.compute-1.amazonaws.com:8080";

        static string httpCall(string myUrl)
        {
            // Create a request for the URL.
            WebRequest request = WebRequest.Create(myUrl);
            // If required by the server, set the credentials.
            request.Credentials = CredentialCache.DefaultCredentials;

            // Get the response.
            WebResponse response = request.GetResponse();
            // Display the status.
            Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            // Get the stream containing content returned by the server.
            // The using block ensures the stream is automatically closed.
            using (Stream dataStream = response.GetResponseStream())
            {
                // Open the stream using a StreamReader for easy access.
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.
                string responseFromServer = reader.ReadToEnd();
                // Display the content.
                //Console.WriteLine(responseFromServer);

                jsonResponse = responseFromServer;
            }
            response.Close();
            return jsonResponse;

        }

        static void ReadOccs(string projectName)
        {
            string grURL = "http://"+ grafeasURL + "/v1beta1/" + projectName+"/occurrences";
            jsonResponse = httpCall(grURL);
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;
                JsonElement projects = root.GetProperty("occurrences");

                foreach (JsonElement gProject in projects.EnumerateArray())
                {
                    JsonElement gProjName = gProject.GetProperty("name");

                    Console.WriteLine(gProjName);
                }


            }

        }

        static void getResponse()
        {
            // Create a request for the URL.
            string grURL="http://" + grafeasURL + "/v1beta1/projects";
            jsonResponse = httpCall(grURL);
            using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
            {
                JsonElement root = doc.RootElement;
                JsonElement projects = root.GetProperty("projects");

                foreach (JsonElement gProject in projects.EnumerateArray())
                {
                    JsonElement gProjName = gProject.GetProperty("name");

                    Console.WriteLine(gProjName);
                    ReadOccs(gProjName.ToString());
                }

                
            }
            
            
        }



        static void Main(string[] args)
        {
            
            Console.WriteLine("Hello World!");
            getResponse();

        }
    }
}
