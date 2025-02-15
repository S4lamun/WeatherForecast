using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace WeatherForecast
{
    public partial class MainWindow : Window
    {
        #region GlobalVariable
        WeatherResponse WeatherData { get; set; }
        new string Name { get; set; }
        List<Town> Towns { get; set; }
        bool TemperatureBool;
        bool SpeedBool;
        #endregion

        public MainWindow()
        {
            InitializeComponent();
            TownBox.Text = "Enter City Name";
            TemperatureBool = false;
            SpeedBool = false;
            Name = string.Empty; 
        }

        #region TownBox
        private void TownBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (TownBox.Text == "Enter City Name")
            {
                TownBox.Text = "";
                TownBox.Foreground = Brushes.Black;
            }
        } // function to change writing inside TownBox when clicked

        private async void TownBox_LostFocus(object sender, RoutedEventArgs e)
        {
            string town = TownBox.Text;
            town = town.Trim();
            if (town.Length < 3)
            {
                MessageBox.Show("Your Input is too short (minimum 4 letters)", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            town = char.ToUpper(town[0]) + town.Substring(1); // making a spot to be proper name of Town

            // Getting City Coordinates
            string geoUrl = $"https://nominatim.openstreetmap.org/search?q={town}&format=json&addressdetails=1"; // URL address of City
            using HttpClient httpClient = new();
            httpClient.DefaultRequestHeaders.Add("User-Agent", "WeatherForecastApp/1.0 (email@example.com)");
            httpClient.Timeout = TimeSpan.FromSeconds(10); // if HTTP is overloaded program will wait 10 sec before throwing exception

            try
            {
                HttpResponseMessage responseGeo = await httpClient.GetAsync(geoUrl);
                responseGeo.EnsureSuccessStatusCode(); // if error it will throw exception
                string jsonStringGeo = await responseGeo.Content.ReadAsStringAsync();
                Towns = JsonSerializer.Deserialize<List<Town>>(jsonStringGeo); // making a list of deserialized towns from json
                ChooseCombo.ItemsSource = new ObservableCollection<Town>(Towns); // making ObservableCollection for ChooseCombo
            }
            catch (Exception ex) // catching Exception when program can't get Data
            {
                MessageBox.Show($"Error gathering weather data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
            }
        } // function to gather data for ChooseCombo

        public void ResetTownBox()
        {
            TownBox.Text = "Enter City Name";
            TownBox.Foreground = Brushes.Black;
        } // function to reset TownBox when user returns to MainWindow
        #endregion

        #region Combo
        private void ChooseCombo_SelectionChanged(object sender, SelectionChangedEventArgs e) // Saving selectedTown from ChooseCombo
        {
            if (ChooseCombo.SelectedItem is Town selectedTown)
            {
                Name = selectedTown.DisplayName;
            }
        }

        public void ResetChooseCombo()
        {
            ChooseCombo.Text = string.Empty;
            ChooseCombo.ItemsSource = null;
            
        }
        #endregion Combo

        #region Buttons
        private async void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(ChooseCombo.Text))
            {
                MessageBox.Show("Please select a city from the list.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(TownBox.Text) || TownBox.Text == "Enter City Name")
            {
                MessageBox.Show("You must enter the name of city", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                return;
            }

            var selectedTown = Towns.FirstOrDefault(t => t.DisplayName == Name);
            // Getting Hourly Data of our City
            if (selectedTown == null)
            {
                MessageBox.Show("Choose proper town!", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.None);
                return;
            }

            string meteoUrl = $"https://api.open-meteo.com/v1/forecast?latitude={selectedTown.Latitude}" +
                              $"&longitude={selectedTown.Longitude}" +
                              $"&hourly=temperature_2m,precipitation,relative_humidity_2m,windspeed_10m,precipitation_probability,pressure_msl&forecast_days=7&timezone=Europe/Berlin";

            try
            {
                using HttpClient httpClient = new();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "WeatherForecastApp/1.0 (email@example.com)");
                httpClient.Timeout = TimeSpan.FromSeconds(10);
                HttpResponseMessage responseMeteo = await httpClient.GetAsync(meteoUrl);
                responseMeteo.EnsureSuccessStatusCode();
                string jsonStringMeteo = await responseMeteo.Content.ReadAsStringAsync();

                WeatherData = JsonSerializer.Deserialize<WeatherResponse>(jsonStringMeteo, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }); // last function makes program avoid case sensitivity issues

                if (WeatherData != null && WeatherData.Hourly != null)
                {
                    
                    Forecast forecastWindow = new(this, WeatherData, Name, TemperatureBool, SpeedBool);

                    this.Hide(); // hiding MainWindow
                    forecastWindow.ShowDialog();

                    this.Show(); // After closing Forecast, returning to MainWindow
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
        } // function to find WeatherData using Latitude and Longitude of chosen town

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        } // Shutting down application
        #endregion
       
        #region CheckBox
        private void TemperatureCheck_Checked(object sender, RoutedEventArgs e)
        {
            TemperatureBool = true;
        }

        private void SpeedCheck_Checked(object sender, RoutedEventArgs e)
        {
            SpeedBool = true;
        }

        private void TemperatureCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            TemperatureBool = false;
        }

        private void SpeedCheck_Unchecked(object sender, RoutedEventArgs e)
        {
            SpeedBool = false;
        }

        public void CheckBoxReset()
        {
            TemperatureCheck.IsChecked = false;
            SpeedCheck.IsChecked = false;

        }
        #endregion
    }
}
