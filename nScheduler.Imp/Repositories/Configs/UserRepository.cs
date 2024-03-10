using nScheduler.Domain.Models.Configs;
using nScheduler.Domain.Repositories.Configs;

namespace nScheduler.Imp.Repositories.Configs;

public class UserRepository : BaseRepository<UserModel, string>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }
}