using System;

namespace WeatherApi.Core
{
	public class WeatherInfo
	{
		/// <summary>
		/// Date of data calculation
		/// </summary>
		public DateTimeOffset DateOfCalculation { get; set; }

		/// <summary>
		/// Speed of the wind in meter/sec
		/// </summary>
		public double WindSpeed { get; set; }

		/// <summary>
		/// Cloudiness, %
		/// </summary>
		public double Cloudiness { get; set; }
	}
}
