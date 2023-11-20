using System.Data.SqlTypes;
using Microsoft.EntityFrameworkCore;
using SEP_Web.Models;

namespace SEP_Web.Database;
public class SEP_WebContext : DbContext
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = Environment.GetEnvironmentVariable("SEP_WEB_CONNECTION_STRING");

        ServerVersion serverVersion = ServerVersion.AutoDetect(connectionString);

        if (string.IsNullOrEmpty(connectionString)) throw new SqlNullValueException($"A string de conexão com o banco de dados não foi encontrada");

        optionsBuilder.UseMySql(connectionString, serverVersion);
    }

    public DbSet<UserAdministrator> Administrators => Set<UserAdministrator>();
    public DbSet<UserEvaluator> Evaluators => Set<UserEvaluator>();
    public DbSet<CivilServant> Servants => Set<CivilServant>();
    public DbSet<Instituition> Instituitions => Set<Instituition>();
    public DbSet<Division> Divisions => Set<Division>();
    public DbSet<Section> Sections => Set<Section>();
    public DbSet<Sector> Sectors => Set<Sector>();

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
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SEP_WebContext).Assembly);
    }
}
