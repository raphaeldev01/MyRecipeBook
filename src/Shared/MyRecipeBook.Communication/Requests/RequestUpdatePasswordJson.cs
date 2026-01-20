using System;

namespace MyRecipeBook.Communication.Requests;

public class RequestUpdatePasswordJson
{
    public string Password { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}
