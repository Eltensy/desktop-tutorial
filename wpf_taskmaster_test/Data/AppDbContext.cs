using System;
using System.Security.Principal;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.Options;
using Npgsql;

namespace wpf_backend.Data
{
    public enum StateType
    {
        trash,
        //in_progress,
        done,
        canceled,
        in_progress
    }

    public enum PriorityType
    {
        low,
        middle,
        high
    }

    public enum LangType
    {
        ua,
        en
    }

    public enum ThemeType
    {
        dark,
        light
    }

    public enum DateFormatType
    {
        eu,
        us,
        abr
    }

    public enum TimeFormatType
    {
        _12,
        _24
    }

    internal class AppDbContext : DbContext
    {
        protected readonly IConfiguration Configuration;

        public AppDbContext()
        {
            Console.WriteLine(System.AppDomain.CurrentDomain.BaseDirectory + "appsettings.json");
            Configuration = new ConfigurationBuilder()
            .SetBasePath(System.AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json")
            .Build();
        }
        public AppDbContext(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnModelCreating(ModelBuilder model)
        {
            model
                .HasPostgresEnum(null, "lang_enum", new[] { "ua", "en" })
                .HasPostgresEnum(null, "theme_enum", new[] { "dark", "light" })
                .HasPostgresEnum(null, "date_format_enum", new[] { "eu", "us", "abr" })
                .HasPostgresEnum(null, "time_format_enum", new[] { "_12", "_24" })
                .HasPostgresEnum(null, "state_enum", new[] { "in progress", "done", "canceled", "in_progress" })
                .HasPostgresEnum(null, "priority_enum", new[] { "low", "middle", "high" });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            // connect to postgres with connection string from app settings
            options.UseNpgsql(Configuration.GetConnectionString("task_master_db"));
            NpgsqlConnection.GlobalTypeMapper
                .MapEnum<StateType>("state_enum")
                .MapEnum<PriorityType>("priority_enum")
                .MapEnum<LangType>("lang_enum")
                .MapEnum<ThemeType>("theme_enum")
                .MapEnum<DateFormatType>("date_format_enum")
                .MapEnum<TimeFormatType>("time_format_enum");

        }

        public DbSet<Data.User> users { get; set; }
        public DbSet<Data.User_settings> user_settings { get; set; }
        public DbSet<Data.Project> projects { get; set; }
        public DbSet<Data.Task> tasks { get; set; }
        public DbSet<Data.Progress_log> progress_log { get; set; }

    }
}
