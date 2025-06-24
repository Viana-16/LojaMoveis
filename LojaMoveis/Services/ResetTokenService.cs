// Services/ResetTokenService.cs
using LojaMoveis.Models;

public class ResetTokenService
{
    private static List<ResetToken> tokens = new();

    public void SalvarToken(string email, string token)
    {
        tokens.RemoveAll(t => t.Email == email); // remove token antigo
        tokens.Add(new ResetToken
        {
            Email = email,
            Token = token,
            ExpiraEm = DateTime.UtcNow.AddMinutes(30)
        });
    }

    public string? ObterEmailPorToken(string token)
    {
        var tokenObj = tokens.FirstOrDefault(t => t.Token == token && t.ExpiraEm > DateTime.UtcNow);
        return tokenObj?.Email;
    }

    public void RemoverToken(string token)
    {
        tokens.RemoveAll(t => t.Token == token);
    }
}
