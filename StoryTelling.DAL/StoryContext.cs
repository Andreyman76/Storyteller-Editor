using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace StoryTelling.DAL;

public class StoryContext : DbContext
{
    private readonly string _dbFileName;

    public DbSet<ProjectSettings> ProjectSettings { get; set; }
    public DbSet<StoryNode> Nodes { get; set; }
    public DbSet<StoryTransition> Transitions { get; set; }

    public StoryContext(string dbFileName)
    {
        _dbFileName = dbFileName;
    }

    protected override void OnConfiguring(DbContextOptionsBuilder options)
    {
        var connectionString = new SqliteConnectionStringBuilder()
        {
            DataSource = _dbFileName
        }.ToString();

        var connection = new SqliteConnection(connectionString);
        connection.Open();
        using var command = connection.CreateCommand();
        command.CommandText = "PRAGMA journal_mode = DELETE;";
        command.ExecuteNonQuery();

        options.UseSqlite(connection);
    }


}