using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WeatherForecast
{
    public partial class Forecast : Window
    {
        #region GlobalVariable
        private WeatherResponse WeatherData { get; set; }
        private MainWindow mainWindow;
        #endregion


        public Forecast(MainWindow mainWindow, WeatherResponse weatherData, string name)
        {
            InitializeComponent();
            WeatherData = weatherData;
            this.mainWindow = mainWindow;
            NameBlock.Text = name;

            if (WeatherData.Hourly != null)
            {
                List<WeatherEntry> weatherEntries = new List<WeatherEntry>(); //making a list of Hourly weather data and give it as a source to grid
                

                for (int i = 0; i < WeatherData.Hourly.Time.Length; i++)
                {
                    if (!DateTime.TryParseExact(WeatherData.Hourly.Time[i], new[] {"yyyy-MM-ddTHH:mm"}, null, System.Globalization.DateTimeStyles.None, out DateTime d) || d<DateTime.Now) // selecting data which are later than DateTime.Now
                    {
                        continue;
                    }

                    weatherEntries.Add(new WeatherEntry
                    {
                        Time = d,
                        Temperature = WeatherData.Hourly.Temperature_2m[i],
                        Precipitation = WeatherData.Hourly.Precipitation[i],
                        PrecipitationProbability = WeatherData.Hourly.PrecipitationProbability[i],
                        Humidity = WeatherData.Hourly.Relative_Humidity_2m[i],
                        WindSpeed = WeatherData.Hourly.Windspeed_10m[i],
                        Pressure = WeatherData.Hourly.Pressure[i]
                    }); //Adding to list HourlyData of Weather

                }

                DataGrid.ItemsSource = new ObservableCollection<WeatherEntry>(weatherEntries); // making a new ObservableCollection to display Data
            }
            else
            {
                MessageBox.Show("No weather Data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        

        public void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            mainWindow.ResetTownBox(); // resets name of TextBox
            mainWindow.Show(); // return to MainWindow
        } // Button for going back to MainWindow 
    }


        public class WeatherEntry // enables making a list of object for DataGrid
    {   
        public DateTime Time { get; set; }

        public float Temperature { get; set; }

        public float Precipitation { get; set; }

        public int Humidity { get; set; }

        public float WindSpeed { get; set; }

        public float PrecipitationProbability { get; set; }

        public float Pressure { get; set; }
    }
}
    