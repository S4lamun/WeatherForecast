using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherForecast
{

        // Class to get coordinates
       public class Town
        {
            [JsonPropertyName("lat")]
            public string Latitude { get; set; }

            [JsonPropertyName("lon")]
            public string Longitude { get; set; }

        [JsonPropertyName("display_name")]
        public string DisplayName { get; set; }
    }

      public  class HourlyData
        {
            [JsonPropertyName("time")]
            public string[] Time { get; set; }

            [JsonPropertyName("temperature_2m")]
            public float[] Temperature_2m { get; set; }

            [JsonPropertyName("precipitation")]
            public float[] Precipitation { get; set; }

            [JsonPropertyName("relative_humidity_2m")]
            public int[] Relative_Humidity_2m { get; set; }

            [JsonPropertyName("windspeed_10m")]
            public float[] Windspeed_10m { get; set; }

        [JsonPropertyName("precipitation_probability")]
        public float[] PrecipitationProbability { get; set; }

        [JsonPropertyName ("pressure_msl")]
        public float[] Pressure { get; set; }

    }

        //Class to collect MeteoData - gathers Hourly data of Temperature, Precipitation, Humudity, Windspeed
       public class WeatherResponse
        {
            public HourlyData Hourly { get; set; }
        }
}
