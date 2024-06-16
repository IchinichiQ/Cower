using Cower.Data.Models;
using Cower.Data.Models.Entities;

namespace Cower.Data.Repositories;

public interface IUserRepository
{
    public Task<UserEntity?> GetUser(long id);
    public Task<UserEntity?> GetUserByCredentials(string email, byte[] password);
    public Task<UserEntity?> GetUserByEmail(string email);
    public Task<UserEntity> AddUser(UserEntity user);
    public Task<UserEntity?> UpdateUser(UpdateUserDal dal);
}