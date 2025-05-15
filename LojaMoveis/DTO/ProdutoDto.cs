namespace LojaMoveis.DTO
{
    public class ProdutoDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Descricao { get; set; } = string.Empty;
        public string ImagemUrl { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public double Avaliacao { get; set; }
    }
}
