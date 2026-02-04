using Pustok.Business.Abstractions;

namespace Pustok.Business.Exceptions;

public class LoginFailException(string message = "Login failed") : Exception(message), IBaseException
{
    public int StatusCode { get; set; } = 400;
}