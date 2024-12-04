namespace Comanda.Api.Dtos_data_transfer_object_
{
    public class UsuarioResponse
    {
        public int idUsuario { get; set; }
        public string emailUsuario { get; set; } = default!;
        public string token { get; set; } = default!;
    }
}
