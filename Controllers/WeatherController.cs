using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WeatherApi.Converter.Contract;
using WeatherApi.Core;

namespace WeatherApi.Controllers
{
	[Produces("application/json", "text/plain")]
	[ApiController]
	[Route("api/[controller]")]
	public class WeatherController : ControllerBase
	{
		private readonly IWeatherInfoConverter weatherInfoConverter;

		private readonly string apiKey;
		private readonly HttpClient httpClient;

		public WeatherController(IWeatherInfoConverter weatherInfoConverter, IConfiguration configuration)
		{
			this.weatherInfoConverter = weatherInfoConverter;
			apiKey = configuration["OpenWeatherMapApiKey"];

			httpClient = new HttpClient();
			httpClient.BaseAddress = new Uri("https://api.openweathermap.org/data/2.5");
		}

		/// <summary>
		/// Get current weather in specific city
		/// </summary>
		/// <param name="city">Name of the city</param>
		/// <param name="accept">Response media type</param>
		/// <returns>Current weather information in specific city</returns>
		/// <response code="200">Returns current weather information</response>
		/// <response code="415">If accept contains media type that aren`t produced</response>
		/// <response code="500">If server throw an exception or get not success status code from OpenWeatherMap</response>
		[HttpGet]
		[Route("GetCurrentWeather")]
		[ProducesResponseType(typeof(CurrentWeather), 200)]
		[ProducesResponseType(typeof(Response), 415)]
		[ProducesResponseType(typeof(Response), 500)]
		public async Task<IActionResult> GetCurrentWeather(string city, [FromHeader] string accept)
		{
			var requestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(httpClient.BaseAddress + $"/weather?q={city}&appid={apiKey}&units=metric"),
			};

			var response = await httpClient.SendAsync(requestMessage);

