//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ConsoleApp1
{
    using System;
    using System.Collections.Generic;
    
    public partial class Empleado
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Empleado()
        {
            this.Pedido = new HashSet<Pedido>();
        }
    
        public int idEmpleado { get; set; }
        public int idDepartamento { get; set; }
        public bool activo { get; set; }
        public string correo { get; set; }
        public string usuario { get; set; }
        public string contraseña { get; set; }
        public string nombre { get; set; }
        public string apellido1 { get; set; }
        public string apellido2 { get; set; }
        public System.DateTime fechaNacimiento { get; set; }
        public string direccion { get; set; }
        public string ciudad { get; set; }
        public string pais { get; set; }
        public int telefono { get; set; }
        public byte[] foto { get; set; }
    
        public virtual Departamento Departamento { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Pedido> Pedido { get; set; }
    }
}
