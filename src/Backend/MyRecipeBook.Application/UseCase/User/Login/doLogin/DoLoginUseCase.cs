using System;
using System.Text;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.User.Login.doLogin;

public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repositoy;
    private readonly PasswordEncripter _encripter;

    public DoLoginUseCase(IUserReadOnlyRepository userReadOnlyRepository, PasswordEncripter encripter)
    {
        _repositoy = userReadOnlyRepository;
        _encripter = encripter;
    }

    public async Task<ResponseRegisterUserJson> Execute (RequestDoLoginUserJson request)
    {
        Validate(request);

        var user = await _repositoy.GetByEmailAndPassword(request.Email, _encripter.Encrypt(request.Password));
        
        if(user == null)  throw new ErrorOnDoLogin();

        return new ResponseRegisterUserJson()
        {
            Name = user!.Name
        };
    }

    private void Validate(RequestDoLoginUserJson request)
    {
        var doLoginValidator = new DoLoginUserValidator();

        var result = doLoginValidator.Validate(request);

        if(result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnDoLogin(errorMessages);
        }
    }
}
