using System;
using Moq;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Users;

namespace CommonTestUtilities.Repositories;

public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder() => _repository = new Mock<IUserReadOnlyRepository>();

    public void ExistActiveUsersWithEmail(string email)
    {
        _repository.Setup(repository => repository.ExistActiveUsersWithEmail(email)).ReturnsAsync(true);
    }

    public void GetByEmailAndPassword(User user)
    {
        _repository.Setup(repository => repository.GetByEmailAndPassword(user.Email, user.Password)).ReturnsAsync(user);
    }

    public IUserReadOnlyRepository Build () => _repository.Object;
}
