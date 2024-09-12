using System.Diagnostics.CodeAnalysis;

namespace _1_API.Response;

public class AuthenticationResponse
{
    public string token { get; set; }
    public string refreshToken { get; set; }
    public bool result { get; set; }
    public string message { get; set; }
}