namespace Comanda.Api.Dtos_data_transfer_object_
{
    public class ComandaUpdateDto
    {
        public int Id { get; set; }
        public int NumeroMesa { get; set; }
        public string NomeCliente { get; set; }
        // propriedade Array(vetor)int
        public CardapioItemDtoPut[] CardapioItems { get; set; } = [];
    }

    public class CardapioItemDtoPut
    {
        public int Id { get; set; }
        public bool excluir { get; set; }
        public int cardapioItemId {get; set;}
    }
}
