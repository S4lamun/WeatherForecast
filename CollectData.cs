using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WeatherForecast
{   
    //Added required because this data is necessary for program to work 
       public class Town
        {
            [JsonPropertyName("lat")]
           required public string Latitude { get; set; }

            [JsonPropertyName("lon")]
        required public string Longitude { get; set; }

            [JsonPropertyName("display_name")]
        required public string DisplayName { get; set; }


        } // Class to get coordinates


       public  class HourlyData
    {
        [JsonPropertyName("time")]
        required public string[] Time { get; set; }


        [JsonPropertyName("temperature_2m")]
        required public float[] Temperature_2m { get; set; }


        [JsonPropertyName("precipitation")]
        required public float[] Precipitation { get; set; }


        [JsonPropertyName("relative_humidity_2m")]
        required public int[] Relative_Humidity_2m { get; set; }


        [JsonPropertyName("windspeed_10m")]
        required public float[] Windspeed_10m { get; set; }


        [JsonPropertyName("precipitation_probability")]
        required public float[] PrecipitationProbability { get; set; }


        [JsonPropertyName ("pressure_msl")]
        required public float[] Pressure { get; set; }

    } // Class for Hourly Data

        
       public class WeatherResponse //Class to collect MeteoData - gathers Hourly data of Temperature, Precipitation, Humudity, Windspeed
       {
        required public HourlyData Hourly { get; set; }
       }
}
