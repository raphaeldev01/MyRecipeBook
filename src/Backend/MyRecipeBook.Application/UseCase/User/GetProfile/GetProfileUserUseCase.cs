using System;
using AutoMapper;
using MyRecipeBook.Communication.Responses;
using MyRecipeBook.Domain.Services.LoggedUser;

namespace MyRecipeBook.Application.UseCase.User.GetProfile;

public class GetProfileUserUseCase : IGetProfileUserUseCase
{
    private readonly ILoggedUser _loggedUser;
    private readonly IMapper _mapper;

    public GetProfileUserUseCase(
        IMapper mapper,
        ILoggedUser loggedUser
    )
    {
        _loggedUser = loggedUser;
        _mapper = mapper;
    }

    public async Task<ResponseProfileUserJson> Execute ()
    {  
        var user = await _loggedUser.User();
        
        return _mapper.Map<ResponseProfileUserJson>(user);
    }
}
