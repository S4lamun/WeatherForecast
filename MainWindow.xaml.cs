using System.Net.Http;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WeatherForecast
{

    public partial class MainWindow : Window
    {
        WeatherResponse WeatherData { get; set; }
        private MainWindow mainWindow;
        string Name { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            TownBox.Text = "Enter City Name";
            this.mainWindow = this;

        }


        private void TownBox_GotFocus(object sender, RoutedEventArgs e) 
        {
            if (TownBox.Text == "Enter City Name")
            {
                TownBox.Text = "";
                TownBox.Foreground = Brushes.Black;
            }
        }



        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            string spot = TownBox.Text;
            Name = spot;
            if (string.IsNullOrEmpty(spot) || TownBox.Text == "Enter City Name")
            {
                MessageBox.Show("You must enter the name of city", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }

            //Getting City Coordiantes
            string geoUrl = $"https://nominatim.openstreetmap.org/search?q={spot}&format=json&addressdetails=1"; //URL address of City
            using HttpClient clientGeo = new HttpClient();
            clientGeo.DefaultRequestHeaders.Add("User-Agent", "WeatherForecastApp/1.0 (email@example.com)"); // needed to avoid 403 Error

            try
            {
                HttpResponseMessage responseGeo = await clientGeo.GetAsync(geoUrl);
                responseGeo.EnsureSuccessStatusCode(); // if error it will throw excpetion
                string jsonStringGeo = await responseGeo.Content.ReadAsStringAsync();
                var towns = JsonSerializer.Deserialize<List<Town>>(jsonStringGeo); //making a list of deseralized towns from json 

                if (towns != null && towns.Count > 0)
                {
                    Console.WriteLine($"Latitude: {towns[0].Latitude}, Longitude: {towns[0].Longitude}"); // Getting first search of City

                    // Getting Hourly Data of our City
                    string meteoUrl = $"https://api.open-meteo.com/v1/forecast?latitude={towns[0].Latitude}" + $"&longitude={towns[0].Longitude}" + $"&hourly=temperature_2m,precipitation,relative_humidity_2m,windspeed_10m" + $"&forecast_days=7" + $"&timezone=Europe/Berlin";

                    using HttpClient clientMeteo = new HttpClient();

                    try
                    {
                        HttpResponseMessage responseMeteo = await clientMeteo.GetAsync(meteoUrl);
                        responseMeteo.EnsureSuccessStatusCode();
                        string jsonStringMeteo = await responseMeteo.Content.ReadAsStringAsync();

                        WeatherData = JsonSerializer.Deserialize<WeatherResponse>(jsonStringMeteo, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // last function makes program to avoid size of Letter

                        if (WeatherData != null && WeatherData.Hourly != null)
                        {

                            if (WeatherData != null && WeatherData.Hourly != null)
                            {
                                Forecast forecastWindow = new Forecast(this, WeatherData, Name);

                                this.Hide(); // hiding MainWindow
                                forecastWindow.ShowDialog();

                                this.Show(); //After shutdown of Forecast, returning MainWindow

                            }

                        }
                        else
                        {
                            MessageBox.Show("There is no data for this area!", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                        }
                    }
                    catch (Exception ex)
                    {

                        MessageBox.Show($"Error gathering weather data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                    }
                }
                else
                {

                    MessageBox.Show("There is no data for this localization", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Error gathering geographic data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

       
    }
}
      