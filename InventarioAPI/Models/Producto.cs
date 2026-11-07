namespace InventarioAPI.Models
{
    public class Producto : BaseModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public double Precio { get; set; }
        public int Stock { get; set; }
        public string Categoria { get; set; } = string.Empty;
        public List<string> Etiquetas { get; set; } = new();

        //  proveedor embebido requerido por la tarea
        public Proveedor Proveedor { get; set; } = new Proveedor();
    }

    public class Proveedor
    {
        public string Nombre { get; set; } = string.Empty;
        public string Contacto { get; set; } = string.Empty;
    }
}