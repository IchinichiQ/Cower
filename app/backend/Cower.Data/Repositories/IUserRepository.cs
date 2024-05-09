using Cower.Data.Entities;

namespace Cower.Data.Repositories;

public interface IUserRepository
{
    public Task<UserEntity?> GetUser(long id);
    public Task<UserEntity?> GetUserByCredentials(string email, byte[] password);
    public Task<UserEntity> AddUser(UserEntity user);
}