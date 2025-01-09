using Microsoft.AspNetCore.Mvc;

namespace POC_PROJ_API_01.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class WeatherForeCastController : ControllerBase
	{
		[HttpGet]
		public IActionResult GetWeatherCast()
		{
			var summaries = new[]
			{
				"Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
			};

			var forecast = Enumerable.Range(1, 5).Select(index =>
				new WeatherForecast
				(
					DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
					Random.Shared.Next(-20, 55),
					summaries[Random.Shared.Next(summaries.Length)]
				))
				.ToArray();
			return Ok(forecast);
		}
	}

	record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
	{
		public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
	}
}