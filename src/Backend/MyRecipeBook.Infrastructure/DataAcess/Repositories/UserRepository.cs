using System;
using Microsoft.EntityFrameworkCore;
using MyRecipeBook.Domain.Entities;
using MyRecipeBook.Domain.Repositories.Users;

namespace MyRecipeBook.Infrastructure.DataAcess.Repositories;

public class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository, IUserUpdateOnlyRepository
{
    private readonly MyRecipeBookDbContext _dbContext;

    public UserRepository(MyRecipeBookDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user) => await _dbContext.Users.AddAsync(user);

    public async Task<bool> ExistActiveUsersWithEmail(string email) => await _dbContext.Users.AnyAsync(user => user.Email.Equals(email) && user.Active);


    public async Task<bool> ExistActiveUserWithUserIdentifier(Guid guid) => await _dbContext.Users.AnyAsync(user => user.UserIdentifyer.Equals(guid) && user.Active);

    public async Task<User?> GetByEmailAndPassword(string email, string password)
    {
        return await _dbContext
             .Users
             .AsNoTracking()
             .FirstOrDefaultAsync(user =>
                 user.Email.Equals(email) &&
                 user.Password.Equals(password) &&
                 user.Active);
    }

    public async Task<User?> GetUsetById (long id)
    {
        return await _dbContext
            .Users
            .FirstAsync(User => User.Id == id);
    }

    public void Update(User user) => _dbContext.Users.Update(user);

}
