using System;
using Microsoft.EntityFrameworkCore;
using nScheduler.Domain.Entities;

namespace nScheduler.Infrastructure.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<EnvironmentConfig> EnvironmentConfigs { get; set; }
}

