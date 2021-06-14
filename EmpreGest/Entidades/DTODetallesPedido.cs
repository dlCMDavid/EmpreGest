using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Objeto que se utiliza para almacenar los datso que se muestran en el datagrid correspondiente a los detalles de los pedidos
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Entidades
{
    public class DTODetallesPedido
    {
        public int idProducto { get; set; }
        public string nombreCategoria { get; set; }
        public string descripcion { get; set; }
        public decimal precioUnitario { get; set; }
        public int cantidad { get; set; }
        public decimal precioFinal { get; set; }

        public DTODetallesPedido(int idProducto, string descripcion, string nombreCategoria, decimal precioUnitario, int cantidad, decimal precioFinal)
        {
            this.idProducto = idProducto;
            this.descripcion = descripcion;
            this.nombreCategoria = nombreCategoria;
            this.precioUnitario = precioUnitario;
            this.cantidad = cantidad;
            this.precioFinal = precioFinal;
        }
    }
}
