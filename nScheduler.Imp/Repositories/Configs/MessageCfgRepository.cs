using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Imp.Repositories.Configs;

public class MessageCfgRepository : BaseRepository<MessageCfg, Guid>, IMessageCfgRepository
{
    public MessageCfgRepository(DataContext context) : base(context)
    {
    }
}