//This application is written by Syed Akbar at August 17, 2019

using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace maze_api_caller
{
    class Program
    {
        static void Main()
        {
            foreach(var mazeFile in MazeChallance())
                ExecuteAction(mazeFile, new HttpClient(),PostFile);
        }


        private static IEnumerable<string> MazeChallance()
        {
            yield return "maze1.txt";
            yield return "maze2.txt";
            yield return "maze3.txt";
        }

        private static void ExecuteAction( string fileName, HttpClient client, Action<string, HttpClient> apicall)
        {
            Console.WriteLine ($"Execution of {apicall.Method.Name} with file name {fileName} started at {DateTime.Now} ");
            apicall(fileName, client);
            Console.WriteLine ($"Execution of {apicall.Method.Name} with file name {fileName} completed at {DateTime.Now} ");
        }

        private static void PostFile(string mazeFileName, HttpClient client)
        {                               
            string[] data = File.ReadAllLines(mazeFileName);
            new ConsumeMazeAPI(client).SolveMazeChallange(data,(x) => {Console.WriteLine(x);} ).GetAwaiter().GetResult();
        }

    }

    public class MazeSolution {
        public int steps { get; set; }
        public int solution { get; set; }
    } 

    public class ConsumeMazeAPI {

        private readonly HttpClient client;
        public ConsumeMazeAPI(HttpClient httpclient) {
                client = httpclient;
        }

        public  async Task SolveMazeChallange(string [] mazeInput, Action<string> displyResult)
        {
            
            client.BaseAddress = new Uri("http://localhost:8080/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {                
                var solvedMaze = await PostMazeArryAsync(mazeInput, client);   
                displyResult(solvedMaze);     
                        
            
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

        public static async Task<string> PostMazeArryAsync(string [] mazeInput, HttpClient client)
        {
            var serializedProduct = JsonConvert.SerializeObject(mazeInput);
            var content = new StringContent(serializedProduct, Encoding.UTF8, "application/json");                     
            var response = await client.PostAsync("api/maze/solveMaze", content);
            var result =  JsonConvert.DeserializeObject(await response.Content.ReadAsStringAsync());                                                        
            return  result.ToString();
        }
                                
    }
}