using Dapper;
using Microsoft.Extensions.Configuration;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;

namespace WebApplication1
{
    public static class Extensions
    {
        private static int _ErrorRetry = 10;

        private static string _key = "Ajduosn4021";

        
        public static string FirstCharToUpper(this string input) =>
        input switch
        {
            null => throw new ArgumentNullException(nameof(input)),
            "" => throw new ArgumentException($"{nameof(input)} cannot be empty", nameof(input)),
            _ => input.First().ToString().ToUpper() + input.Substring(1)
        };
        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }


        private static string ApplicationExeDirectory()
        {
            var location = System.Reflection.Assembly.GetExecutingAssembly().Location;
            var appRoot = Path.GetDirectoryName(location);

            return appRoot;
        }

        public static IConfigurationRoot GetAppSettings()
        {
            var ambiente = System.Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            string applicationExeDirectory = ApplicationExeDirectory();

            var builder = new ConfigurationBuilder()
            .SetBasePath(applicationExeDirectory)
            .AddJsonFile($"appsettings.{ambiente}.json");

            return builder.Build();
        }


        public static IEnumerable<T> Execute_Query<T>(string query)
        {
            IEnumerable<T> items = null;

            int Contador = 0;

            while (Contador <= _ErrorRetry)
            {
                try
                {
                    using (SqlConnection connection = new SqlConnection(GetAppSettings()["ConnectionString"].ToString().Trim()))
                    {
                        items = connection.Query<T>(query);
                    }

                    Contador = _ErrorRetry + 1;
                }
                catch (Exception ex)
                {
                    if (Contador == _ErrorRetry)
                    {
                        throw ex;
                    }

                    Contador++;
                }
            }

            return items;
        }

        public static decimal ToDecimal(this string value)
        {
            try
            {
                return Convert.ToDecimal(value);
            }
            catch (Exception)
            {
                return 0;
            }
        }


        public static void RestSharExecuteExample()
        {
            var client = new RestClient("https://petstore.swagger.io/v2/pet/findByStatus?status=available");
            client.Timeout = -1;
            var request = new RestRequest(Method.GET);
            IRestResponse response = client.Execute(request);
            Console.WriteLine(response.Content);

        }


    }


}
