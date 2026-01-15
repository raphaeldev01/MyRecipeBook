using System;
using Moq;
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

    public IUserReadOnlyRepository Build () => _repository.Object;
}
