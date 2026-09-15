using System.Security.Claims;

namespace Huellitas.Utilities;

public static class ClaimsPrincipalExtensions
{
    public static string GetRequiredUserId(this ClaimsPrincipal principal)
    {
        return principal.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new InvalidOperationException(
                "El principal actual no contiene un identificador de usuario.");
    }
}
