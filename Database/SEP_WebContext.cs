using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using SEP_Web.Models;

namespace SEP_Web.Database;
public class SEP_WebContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

        try
        {
            string connectionString = Environment.GetEnvironmentVariable("SEP_WEB_CONNECTION_STRING");

            if (string.IsNullOrEmpty(connectionString)) throw new InvalidOperationException($"A string de conexão com o banco de dados não foi encontrada");

            ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

            optionsBuilder.UseMySql(connectionString, serverVersion);
        }
        catch (InvalidOperationException dbException) when (dbException.InnerException is MySqlException mySqlException)
        {
            throw mySqlException;
        }
    }

    public DbSet<UserAdministrator> Administrators => Set<UserAdministrator>();
    public DbSet<UserEvaluator> Evaluators => Set<UserEvaluator>();
    public DbSet<CivilServant> Servants => Set<CivilServant>();
    public DbSet<Instituition> Instituitions => Set<Instituition>();
    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Sector> Sectors => Set<Sector>();

    public DbSet<Licenses> Licenses => Set<Licenses>();

    public DbSet<Assessment> Assessments => Set<Assessment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Adiciona como índice exclusivo as propriedades que não podem ser duplicadas na tabela de administradores presente no banco de dados;
        modelBuilder.Entity<UserAdministrator>().HasIndex(u => u.Masp).IsUnique();
        modelBuilder.Entity<UserAdministrator>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<UserAdministrator>().HasIndex(u => u.Login).IsUnique();
        modelBuilder.Entity<UserAdministrator>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserAdministrator>().HasIndex(u => u.Phone).IsUnique();
        modelBuilder.Entity<UserAdministrator>().HasIndex(u => u.Password).IsUnique();

        // Adiciona como índice exclusivo as propriedades que não podem ser duplicadas na tabela de avaliadores presente no banco de dados;
        modelBuilder.Entity<UserEvaluator>().HasIndex(u => u.Masp).IsUnique();
        modelBuilder.Entity<UserEvaluator>().HasIndex(u => u.Name).IsUnique();
        modelBuilder.Entity<UserEvaluator>().HasIndex(u => u.Login).IsUnique();
        modelBuilder.Entity<UserEvaluator>().HasIndex(u => u.Email).IsUnique();
        modelBuilder.Entity<UserEvaluator>().HasIndex(u => u.Phone).IsUnique();
        modelBuilder.Entity<UserEvaluator>().HasIndex(u => u.Password).IsUnique();

        // Adiciona como índice exclusivo as propriedades que não podem ser duplicadas na tabela de licenças presente no banco de dados;
        modelBuilder.Entity<Licenses>().HasIndex(u => u.Name).IsUnique();
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SEP_WebContext).Assembly);
    }
}