using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeatherApi.Core
{
	public class WeatherComparison
	{
		/// <summary>
		/// Current weather in the first city
		/// </summary>
		private CurrentWeather firstCityWeather;
		/// <summary>
		/// Current weather in the second city
		/// </summary>
		private CurrentWeather secondCityWeather;

		public WeatherComparison(CurrentWeather firstCityWeather, CurrentWeather secondCityWeather)
		{
			this.firstCityWeather = firstCityWeather;
			this.secondCityWeather = secondCityWeather;
		}

		/// <summary>
		/// Dates of data calculation
		/// </summary>
		public string DateOfCalculation
		{
			get => firstCityWeather.DateOfCalculation.ToString("O") + " vs " + secondCityWeather.DateOfCalculation.ToString("O"); 
		}

		/// <summary>
		/// Current temperatures in Celsius
		/// </summary>
		public string Temperature 
		{
			get => firstCityWeather.Temperature + "C vs " + secondCityWeather.Temperature + "C";
		}

		/// <summary>
		/// Speed of the wind in meter/sec
		/// </summary>
		public string WindSpeed 
		{
			get => firstCityWeather.WindSpeed + "m/s vs " + secondCityWeather.WindSpeed + "m/s";
		}


		/// <summary>
		/// Cloudiness, %
		/// </summary>
		public string Cloudiness 
		{
			get => firstCityWeather.Cloudiness + "% vs " + secondCityWeather.Cloudiness + "%";
		}

		/// <summary>
		/// Weather descroption
		/// </summary>
		public string Description 
		{
			get => firstCityWeather.Description + " vs " + secondCityWeather.Description;
		}
	}
}
