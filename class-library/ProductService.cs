using Microsoft.Data.SqlClient;

namespace MyClassLibrary
{
	public class ProductService
	{
		private readonly string _connectionString;

		public ProductService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task GetProductById(string myId)
		{
			await Task.Run(() =>
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
			});
		}
	}
}