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
using System.Threading;
using System.Net;
using System.IO;
using System.Text.Json;
using MVMM;
namespace WpfApp6
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
         public  MainWindow()
        {
            InitializeComponent();
            GetWeather();
        }
        public async void GetWeather()
        {
           
            using (var client = new WebClient())
            {
                List<TextBlock> textblocksDayofWeek = new List<TextBlock>()
                {
                    TextBlockDayOfWeek1,
                    TextBlockDayOfWeek2,
                    TextBlockDayOfWeek3,
                    TextBlockDayOfWeek4,
                    TextBlockDayOfWeek5,
                    TextBlockDayOfWeek6,
                    TextBlockDayOfWeek7
                };
                List<TextBlock> textBlocksTempday = new List<TextBlock>()
                {
                    tempday1,
                    tempday2,
                    tempday3,
                    tempday4,
                    tempday5,
                    tempday6,
                    tempday7
                };
                List<TextBlock> textBlocksTempnight = new List<TextBlock>()
                {
                    tempnight1,
                    tempnight2,
                    tempnight3,
                    tempnight4,
                    tempnight5,
                    tempnight6,
                    tempnight7
                };
                var lat = 47.2313f;
                var lon = 39.7233f;
         
                var units = "metric";
                var dataweather = client.DownloadString($@"https://api.openweathermap.org/data/2.5/onecall?lat={lat}&lon={lon}&units={units}&exclude=minutely,alerts&appid=b46216ac85bfd86207ceabc0e5de7260");
                var ParseWather = JsonSerializer.Deserialize<Root>(dataweather);
                textblockFirstTemp.Text =Convert.ToInt32( ParseWather.daily[0].temp.max).ToString()+"C";
                BitmapImage bitmapImage1=new BitmapImage(new Uri($@"https://openweathermap.org/img/wn/{ParseWather.daily[0].weather[0].icon}.png"));
                ImageFirts.Source = bitmapImage1;
                for (int i = 0; i < textblocksDayofWeek.Count; i++)
                {
                    var dayOfWeek = DateTimeOffset.FromUnixTimeSeconds(ParseWather.daily[i].dt).DayOfWeek;
                    textblocksDayofWeek[i].Text = dayOfWeek.ToString();
                    
                }
                for (int i = 0; i < ParseWather.daily.Count - 1; i++)
                {
                    textBlocksTempday[i].Text = (Convert.ToInt32(ParseWather.daily[i].temp.max)).ToString() + "C";
                    textBlocksTempnight[i].Text = (Convert.ToInt32(ParseWather.daily[i].temp.min)).ToString() + "C";
                    BitmapImage bitmapImage = new BitmapImage(new Uri($@"https://openweathermap.org/img/wn/{ParseWather.daily[i].weather[0].icon}.png"));
                    Image image = boxImage.Children[i] as Image;
                    image.Source = bitmapImage;
                }

                List<string> hours=new List<string>();
                List<int> temps=new List<int>();
                List<BitmapImage> Icons=new List<BitmapImage>();
                for (int i = 0; i <ParseWather.hourly.Count; i++)
                {
                    hours.Add( DateTimeOffset.FromUnixTimeSeconds(ParseWather.hourly[i].dt + ParseWather.timezone_offset).ToString("HH"));
                    temps.Add( Convert.ToInt32(ParseWather.hourly[i].temp));
                    BitmapImage bitmapImage = new BitmapImage(new Uri($@"https://openweathermap.org/img/wn/{ParseWather.hourly[i].weather[0].icon}.png"));
                    Icons.Add ( bitmapImage);
                }
                ViewModal viewModal = new ViewModal(hours, temps, Icons);
                DataContext = viewModal;
                TempLike.Text = $"It feels like It feels like {Convert.ToInt32( ParseWather.hourly[0].temp)}";
                System.Windows.Threading.DispatcherTimer dispatcherTimer = new System.Windows.Threading.DispatcherTimer();
                dispatcherTimer.Tick += new EventHandler(dispatcherTimer_Tick);
                TextBlockTimer.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();
                dispatcherTimer.Interval = new TimeSpan(0, 0, 10);
                dispatcherTimer.Start();
                
            }
            void dispatcherTimer_Tick(object sender, EventArgs e)
            {
                TextBlockTimer.Text = DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString();

            }

        }
        public class DetectRegionFromAPI
        {
            public float lat
            {
                get;set;
            }
            public float lon
            {
                get;
                set;
            }
        }
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class Current
        {
            public int dt { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
            public double temp { get; set; }
            public double feels_like { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double dew_point { get; set; }
            public int uvi { get; set; }
            public int clouds { get; set; }
            public int visibility { get; set; }
            public double wind_speed { get; set; }
            public int wind_deg { get; set; }
            public int wind_gust { get; set; }
            public List<Weather> weather { get; set; }
        }

        public class Daily
        {
            public int dt { get; set; }
            public int sunrise { get; set; }
            public int sunset { get; set; }
            public int moonrise { get; set; }
            public int moonset { get; set; }
            public double moon_phase { get; set; }
            public Temp temp { get; set; }
            public FeelsLike feels_like { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double dew_point { get; set; }
            public double wind_speed { get; set; }
            public int wind_deg { get; set; }
            public double wind_gust { get; set; }
            public List<Weather> weather { get; set; }
            public int clouds { get; set; }
            public double pop { get; set; }
            public double uvi { get; set; }
            public double? rain { get; set; }
        }

        public class FeelsLike
        {
            public double day { get; set; }
            public double night { get; set; }
            public double eve { get; set; }
            public double morn { get; set; }
        }

        public class Hourly
        {
            public int dt { get; set; }
            public double temp { get; set; }
            public double feels_like { get; set; }
            public int pressure { get; set; }
            public int humidity { get; set; }
            public double dew_point { get; set; }
            public double uvi { get; set; }
            public int clouds { get; set; }
            public int visibility { get; set; }
            public double wind_speed { get; set; }
            public int wind_deg { get; set; }
            public double wind_gust { get; set; }
            public List<Weather> weather { get; set; }
            public double pop { get; set; }
            public Rain rain { get; set; }
        }

        public class Rain
        {
     
            public double _1h { get; set; }
        }

        public class Root
        {
            public double lat { get; set; }
            public double lon { get; set; }
            public string timezone { get; set; }
            public int timezone_offset { get; set; }
            public Current current { get; set; }
            public List<Hourly> hourly { get; set; }
            public List<Daily> daily { get; set; }
        }

        public class Temp
        {
            public double day { get; set; }
            public double min { get; set; }
            public double max { get; set; }
            public double night { get; set; }
            public double eve { get; set; }
            public double morn { get; set; }
        }

        public class Weather
        {
            public int id { get; set; }
            public string main { get; set; }
            public string description { get; set; }
            public string icon { get; set; }
        }

    }
}
