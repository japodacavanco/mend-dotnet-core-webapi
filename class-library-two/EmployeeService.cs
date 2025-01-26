using Microsoft.Data.SqlClient;

namespace MyClassLibrary2
{
	public class EmployeeService
	{
		private readonly string _connectionString;

		public EmployeeService(string connectionString)
		{
			_connectionString = connectionString;
		}

		public async Task GetEmployeeById(string employeeId)
		{
			await Task.Run(() =>
			{
				using (var connection = new SqlConnection(_connectionString))
				{
					// This query is vulnerable to SQL injection
					var query = $"SELECT * FROM Employees WHERE EmployeeId = '{employeeId}'";

					// This query is safe from SQL injection
					// var query = "SELECT * FROM Employees WHERE EmployeeId = @EmployeeId";

					var command = new SqlCommand(query, connection);

					// command.Parameters.AddWithValue("@EmployeeId", employeeId);

					connection.Open();
					var reader = command.ExecuteReader();
					// Process the data reader...
				}
			});
		}

	}
}