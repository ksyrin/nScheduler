using System;
using nScheduler.Domain.Models.Users;
using nScheduler.Domain.Repositories.Users;

namespace nScheduler.Dockerd.Repositories.Users;

public class UserRepository : BaseRepository<UserModel>, IUserRepository
{
    public UserRepository(DataContext context) : base(context)
    {
    }
}

