namespace WeatherApi.Core
{
	public class WeatherForecast : WeatherInfo
	{
		/// <summary>
		/// Minimum temperature in Celsius
		/// </summary>
		public double MinTemperature { get; set; }

		/// <summary>
		/// Maximum temperature in Celsius
		/// </summary>
		public double MaxTemperature { get; set; }

		public override string ToString()
		{
			return $"Date: {DateOfCalculation.ToString("o")}\n\t" +
				$"Minimum temperature - {MinTemperature} degree(s) of Celsius;\n\t" +
				$"Maximum temperature - {MaxTemperature} degree(s) of Celsius;\n\t" +
				$"Speed of wind - {WindSpeed} meter/sec;\n\t" +
				$"Cloudiness - {Cloudiness}%.\n";
		}
	}
}
