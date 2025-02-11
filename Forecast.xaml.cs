using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace WeatherForecast
{
    public partial class Forecast : Window
    {
        private WeatherResponse Weather { get; set; }
        private MainWindow mainWindow;

        public Forecast(MainWindow mainWindow, WeatherResponse weather, string name)
        {
            InitializeComponent();
            Weather = weather;
            this.mainWindow = mainWindow;
            NameBlock.Text = name;
            if (Weather.Hourly != null)
            {
                List<WeatherEntry> weatherEntries = new List<WeatherEntry>(); //making a list of Hourly weather data and give it as a source to grid
                

                for (int i = 0; i < Weather.Hourly.Time.Length; i++)
                {
                    if (!DateTime.TryParseExact(Weather.Hourly.Time[i], new[] {"yyyy-MM-ddTHH:mm"}, null, System.Globalization.DateTimeStyles.None, out DateTime d) || d<DateTime.Now) // selecting data which are older than DateTime.Now
                        {
                        continue;
                     }

                    weatherEntries.Add(new WeatherEntry
                    {
                        Time = d,
                        Temperature = Weather.Hourly.Temperature_2m[i],
                        Precipitation = Weather.Hourly.Precipitation[i],
                        PrecipitationProbability = Weather.Hourly.PrecipitationProbability[i],
                        Humidity = Weather.Hourly.Relative_Humidity_2m[i],
                        WindSpeed = Weather.Hourly.Windspeed_10m[i],
                        Pressure = Weather.Hourly.Pressure[i]
                    });

                }

                DataGrid.ItemsSource = new ObservableCollection<WeatherEntry>(weatherEntries);
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
        }
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
    