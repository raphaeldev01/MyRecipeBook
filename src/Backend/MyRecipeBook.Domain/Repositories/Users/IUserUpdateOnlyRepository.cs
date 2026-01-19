using System;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserUpdateOnlyRepository
{
    public Task<User?> GetUsetById(long id);
    public void Update(User user);
}
