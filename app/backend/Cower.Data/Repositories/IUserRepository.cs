using Cower.Data.Entities;

namespace Cower.Data.Repositories;

public interface IUserRepository
{
    public UserEntity? GetUser(long id);
    public UserEntity? GetUserByCredentials(string email, byte[] password);
    public UserEntity AddUser(UserEntity user);
}