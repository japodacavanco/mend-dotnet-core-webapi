using Microsoft.AspNetCore.Mvc;
using MyClassLibrary;

namespace POC_PROJ_API_01.Controllers
{
    [ApiController]
	[Route("api/[controller]")]
	public class ProductsController : ControllerBase
	{
		private readonly string _connectionString; // = "YourConnectionStringHere";
		private readonly ProductService _productService;
		private readonly string _baseDirectory = "C:\\Files";

		public ProductsController(IConfiguration configuration)
		{
			_connectionString = configuration.GetConnectionString("DefaultConnection")!;
			_productService = new ProductService(_connectionString, _baseDirectory);
		}

		[HttpGet("{myId}/products")]
		public async Task<IActionResult> GetProductById(string myId)
		{
			await _productService.GetProductById(myId);
			return Ok();
		}

		[HttpGet]
		public async Task<IActionResult> GetProducts()
		{
			var products = await _productService.GetProducts();
			return Ok(products);
		}

		// CWE-22: Path/Directory Traversal
		[HttpGet("{id}/{fileName}")]
		public async Task<IActionResult> DownloadFile(string id, string fileName)
		{
			var fileBytes = await _productService.DownloadFile(id, fileName);
			return File(fileBytes, "application/octet-stream");
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
				var fileContent = await _productService.ReadFile(filePath);
				return Ok(fileContent);
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
