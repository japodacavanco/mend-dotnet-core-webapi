using Microsoft.Data.SqlClient;
using MyClassLibrary2;

namespace MyClassLibrary
{
	public class ProductService
	{
		private readonly string _connectionString;
		private readonly string _baseDirectory;
		private readonly EmployeeService _employeeService;

		public ProductService(string connectionString, string baseDirectory)
		{
			_connectionString = connectionString;
			_baseDirectory = baseDirectory;
			_employeeService = new EmployeeService(connectionString);
		}

		public async Task GetProductById(string myId)
		{
			using (var connection = new SqlConnection(_connectionString))
			{
				// This query is vulnerable to SQL injection
				var query = $"SELECT * FROM Products WHERE ProductId = '{myId}'";

				// This query is safe from SQL injection
				// var query = "SELECT * FROM Products WHERE ProductId = @ProductId";

				var command = new SqlCommand(query, connection);

				// command.Parameters.AddWithValue("@ProductId", myId);

				await _employeeService.GetEmployeeById(myId);

				connection.Open();
				var reader = command.ExecuteReader();
				// Process the data reader...
			}
		}

		public async Task<dynamic> GetProducts()
		{
			return await Task.Run<dynamic>(() =>
			{
				var products = new[]
				{
				new { Id = 1, Name = "Product A", Price = 10.0 },
				new { Id = 2, Name = "Product B", Price = 15.0 }
				};

				return products;
			});

		}

		public async Task<dynamic> DownloadFile(string id, string fileName)
		{
			return await Task.Run<dynamic>(() =>
			{
				// WARNING: This code is intentionally vulnerable to path traversal attacks.
				var filePath = Path.Combine("C:\\Files", fileName);
				var fileBytes = System.IO.File.ReadAllBytes(filePath);
				return fileBytes;

				// // Solution to CWE-22: Path/Directory Traversal
				// // Validate fileName for invalid characters
				// foreach (char c in Path.GetInvalidFileNameChars())
				// {
				// 	if (fileName.Contains(c))
				// 	{
				// 		throw new Exception("Invalid file name.");
				// 	}
				// }

				// string basePath = "C:\\Files";
				// string filePath = Path.Combine(basePath, fileName);
				// string fullPath = Path.GetFullPath(filePath);

				// // Ensure the file is within the intended directory
				// if (!fullPath.StartsWith(basePath, StringComparison.OrdinalIgnoreCase))
				// {
				// 	throw new Exception("Invalid file path.");
				// }

				// // Check if the file exists
				// if (!System.IO.File.Exists(fullPath))
				// {
				// 	throw new Exception("File not found.");
				// }

				// // Read and return the file
				// var fileBytes = System.IO.File.ReadAllBytes(fullPath);
				// return fileBytes;
			});
		}

		public async Task<dynamic> ReadFile(string filePath)
		{
			return await Task.Run<dynamic>(() =>
			{
				// WARNING: This code is intentionally vulnerable to symlink attacks.
				// It does not validate the file path properly.
				var fileContent = System.IO.File.ReadAllTextAsync(filePath);
				return fileContent;

				// // Solution to CWE-59: Symlink Vulnerability
				// // Combine the base directory with the user-provided path
				// var combinedPath = Path.Combine(_baseDirectory, filePath);

				// // Resolve the absolute path
				// var resolvedPath = Path.GetFullPath(combinedPath);

				// // Check if the resolved path starts with the base directory path
				// if (!resolvedPath.StartsWith(_baseDirectory))
				// {
				// 	throw new Exception("Invalid file path.");
				// }

				// // Check if the file exists
				// if (!System.IO.File.Exists(resolvedPath))
				// {
				// 	throw new Exception("File not found.");
				// }

				// // Read the file content
				// var fileContent = System.IO.File.ReadAllTextAsync(resolvedPath);
				// return fileContent;
			});
		}

	}
}