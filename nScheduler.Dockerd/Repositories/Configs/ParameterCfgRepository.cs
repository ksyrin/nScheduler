using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Dockerd.Repositories.Configs;

public class ParameterCfgRepository : BaseRepository<ParameterCfg>, IParameterCfgRepository
{
    public ParameterCfgRepository(DataContext context) : base(context)
    {
    }
}