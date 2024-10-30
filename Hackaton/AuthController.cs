using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;


[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    [HttpGet("google-login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties { RedirectUri = "/api/auth/google-callback" };
        return Challenge(properties, GoogleDefaults.AuthenticationScheme);
    }

    [HttpGet("google-callback")]
    public IActionResult GoogleCallback()
    {
        var userInfo = HttpContext.User;
        // Aquí manejas el usuario autenticado y rediriges a Blazor WebAssembly
        return Redirect("https://tuaplicacioncliente.com");
    }
}

internal class GoogleDefaults
{
    public static string[] AuthenticationScheme { get; internal set; }
}