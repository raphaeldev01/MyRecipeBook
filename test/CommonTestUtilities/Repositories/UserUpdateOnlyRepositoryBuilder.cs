using System;
using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class UserUpdateOnlyRepositoryBuilder
{
    private readonly Mock<IUserUpdateOnlyRepository> _repository;

    public UserUpdateOnlyRepositoryBuilder() => _repository = new Mock<IUserUpdateOnlyRepository>();

    public UserUpdateOnlyRepositoryBuilder GetUserById(User user)
    {
        _repository.Setup(repository => repository.GetUserById(user.Id)).ReturnsAsync(user);
        return this;
    }

    public void Update(User user)
    {
        _repository.Setup(repository => repository.Update(user));
    }

    public IUserUpdateOnlyRepository Build () => _repository.Object;
}
