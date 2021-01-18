# WeatherAPI

The WeatherAPI service supports following methods:
  
| API           | Description   |
| ------------- |:-------------:|
| /api/Weather/GetCurrentWeather| Get current weather in specific city |
| /api/Weather/GetForecast      | Get forecast for 5 days with 3-hour step in specific city      |
| /api/Weather/GetShortForecast | Get short forecast for 5 days in specific city      |

Note! The service can produce ```application/json``` or ```text/plain``` response media types according to the specified ```Accept``` header.  

***

> Swagger UI is available on https://localhost:44399/
