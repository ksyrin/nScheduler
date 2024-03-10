using System;
using nScheduler.Domain.Entities;
using nScheduler.Domain.Repositories;

namespace nScheduler.Infrastructure.Repositories;

public class ImageConfigRepository : BaseEFRepository<ImageConfig>, IImageConfigRepository
{
    public ImageConfigRepository(DataContext context) : base(context)
    {
    }
}

