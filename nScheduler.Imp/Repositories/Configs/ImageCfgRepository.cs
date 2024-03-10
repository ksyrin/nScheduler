using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Imp.Repositories.Configs;

public class ImageCfgRepository : BaseRepository<ImageCfg, Guid>, IImageCfgRepository
{
    public ImageCfgRepository(DataContext context) : base(context)
    {
    }
}