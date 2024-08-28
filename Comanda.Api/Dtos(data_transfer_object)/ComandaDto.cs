namespace Comanda.Api.Dtos_data_transfer_object_
{
    //serve pra transferir dados
    //dados que nos interesa
    public class ComandaDto
    {
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; }
        // propriedade Array(vetor)int
        public int[] CardapioItems { get; set; } = [];
    }
}
