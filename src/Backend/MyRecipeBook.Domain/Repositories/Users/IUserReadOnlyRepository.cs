using System;

namespace MyRecipeBook.Domain.Repositories.Users;

public interface IUserReadOnlyRepository
{
        public Task<bool> ExistActiveUsersWithEmail(string email);

        public Task<Entities.User?> GetByEmailAndPassword(string email, string password);

        public Task<bool> ExistActiveUserWithUserIdentifier (Guid guid);
}
