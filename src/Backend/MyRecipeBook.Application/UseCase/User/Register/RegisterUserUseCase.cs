using System.Threading.Tasks;
using AutoMapper;
using MyRecipeBook.Application.Services.AutoMapper;
using MyRecipeBook.Application.Services.Cryptography;
using MyRecipeBook.Communication.Requests;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Repositories;
using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Domain.Scurity.AccessToken;
using MyRecipeBook.Exceptions;


// using MyRecipeBook.Domain.Repositories.Users;
using MyRecipeBook.Exceptions.ExceptionsBase;

namespace MyRecipeBook.Application.UseCase.User.Register;

public class RegisterUserUseCase : IRegisterUserUseCase
{
    private readonly IUserReadOnlyRepository _readOnlyRepository;
    private readonly IUserWriteOnlyRepository _writeOnlyRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly PasswordEncripter _passwordEncripter;
    private readonly  IAccessTokenGeneretor _accessTokenGeneretor;

    public RegisterUserUseCase(
        IUserReadOnlyRepository readOnlyRepository,
        IUserWriteOnlyRepository writeOnlyRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper,
        PasswordEncripter passwordEncripter,
        IAccessTokenGeneretor accessTokenGeneretor
    )
    {
        _writeOnlyRepository = writeOnlyRepository;
        _readOnlyRepository = readOnlyRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _passwordEncripter = passwordEncripter;
        _accessTokenGeneretor = accessTokenGeneretor;
    }

    public async Task<ResponseRegisterUserJson> Execute(RequestRegisterUserJson request)
    {
        await Validate(request);

        var user = _mapper.Map<Domain.Entities.User>(request);
        user.Password = _passwordEncripter.Encrypt(request.Password);
        user.UserIdentifyer = Guid.NewGuid();

        await _writeOnlyRepository.Add(user);

        await _unitOfWork.Commit();

        return new ResponseRegisterUserJson
        {
            Name = user.Name,
            Tokens = new ResponseTokensJson()
            {
                AccessToken = _accessTokenGeneretor.Generete(user.UserIdentifyer)
            }
        };
    }

    private async Task Validate(RequestRegisterUserJson request)
    {
        var validator = new RegisterValidator();

        var result = validator.Validate(request);

        var emailExists = await _readOnlyRepository.ExistActiveUsersWithEmail(request.Email);

        if(emailExists)
            result.Errors.Add(new FluentValidation.Results.ValidationFailure(string.Empty, ResourceExceptionsMessages.EMAIL_EXISTS));

        if(result.IsValid == false)
        {
            var errorMessages = result.Errors.Select(e => e.ErrorMessage).ToList();

            throw new ErrorOnValidate(errorMessages);
        }
    }
}