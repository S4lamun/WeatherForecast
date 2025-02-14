using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherForecast
{
    // Added required because this data is necessary for program to work
    public class Town
    {
        [JsonPropertyName("lat")]
        public required string Latitude { get; set; }

        [JsonPropertyName("lon")]
        public required string Longitude { get; set; }

        [JsonPropertyName("display_name")]
        public required string DisplayName { get; set; }
    } // Class to get coordinates

    public class HourlyData
    {
        [JsonPropertyName("time")]
        public required string[] Time { get; set; }

        [JsonPropertyName("temperature_2m")]
        public required double[] Temperature { get; set; }

        [JsonPropertyName("precipitation")]
        public required double[] Precipitation { get; set; }

        [JsonPropertyName("relative_humidity_2m")]
        public required int[] Relative_Humidity { get; set; }

        [JsonPropertyName("windspeed_10m")]
        public required double[] Windspeed { get; set; }

        [JsonPropertyName("precipitation_probability")]
        public required double[] PrecipitationProbability { get; set; }

        [JsonPropertyName("pressure_msl")]
        public required double[] Pressure { get; set; }
    } // Class for Hourly Data

    public class WeatherResponse // Class to collect MeteoData - gathers Hourly data of Temperature, Precipitation, Humidity, Windspeed
    {
        public required HourlyData Hourly { get; set; }
    }
}
