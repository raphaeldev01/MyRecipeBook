using System;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserUpdateOnlyRepository
{
    public Task<User?> GetUserById(long id);
    public void Update(User user);
}
