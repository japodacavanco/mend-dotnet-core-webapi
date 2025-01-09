using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace POC_PROJ_API_01.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly string? _connectionString; // = "YourConnectionStringHere";

		public ProductsController(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection");
		}

		[HttpGet("{myId}/products")]
		public IActionResult GetProductById(string myId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				// This query is vulnerable to SQL injection
				var query = $"SELECT * FROM Products WHERE ProductId = '{myId}'";

				// This query is safe from SQL injection
				// var query = "SELECT * FROM Products WHERE ProductId = @ProductId";

				var command = new SqlCommand(query, connection);

				// command.Parameters.AddWithValue("@ProductId", myId);

				connection.Open();
				var reader = command.ExecuteReader();
				// Process the data reader...
			}
			return Ok();
		}

		[HttpGet]
		public IActionResult GetProducts()
		{
			var products = new[]
			{
			new { Id = 1, Name = "Product A", Price = 10.0 },
			new { Id = 2, Name = "Product B", Price = 15.0 }
			};
			return Ok(products);
		}

		// CWE-22: Path/Directory Traversal
		[HttpGet("{id}/{fileName}")]
		public IActionResult DownloadFile(string id, string fileName)
		{
			var filePath = Path.Combine("C:\\Files", fileName);
			var fileBytes = System.IO.File.ReadAllBytes(filePath);
			return File(fileBytes, "application/octet-stream");

			// // Solution to CWE-22: Path/Directory Traversal
			// 	// Validate fileName for invalid characters
			// 	foreach (char c in Path.GetInvalidFileNameChars())
			// 	{
			// 		if (fileName.Contains(c))
			// 		{
			// 			return BadRequest("Invalid file name.");
			// 		}
			// 	}

			// 	string basePath = "C:\\Files";
			// 	string filePath = Path.Combine(basePath, fileName);
			// 	string fullPath = Path.GetFullPath(filePath);

			// 	// Ensure the file is within the intended directory
			// 	if (!fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
			// 	{
			// 		return BadRequest("Invalid file path.");
			// 	}

			// 	// Check if the file exists
			// 	if (!System.IO.File.Exists(fullPath))
			// 	{
			// 		return NotFound("File not found.");
			// 	}

			// 	// Read and return the file
			// 	var fileBytes = System.IO.File.ReadAllBytes(fullPath);
			// 	return File(fileBytes, "application/octet-stream");
		}

		// CWE-59: Symlink Vulnerability
		[HttpGet("read")]
		public async Task<IActionResult> ReadFile([FromQuery] string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
			{
				return BadRequest("File path is required.");
			}

			try
			{
				// WARNING: This code is intentionally vulnerable to symlink attacks.
				// It does not validate the file path properly.
				var fileContent = await System.IO.File.ReadAllTextAsync(filePath);
				return Ok(fileContent);

				// // Solution to CWE-59: Symlink Vulnerability
				// // Combine the base directory with the user-provided path
				// var combinedPath = Path.Combine(_baseDirectory, filePath);

				// // Resolve the absolute path
				// var resolvedPath = Path.GetFullPath(combinedPath);

				// // Check if the resolved path starts with the base directory path
				// if (!resolvedPath.StartsWith(_baseDirectory))
				// {
				// 	return BadRequest("Invalid file path.");
				// }

				// // Check if the file exists
				// if (!System.IO.File.Exists(resolvedPath))
				// {
				// 	return NotFound("File not found.");
				// }

				// // Read the file content
				// var fileContent = await System.IO.File.ReadAllTextAsync(resolvedPath);
				// return Ok(fileContent);
			}
			catch (FileNotFoundException)
			{
				return NotFound("File not found.");
			}
			catch (UnauthorizedAccessException)
			{
				return StatusCode(403, "Access to the file is denied.");
			}
			catch (IOException)
			{
				return StatusCode(500, $"An I/O error occurred");//: {ex.Message}");
			}
		}
	}
}
