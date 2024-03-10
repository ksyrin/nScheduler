using Microsoft.EntityFrameworkCore;
using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Models.Jobs;
using nScheduler.Domain.Models.Users;

namespace nScheduler.Dockerd.Repositories;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<ParameterCfg> ParameterCfgs { get; set; }

    public DbSet<ImageCfg> ImageCfgs { get; set; }

    public DbSet<MessageCfg> MessageCfgs { get; set; }

    public DbSet<JobInfoModel> Jobs { get; set; }

    public DbSet<JobLogModel> Logs { get; set; }

    public DbSet<UserModel> Users { get; set; }
}