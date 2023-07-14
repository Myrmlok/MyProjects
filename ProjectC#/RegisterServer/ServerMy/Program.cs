using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Dapper;
using System.Data.SqlClient;
namespace ServerMy
{
    internal class Program
    {
        static TcpListener TcpListener = new TcpListener(IPAddress.Any, 8888);
        static string Connectionstring=@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=users;Integrated Security=True;Connect Timeout=30;";
        static async Task Main(string[] args)
        {
            TcpListener.Start();
            while (true)
            {
                await Task.Yield();
                TcpClient client = await TcpListener.AcceptTcpClientAsync();
                await ListenForData(client);
            }
        }
        static async Task ListenForData(TcpClient client)
        {
            while (client.Connected)
            {
                try
                {
                    await Task.Yield();
                    var stream = client.GetStream();
                    var ParsedJson =  JsonSerializer.Deserialize<ParsedJson>(await StreamRead(stream));
                    Console.WriteLine(ParsedJson.RequestAndAnswer);
                    if (ParsedJson.RequestAndAnswer == "test")
                    {
                        Console.WriteLine("test");
                    }
                    if (ParsedJson.RequestAndAnswer == "AddUser")
                    {
                        await SetAnswerAsync(await AddUser(ParsedJson), client);
                    }
                    if (ParsedJson.RequestAndAnswer == "LogInUser")
                    {
                        await SetAnswerAsync(await LogInUserAsync(ParsedJson), client);
                    }
                    if (ParsedJson.RequestAndAnswer == "ChangePassword")
                    {
                        await SetAnswerAsync(await ChangePasswordAsync(ParsedJson), client);
                    }
                    

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
        static async Task<bool> AddUser(ParsedJson parsedJson)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Connectionstring))
            {

                var user = await sqlConnection.QueryFirstOrDefaultAsync<User>($"Select * From Users where [UserName]='{parsedJson.UserName}'");
                if (user != null)
                {
                    return false;
                }
                await sqlConnection.ExecuteAsync($"Insert into Users ([UserName],[Password]) values('{parsedJson.UserName}','{parsedJson.Password}')");
                return true;
            }
        }
        static async Task<bool> ChangePasswordAsync(ParsedJson parsedJson)
        {
            using (SqlConnection sqlConnection = new SqlConnection(Connectionstring))
            {

                var user = await sqlConnection.QueryFirstOrDefaultAsync<User>($"Select  * From Users where UserName='{parsedJson.UserName}'");
                if (user == null)
                {
                    return false;
                }
                await sqlConnection.ExecuteAsync($"Update Users set Password='{parsedJson.Password}' where UserName='{parsedJson.UserName}'");
                return true;
            }
        }
        static async Task<bool> LogInUserAsync(ParsedJson parsedJson)
        {
            using(SqlConnection sqlConnection=new SqlConnection(Connectionstring))
            {
                var user = await sqlConnection.QueryFirstOrDefaultAsync<User>($"Select * From Users where UserName='{parsedJson.UserName}'");
                if (user != null)
                {
                    return true;
                }
                return false;
            }
        }
        static async Task SetAnswerAsync(bool commandExecuted,TcpClient client)
        {
           await JsonSerializer.SerializeAsync<ParsedJson>(client.GetStream(), new ParsedJson() { CommandExecuted=commandExecuted,RequestAndAnswer="IsAnswer"});
           
        }
        static async Task<string> StreamRead(NetworkStream stream)
        {
            string FinalResult = "";
            while (true)
            {
                await Task.Yield();
                byte[] recieve = new byte[1024];
                var byteCount =await stream.ReadAsync(recieve, 0, recieve.Length);
                if (byteCount < 1024)
                {
                    FinalResult += Encoding.UTF8.GetString(recieve, 0, byteCount);

                    break;
                }
                while (byteCount > 0)
                {
                    if (byteCount < 1024)
                    {
                        break;
                    }
                    byteCount = await stream.ReadAsync(recieve, 0, recieve.Length);
                }

            }
            FinalResult = FinalResult.Substring(0, FinalResult.IndexOf('}') + 1);
            return FinalResult;
        }
    }
    class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    class ParsedJson
    {
        public string RequestAndAnswer { get; set; }
        public bool CommandExecuted { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
