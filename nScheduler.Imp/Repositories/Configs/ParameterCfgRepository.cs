using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Imp.Repositories.Configs;

public class ParameterCfgRepository : BaseRepository<ParameterCfg, Guid>, IParameterCfgRepository
{
    public ParameterCfgRepository(DataContext context) : base(context)
    {
    }
}