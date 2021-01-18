namespace WeatherApi.Core
{
	public class CurrentWeather : WeatherInfo
	{
		/// <summary>
		/// Current temperature in Celsius
		/// </summary>
		public double Temperature { get; set; }

		public override string ToString()
		{
			return $"Current weather is:\n\t" +
				$"Temperature - {Temperature} degree(s) of Celsius;\n\t" +
				$"Speed of wind - {WindSpeed} meter/sec;\n\t" +
				$"Cloudiness - {Cloudiness}%.\n" +
				$"Date of calculation - {DateOfCalculation.ToString("o")}";
		}
	}
}
