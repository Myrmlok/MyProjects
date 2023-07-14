using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using Registration;
namespace Registration
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    
    public partial class MainWindow : Window
    {
        static TcpClient client;
        static string UserName;
        static string USerPassword;
        static TextBlock ButtonLogOut=new TextBlock();
        static TextBlock ButtonPassword=new TextBlock();
        public MainWindow()
        {
            InitializeComponent();
            ButtonLogOut.Text= "Log out";
            ButtonPassword.Text = "Change Password";
            client = new TcpClient();
            client.Connect(Dns.GetHostName(), 8888);

        }
        
        private async void Button_Click_Register(object sender, RoutedEventArgs e)
        {
            Button button = sender as Button;
            string ButtonContext = button.DataContext as string;
            if (ButtonContext == null)
            {
                return;
            }
            
            string RegName="";
            string RegPassword = "";
            string Command="";
            string StrError="";
            if (ButtonContext == "IsRegister")
            {
                if (RPassword.Text.Trim() == "" || RLogin.Text.Trim() == "")
                {
                    MessageBox.Show("Поля обязательны для заполнения");
                    return;
                }
                if (RPassword.Text != RRepeatPassword.Text)
                {
                    MessageBox.Show("Пароли не совпадают");
                    return;
                }
                RegName = RLogin.Text;
                RegPassword = RPassword.Text;
                Command = "AddUser";
                StrError = "Пользователь с таким именем уже существет";
            }
            if (ButtonContext == "IsLogIn")
            {
                if (LogInLogin.Text.Trim() == "" || LogInPassword.Text.Trim() == "")
                {
                    MessageBox.Show("Поля обязательны для заполнения");
                    return;
                }
                RegName = LogInLogin.Text;
                RegPassword = LogInPassword.Text;
                Command = "LogInUser";
                StrError = "Такого пользователь нет";
            }
            if(ButtonContext == "IsChangePassword")
            {
                if (ChnewPassword.Text.Trim() == "")
                {
                    MessageBox.Show("Поля обязательны для заполнения");
                    return;
                }
                if (ChnewPassword.Text != ChNewRepeatPassword.Text)
                {
                    MessageBox.Show("Пароли не совпадают");
                    return;
                }
                RegName = UserName;
                RegPassword = ChnewPassword.Text;
                Command = "ChangePassword";
                StrError = "Не удалось поменять пароль";
            }
            ParsedJson parseFromClient = new ParsedJson()
            {
                RequestAndAnswer = Command,
                UserName = RegName,
                Password = RegPassword,
                CommandExecuted = false
               
            };
            WriteStream(parseFromClient);
            while (true)
            {
                await Task.Yield();
                parseFromClient =  JsonSerializer.Deserialize<ParsedJson>(await StreamRead(client.GetStream()));
                if (parseFromClient.RequestAndAnswer == "IsAnswer") 
                   break;
            }
            if (parseFromClient.CommandExecuted)
            {
                if (ButtonContext != "IsChangePassword")
                {
                    ComboboxReg.Items.Remove(ButtonLogIn);
                    ComboboxReg.Items.Remove(ButtonReg);
                    ComboboxReg.Items.Add(ButtonLogOut);
                    ComboboxReg.Items.Add(ButtonPassword);
                }
                ComboboxReg.SelectedItem = ButtonPassword;
                UserName = RegName;
                USerPassword = RegPassword;
                Login.Text = "Name:" + UserName;
                if(ButtonContext== "IsChangePassword")
                {
                    MessageBox.Show("Пароль успешно изменён");
                    ChnewPassword.Text = "";
                    ChNewRepeatPassword.Text = "";
                }
            }
            else
            {
                MessageBox.Show(StrError);
            }
     
        }
        static void WriteStream(ParsedJson parseFromClient)
        {
            JsonSerializer.SerializeAsync<ParsedJson>(client.GetStream(), parseFromClient);
            
        }
        public void IsConnected()
        {
            if (client.Connected == false)
            {
                MessageBox.Show("Не удалось подключиться к серверу");
                Close();
            }
        }
        static async Task<string> StreamRead(NetworkStream stream)
        {
            string FinalResult = "";
            while (true)
            {
                await Task.Yield();
                byte[] recieve = new byte[1024];
                var byteCount = await stream.ReadAsync(recieve, 0, recieve.Length);
                if (byteCount < 1024)
                {
                    FinalResult += Encoding.UTF8.GetString(recieve, 0, byteCount);

                    break;
                }
                while (byteCount > 0)
                {
                    FinalResult += Encoding.UTF8.GetString(recieve, 0, byteCount);
                    byteCount = await stream.ReadAsync(recieve, 0, recieve.Length);
                    if (byteCount < 1024)
                    {
                        break;
                    }
                }
            }
            FinalResult = FinalResult.Substring(0, FinalResult.IndexOf('}') + 1);
            return FinalResult;
        }
        private void ButtonPassword_Click()
        {
            Register.Visibility = Visibility.Collapsed;
            LogIn.Visibility = Visibility.Collapsed;
            ChangedPassword.Visibility = Visibility.Visible;
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            TextBlock textBlock =ComboboxReg.SelectedItem as TextBlock;
            if (textBlock == null)
            {
                return;
            }
            if (textBlock.Text == "Register")
            {
                if (Register != null)
                {
                    Register.Visibility = Visibility.Visible;
                    LogIn.Visibility = Visibility.Collapsed;
                    ChangedPassword.Visibility = Visibility.Collapsed;
                }
            }
            if(textBlock.Text== "Log in")
            {
                Register.Visibility = Visibility.Collapsed;
                LogIn.Visibility = Visibility.Visible;
                ChangedPassword.Visibility = Visibility.Collapsed;
            }
            if(textBlock.Text== "Change Password")
            {
                ButtonPassword_Click();
            }
            if (textBlock.Text== "Log out")
            {
                ComboboxReg.Items.Remove(ButtonPassword);
                ComboboxReg.Items.Remove(ButtonLogOut);
                ComboboxReg.Items.Add(ButtonLogIn);
                ComboboxReg.Items.Add(ButtonReg);
                
                UserName = "";
                USerPassword = "";
                Login.Text = "";
                ComboboxReg.SelectedItem = ButtonReg;
            }
        }
    }
}
class ParsedJson
{
    public string RequestAndAnswer { get; set; }
    public bool CommandExecuted { get; set; }
    public string UserName { get; set; }
    public string Password { get; set; }
}


