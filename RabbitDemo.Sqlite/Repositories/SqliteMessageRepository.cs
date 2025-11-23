using Microsoft.Data.Sqlite;

namespace RabbitDemo.Sqlite.Repositories;

public class SqliteMessageRepository
{
    private readonly string _connectionString;

    public SqliteMessageRepository(string dbPath)
    {
        _connectionString = $"Data Source={dbPath}";
        Initialize();
    }

    private void Initialize()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        CREATE TABLE IF NOT EXISTS Messages (
            Id INTEGER PRIMARY KEY AUTOINCREMENT,
            Text TEXT NOT NULL,
            CreatedAt TEXT NOT NULL
        );
        """;

        command.ExecuteNonQuery();
    }

    public void SaveMessage(string text)
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        """
        INSERT INTO Messages (Text, CreatedAt)
        VALUES ($text, $createdAt);
        """;

        command.Parameters.AddWithValue("$text", text);
        command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

        command.ExecuteNonQuery();
    }
}