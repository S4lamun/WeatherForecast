using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace WeatherForecast
{
    public partial class Forecast : Window
    {
        #region GlobalVariable
        private WeatherResponse WeatherData { get; set; }
        private MainWindow mainWindow;
        bool TemperatureBool;
        bool SpeedBool;
        #endregion

        public Forecast(MainWindow mainWindow, WeatherResponse weatherData, string name, bool temperatureBool, bool speedBool)
        {
            InitializeComponent();
            WeatherData = weatherData;
            this.mainWindow = mainWindow;
            NameBlock.Text = name;
            TemperatureBool = temperatureBool;
            SpeedBool = speedBool;

            UpdateColumnHeaders();
            if (WeatherData.Hourly != null)
            {
                // Create a list of WeatherEntry objects to hold filtered hourly weather data
                List<WeatherEntry> weatherEntries = new List<WeatherEntry>();

                
                for (int i = 0; i < WeatherData.Hourly.Time.Length; i++)
                {
                    // Try parsing the date and check if it's in the future compared to current time
                    if (!DateTime.TryParseExact(WeatherData.Hourly.Time[i], new[] { "yyyy-MM-ddTHH:mm" }, null, System.Globalization.DateTimeStyles.None, out DateTime d) || d < DateTime.Now)
                    {
                        continue; // Skip the data if it's invalid or if the time is in the past
                    }

                    // Add the valid data to the weatherEntries list
                    weatherEntries.Add(new WeatherEntry
                    {
                        Time = d,
                        Temperature = temperatureBool?((WeatherData.Hourly.Temperature[i]*(float)(9.0/5.0))+32):(WeatherData.Hourly.Temperature[i]), 
                        Precipitation = WeatherData.Hourly.Precipitation[i],
                        PrecipitationProbability = WeatherData.Hourly.PrecipitationProbability[i],
                        Humidity = WeatherData.Hourly.Relative_Humidity[i],
                        WindSpeed = speedBool ? (WeatherData.Hourly.Windspeed[i]* (float)0.62137):(WeatherData.Hourly.Windspeed[i]),
                        Pressure = WeatherData.Hourly.Pressure[i]
                    });
                   
                }

                // Set the ObservableCollection as the data source for the DataGrid
                WeatherGrid.ItemsSource = new ObservableCollection<WeatherEntry>(weatherEntries);
            }
            else
            {
                // If there is no weather data, show an error message
                MessageBox.Show("No weather Data", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void UpdateColumnHeaders()
        {
            foreach (var column in WeatherGrid.Columns)
            {
                switch (column.Header.ToString())
                {
                    case "Temperature [°C]":
                        column.Header = TemperatureBool ? "Temperature [°F]" : "Temperature [°C]";
                        break;
                    
                    case "Wind Speed [km/h]":
                        column.Header = SpeedBool ? "Wind Speed [mil/h]" : "Wind Speed [km/h]";
                        break;

                }
            }
        } // updating WeatherGrid column headers if there are other units
        
        public void ExitButton_Click(object sender, RoutedEventArgs e) // Event handler for Exit button click, which closes the Forecast window and resets the main window
        {
            this.Close(); // Close the Forecast window
            mainWindow.ResetTownBox(); // Reset the TextBox in the main window
            mainWindow.CheckBoxReset();
            mainWindow.ResetChooseCombo();
            mainWindow.Show(); // Show the main window again
        }
    }

    
    public class WeatherEntry // Class to represent individual weather data entry for each hourly forecast
    {
        public DateTime Time { get; set; } // Time of the weather entry

        public float Temperature { get; set; } // Temperature value

        public float Precipitation { get; set; } // Amount of precipitation

        public int Humidity { get; set; } // Humidity percentage

        public float WindSpeed { get; set; } // Wind speed

        public float PrecipitationProbability { get; set; } // Probability of precipitation

        public float Pressure { get; set; } // Atmospheric pressure
    }
}