			if(!response.IsSuccessStatusCode)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					new Response
					{
						Status = "External Service Error",
						ResponseMessage = $"Service 'api.openweathermap.org' returns " +
						$"{(int)response.StatusCode} {response.ReasonPhrase}"
					});
			}

			CurrentWeather weatherInfo;

			try
			{
				weatherInfo = weatherInfoConverter.ConvertCurrentWeather(response);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					new Response
					{
						Status = "Internal service error",
						ResponseMessage = "Exception message:" + ex.Message
					});
			}

			switch (accept)
			{
				case "*/*":
				case "application/json":
					return Ok(weatherInfo);
				case "text/plain":
					return Ok(weatherInfo.ToString());
				default:
					return StatusCode(StatusCodes.Status415UnsupportedMediaType,
						new Response
						{
							Status = "Internal Service Error",
							ResponseMessage = $"Service doesn`t produce {accept} media type."
						});
			}
		}

		/// <summary>
		/// Get forecast for 5 days with 3-hour step in specific city
		/// </summary>
		/// <param name="city">Name of the city</param>
		/// <param name="accept">Response media type</param>
		/// <returns></returns>
		/// <response code="200">Returns weather forecast information with 3-hour step</response>
		/// <response code="415">If accept contains media type that aren`t produced</response>
		/// <response code="500">If server throw an exception or get not success status code from OpenWeatherMap</response>
		[HttpGet]
		[Route("GetForecast")]
		[ProducesResponseType(typeof(List<WeatherForecast>), 200)]
		[ProducesResponseType(typeof(Response), 415)]
		[ProducesResponseType(typeof(Response), 500)]
		public async Task<IActionResult> GetForecast(string city, [FromHeader] string accept)
		{
			var requestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(httpClient.BaseAddress + $"/forecast?q={city}&appid={apiKey}&units=metric"),
			};

			var response = await httpClient.SendAsync(requestMessage);

			if (!response.IsSuccessStatusCode)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					new Response
					{
						Status = "External Service Error",
						ResponseMessage = $"Service 'api.openweathermap.org\' returns " +
						$"{(int)response.StatusCode} {response.ReasonPhrase}"
					});
			}

			List<WeatherForecast> forecast;
			try
			{
				forecast = weatherInfoConverter.ConvertWeatherForecast(response);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					new Response
					{
						Status = "Internal service error",
						ResponseMessage = "Exception message:" + ex.Message
					});
			}

			switch (accept)
			{
				case "*/*":
				case "application/json":
					return Ok(forecast);
				case "text/plain":
					{
						string forecastText = "The 5 days weather forecast with 3-hour step:\n";
						foreach(WeatherForecast wf in forecast)
						{
							forecastText += wf.ToString() + "\n\n";
						}

						return Ok(forecastText);
					}
				default:
					return StatusCode(StatusCodes.Status415UnsupportedMediaType,
						new Response
						{
							Status = "Service Error",
							ResponseMessage = $"Service doesn`t produce {accept} media type."
						});
			}
		}

		/// <summary>
		/// Get short forecast for 5 days in specific city
		/// </summary>
		/// <param name="city">Name of the city</param>
		/// <param name="accept">Response media type (is taken from header)</param>
		/// <returns></returns>
		/// <response code="200">Returns short weather forecast information </response>
		/// <response code="415">If accept contains media type that aren`t produced</response>
		/// <response code="500">If server throw an exception or get not success status code from OpenWeatherMap</response>
		[HttpGet]
		[Route("GetShortForecast")]
		[ProducesResponseType(typeof(List<WeatherForecast>), 200)]
		[ProducesResponseType(typeof(Response), 415)]
		[ProducesResponseType(typeof(Response), 500)]
		public async Task<IActionResult> GetShortForecast(string city, [FromHeader] string accept)
		{
			var requestMessage = new HttpRequestMessage
			{
				Method = HttpMethod.Get,
				RequestUri = new Uri(httpClient.BaseAddress + $"/forecast?q={city}&appid={apiKey}&units=metric"),
			};

			var response = await httpClient.SendAsync(requestMessage);
			if (!response.IsSuccessStatusCode)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					new Response
					{
						Status = "External Service Error",
						ResponseMessage = $"Service 'api.openweathermap.org' returns " +
						$"{(int)response.StatusCode} {response.ReasonPhrase}"
					});
			}

			List<WeatherForecast> forecast;
			try
			{
				forecast = CalculateDailyForecast(response);
			}
			catch (Exception ex)
			{
				return StatusCode(StatusCodes.Status500InternalServerError,
					new Response
					{
						Status = "Internal service error",
						ResponseMessage = "Exception message:" + ex.Message
					});
			}

			switch (accept)
			{
				case "*/*":
				case "application/json":
					return Ok(forecast);
				case "text/plain":
					{
						string forecastText = "The short 5 days weather forecast:\n";
						foreach (WeatherForecast wf in forecast)
						{
							forecastText += wf.ToString() + "\n\n";
						}

						return Ok(forecastText);
					}
				default:
					return StatusCode(StatusCodes.Status415UnsupportedMediaType,
						new Response
						{
							Status = "Internal Service Error",
							ResponseMessage = $"Service doesn`t produce {accept} media type."
						});
			}
		}

		private List<WeatherForecast> CalculateDailyForecast(HttpResponseMessage httpResponse)
		{
			List<WeatherForecast> weatherForecasts = new List<WeatherForecast>();

			List<WeatherForecast> forecast = weatherInfoConverter.ConvertWeatherForecast(httpResponse);
			foreach(var forecastList in forecast.GroupBy(f => f.DateOfCalculation.Date))
			{
				weatherForecasts.Add(new WeatherForecast
				{
					DateOfCalculation = forecastList.Key,
					MinTemperature = forecastList.Min(f => f.MinTemperature),
					MaxTemperature = forecastList.Max(f => f.MaxTemperature),
					WindSpeed = Math.Round(forecastList.Average(f => f.WindSpeed), 2),
					Cloudiness = Math.Round(forecastList.Average(f => f.Cloudiness), 2)
				});
			}
			weatherForecasts[0].DateOfCalculation = forecast[0].DateOfCalculation;

			return weatherForecasts;
		}
	}
}
