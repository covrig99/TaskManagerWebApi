using System.Security.Claims;

namespace TaskManagerWebApi.JWT_Helper
{
    public static class HelperJWT
    {
        public static string GetUserRole(this ClaimsPrincipal principal)
        {
            var role = (principal?.Identity as ClaimsIdentity)?.FindFirst("rol")?.Value;
            return role;
        }
        public static string GetUserEmail(this ClaimsPrincipal principal)
        {
            if (principal == null) return null;

            return principal.FindFirst(ClaimTypes.Email)?.Value // Standard claim type
                ?? principal.FindFirst("email")?.Value           // Some JWTs use "email"
                ?? principal.FindFirst("mail")?.Value            // Your original claim type
                ?? principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        }
    }
}
