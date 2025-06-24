namespace LojaMoveis.Models
{
    public class ResetSenhaToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Email { get; set; }
        public string Token { get; set; }
        public DateTime Expiracao { get; set; }
    }

}
