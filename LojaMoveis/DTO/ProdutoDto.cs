namespace LojaMoveis.DTO
{
    public class ProdutoDto
    {
        public string Nome { get; set; } = null!;
        public string Categoria { get; set; } = null!;
        public decimal Preco { get; set; }
        public string Descricao { get; set; } = null!;
    }
}
