using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Objeto que se utiliza para almacenar los datos de los pedidos y se que se mostrarán en el DataGrid
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Entidades
{
    public class DTOPedido
    {
        public int idPedido { get; set; }
        public int idCliente { get; set; }
        public string nombreApellidos { get; set; }
        public string fechaPedido { get; set; }
        public string fechaEntrega { get; set; }
        public string estado { get; set; }
        public Decimal PrecioFinal { get; set; }

        public DTOPedido(int idPedido, int idCliente, string nombreApellido, DateTime fechaPedido, DateTime fechaEntrega, string estado, Decimal PrecioFinal)
        {
            this.idPedido = idPedido;
            this.idCliente = idCliente;
            this.nombreApellidos = nombreApellido;
            this.fechaPedido = fechaPedido.ToShortDateString();
            this.estado = estado;
            this.PrecioFinal = PrecioFinal;

            //Comrpobamos si hay fecha de entreaga
            if (fechaEntrega == null)
                this.fechaEntrega = "Sin entregar";
            else
                this.fechaEntrega = fechaEntrega.ToShortDateString();
        }
    }
}
