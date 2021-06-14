using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Objeto que se utiliza para almacenar los datos de los empleados que mostrarán en el DataGrid
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Entidades
{
    public class DTOEmpleado
    {
        public int idEmpleado { get; set; }
        public string nombreDepartamento { get; set; }
        public string correo { get; set; }
        public string usuario { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public System.DateTime fechaNacimiento { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string pais { get; set; }
        public int telefono { get; set; }
        public byte[] foto { get; set; }

        //Contructor vacio
        public DTOEmpleado()
        { }

        //Entidad que se utilizará para cargar los datos en el datagrid
        public DTOEmpleado(int idEmpleado,string nombreDepartamento,string correo,string usuario, string nombre, string apellido1, string apellido2, 
            DateTime fechaNacimiento, string direccion, string ciudad, string pais, int telefono)
        {
            this.idEmpleado = idEmpleado;
            this.nombreDepartamento = nombreDepartamento;
            this.correo = correo;
            this.usuario = usuario;
            this.nombre = nombre;
            this.apellido1 = apellido1;
            this.apellido2 = apellido2;
            this.fechaNacimiento = fechaNacimiento;
            this.direccion = direccion;
            this.ciudad = ciudad;
            this.pais = pais;
            this.telefono = telefono;
        }
    }
}
