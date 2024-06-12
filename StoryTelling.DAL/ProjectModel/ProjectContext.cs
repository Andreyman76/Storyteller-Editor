using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace StoryTelling.DAL.ProjectModel;

public class ProjectContext(string dbFileName) : DbContext
{
    private readonly string _dbFileName = dbFileName;

    public DbSet<ProjectSettings> Settings { get; set; }
    public DbSet<ProjectNode> Nodes { get; set; }
    public DbSet<ProjectTransition> Transitions { get; set; }

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