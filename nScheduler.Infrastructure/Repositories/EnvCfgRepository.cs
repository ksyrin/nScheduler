using System;
using nScheduler.Domain.Entities;
using nScheduler.Domain.Repositories;

namespace nScheduler.Infrastructure.Repositories;

public class EnvCfgRepository :
    BaseEFRepository<EnvironmentConfig>, IEnvCfgRepository
{
    public EnvCfgRepository(DataContext context) : base(context)
    {
    }
}

