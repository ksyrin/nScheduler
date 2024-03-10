using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Dockerd.Repositories.Configs;

public class ImageCfgRepository : BaseRepository<ImageCfg>, IImageCfgRepository
{
    public ImageCfgRepository(DataContext context) : base(context)
    {
    }
}