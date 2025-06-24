// Models/ResetToken.cs
namespace LojaMoveis.Models
{
    public class ResetToken
    {
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime ExpiraEm { get; set; }
    }
}
