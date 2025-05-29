//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using Microsoft.IdentityModel.Tokens;

//public static class TokenService
//{
//    public static string GenerateToken(string userId, string tipo, string email)
//    {
//        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("y1K4r9a4t2P7s8Q3z6U9x4B7e2T0l8H5")); // Troque por uma chave secreta forte

//        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

//        var claims = new[]
//        {
//            new Claim(ClaimTypes.NameIdentifier, userId),
//            new Claim(ClaimTypes.Role, tipo),
//            new Claim(ClaimTypes.Email, email)
//        };

//        var token = new JwtSecurityToken(
//            issuer: "LojaMoveis",
//            audience: "LojaMoveis",
//            claims: claims,
//            expires: DateTime.UtcNow.AddHours(2),
//            signingCredentials: creds
//        );

//        return new JwtSecurityTokenHandler().WriteToken(token);
//    }
//}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;

public static class TokenService
{
    private static readonly string SecretKey = "y1K4r9a4t2P7s8Q3z6U9x4B7e2T0l8H5";

    public static string GenerateToken(string userId, string tipo, string email)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));

        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim(ClaimTypes.Role, tipo),
            new Claim(ClaimTypes.Email, email)
        };

        var token = new JwtSecurityToken(
            issuer: "LojaMoveis",
            audience: "LojaMoveis",
            claims: claims,
            expires: DateTime.UtcNow.AddHours(2),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public static ClaimsPrincipal ValidarToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(SecretKey);

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(key),
            ValidateIssuer = true,
            ValidIssuer = "LojaMoveis",
            ValidateAudience = true,
            ValidAudience = "LojaMoveis",
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero // Para evitar atrasos de alguns segundos
        };

        try
        {
            var principal = tokenHandler.ValidateToken(token, validationParameters, out _);
            return principal;
        }
        catch
        {
            return null;
        }
    }
}
