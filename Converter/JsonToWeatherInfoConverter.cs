using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using WeatherApi.Converter.Contract;
using WeatherApi.Core;

namespace WeatherApi.Converter
{
	public class JsonToWeatherInfoConverter : IWeatherInfoConverter
	{
		public CurrentWeather ConvertCurrentWeather(HttpResponseMessage httpResponse)
		{
			var resp = httpResponse.Content.ReadAsStringAsync().Result;
			var obj = JObject.Parse(resp);

			return ConvertJsonToWeatherInfo(obj, false) as CurrentWeather;
		}

		public List<WeatherForecast> ConvertWeatherForecast(HttpResponseMessage httpResponse)
		{
			var resp = httpResponse.Content.ReadAsStringAsync().Result;
			var obj = JObject.Parse(resp);

			List<WeatherForecast> forecast = new List<WeatherForecast>();

			foreach(JObject ob in obj["list"])
			{
				forecast.Add(ConvertJsonToWeatherInfo(ob, true) as WeatherForecast);
			}

			return forecast;
		}

		private WeatherInfo ConvertJsonToWeatherInfo(JObject obj, bool isForecast)
		{
			WeatherInfo weatherInfo;

			if(isForecast)
			{
				weatherInfo = new WeatherForecast();
				(weatherInfo as WeatherForecast).MinTemperature = Convert.ToDouble(obj["main"]["temp_min"]);
				(weatherInfo as WeatherForecast).MaxTemperature = Convert.ToDouble(obj["main"]["temp_max"]);
			}
			else
			{
				weatherInfo = new CurrentWeather();
				(weatherInfo as CurrentWeather).Temperature = Convert.ToDouble(obj["main"]["temp"]);
			}

			DateTimeOffset dateTimeOffset = DateTimeOffset
				.FromUnixTimeSeconds(Convert.ToInt64(obj["dt"]) + Convert.ToInt64(obj["timezone"]));

			DateTimeOffset dateTime = new DateTimeOffset(dateTimeOffset.DateTime,
				TimeSpan.FromSeconds(Convert.ToInt64(obj["timezone"])));

			weatherInfo.DateOfCalculation = dateTime;
			weatherInfo.WindSpeed = Convert.ToDouble(obj["wind"]["speed"]);
			weatherInfo.Cloudiness = Convert.ToInt32(obj["clouds"]["all"]);

			return weatherInfo;
		}
	}
}
