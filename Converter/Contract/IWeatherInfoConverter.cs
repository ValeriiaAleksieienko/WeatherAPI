using System.Collections.Generic;
using System.Net.Http;
using WeatherApi.Core;

namespace WeatherApi.Converter.Contract
{
	public interface IWeatherInfoConverter
	{
		CurrentWeather ConvertCurrentWeather(HttpResponseMessage httpResponse);

		List<WeatherForecast> ConvertWeatherForecast(HttpResponseMessage httpResponse);
	}
}
