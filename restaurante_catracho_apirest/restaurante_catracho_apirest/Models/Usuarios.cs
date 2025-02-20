namespace restaurante_catracho_apirest.Models
{
    public class Usuarios
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; }
        public string correo { get; set; }
        public string clave { get; set; }
        public string rol { get; set; }
        public string telefono { get; set; }
        public string direccion { get; set; }
    }

    public class ActualizarClaveRequest
    {
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }
        public string NuevaClave { get; set; }
    }
}
