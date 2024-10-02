namespace Comanda.Api.Dtos_data_transfer_object_
{
    public class PedidoCozinhaGetDto
    {
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; } = default!;
        //default! = ""

        public string Titulo { get; set; } = default!;
    }
}
