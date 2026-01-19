using System;
using MyRecipeBook.Domain.Entities;

namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserWriteOnlyRepository
{

    public Task Add(Entities.User user);

}
