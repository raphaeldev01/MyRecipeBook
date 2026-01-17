using System;

namespace MyRecipeBook.Communication.Requests;

public class RequestDoLoginUserJson
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
