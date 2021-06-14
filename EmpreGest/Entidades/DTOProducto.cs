using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Objeto que se utiliza para almacenar los datos de los prodcutos que se mostraran en el DataGrid
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Entidades
{
    public class DTOProducto
    {
        public int idProducto { get; set; }
        public string nombreCategoria { get; set; }
        public string descripcion { get; set; }
        public int unidadesStock { get; set; }
        public int unidadesPedidas { get; set; }
        public int unidadesReservadas { get; set; }
        public int stockMinimo { get; set; }
        public string reponer { get; set; }

        public DTOProducto()
        {}

        // Se utilizará para cargar el datagrid correspondiente a la gestion de los productos(Stock)
        public DTOProducto(int idProducto, string nombreCategoria, string descripcion, int unidadesStock, int unidadesPedidas,int unidadesReservadas, int stockMinimo, bool reponer)
        {
            this.idProducto = idProducto;
            this.nombreCategoria = nombreCategoria;
            this.descripcion = descripcion;
            this.unidadesStock = unidadesStock;
            this.unidadesPedidas = unidadesPedidas;
            this.unidadesReservadas = unidadesReservadas;
            this.stockMinimo = stockMinimo;

            //Si reponer es igual a true lo convertimos en Si
            if (reponer) this.reponer = "Sí";
            else this.reponer = "No";
        }
    }
}
