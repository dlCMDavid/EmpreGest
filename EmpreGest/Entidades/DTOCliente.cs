using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Objeto que se utiliza para almacenar los datos del cliente que se muestran en un combobox
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Entidades
{
    public class DTOCliente
    {
        public int idCliente { get; set; }
        public string nombreApellidos { get; set; }

        //Se utiliza para cargar los datos del cliente en el combobox de los pedidos
        public DTOCliente(int idCliente, string nombre, string apellido1, string apellido2)
        {
            this.idCliente = idCliente;
            this.nombreApellidos = nombre + ", " + apellido1 + " " + apellido2;
        }
    }
}
