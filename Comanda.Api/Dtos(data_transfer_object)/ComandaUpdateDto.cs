namespace Comanda.Api.Dtos_data_transfer_object_
{
    public class ComandaUpdateDto
    {
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; }
        // propriedade Array(vetor)int
        public ComandaItemUpdateDto[] ComandaItens { get; set; } = [];
    }

    public class ComandaItemUpdateDto
    {
        public int Id { get; set; }
        public bool excluir { get; set; } = false;
        public bool incluir { get; set; } = false;
        public int cardapioItemId {get; set;}
    }
}
