using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Dockerd.Repositories.Configs;

public class MessageCfgRepository : BaseRepository<MessageCfg>, IMessageCfgRepository
{
    public MessageCfgRepository(DataContext context) : base(context)
    {
    }
}