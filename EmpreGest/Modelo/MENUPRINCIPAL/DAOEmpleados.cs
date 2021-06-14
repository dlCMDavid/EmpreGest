using EmpreGest.Contexto;
using EmpreGest.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Logica que se utiliza en la vista correspondiente a los empleados
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Modelo
{
    public class DAOEmpleados
    {
        //Método que obtiene las categorias
        public List<Departamento> ObtenerDepartamentos()
        {
            List<Departamento> lstDepartamentos = new List<Departamento>();
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from departamento in contexto.Departamento
                                   select departamento;

                    //Recorremos la consulta añadiendo los departamentos
                    foreach (var item in consulta)
                        lstDepartamentos.Add(item);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la lista
            return lstDepartamentos;
        }

        //Metodo que obtiene el nombre de un departamento a partir de su id
        public string NombreDepartamento(int idDepartamento)
        {
            string nombreDepartamentos = null;
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from departamento in contexto.Departamento
                                   where idDepartamento == departamento.idDepartamento
                                   select departamento.descripcion).FirstOrDefault();

                    //Recorremos la consulta añadiendo los departamentos
                    nombreDepartamentos = consulta;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la lista
            return nombreDepartamentos;
        }

        //Metodo que añade el empleado en la base de datos 
        public void InsertarEmpleado(Empleado objEmpleado) 
        {
            string contraseña = null;
            string contraseñaEncriptada = null;
            try
            {
                //Generamos la contraseña
                contraseña = this.GenerarContraseña();

                //Obtenemos la contraseña encriptada
                contraseñaEncriptada = this.EncriptarSHA256(contraseña);

                //Encriptamos la contraseña y la añadimos al dto
                objEmpleado.contraseña = contraseñaEncriptada;

                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    //Añadimos el empleado y guardamos el contexto
                    contexto.Empleado.Add(objEmpleado);
                    contexto.SaveChanges();
                }

                //Al pasar el empleado por referencia le añado la contraseña sin encriptar para posteriormente enviarla por correo
                objEmpleado.contraseña = contraseña;
            }
            catch (DbUpdateException)
            {
                throw new Exception("El empleado ya se encuentra en la base de datos");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que obtiene una lista con los empleados
        public List<DTOEmpleado> ObtenerEmpleados()
        {
            List<DTOEmpleado> lstEmpleados = new List<DTOEmpleado>();
            DTOEmpleado objEmpleado = null;
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from empleado in contexto.Empleado
                                   from departamento in contexto.Departamento
                                   where empleado.activo == true && empleado.idDepartamento == departamento.idDepartamento
                                   select new { empleado.idEmpleado, departamento.descripcion, empleado.correo, empleado.usuario, empleado.nombre,
                                                empleado.apellido1,empleado.apellido2, empleado.fechaNacimiento,empleado.direccion,empleado.ciudad,
                                                empleado.pais, empleado.telefono};

                    //Recorremos la consulta añadiendo los departamentos
                    foreach (var item in consulta)
                    {
                        //Cremaos un nuevo empleado y le añadimos los datos
                        objEmpleado = new DTOEmpleado(item.idEmpleado, item.descripcion, item.correo, item.usuario, item.nombre,
                                                item.apellido1, item.apellido2, item.fechaNacimiento.Date, item.direccion, item.ciudad,
                                                item.pais, item.telefono);

                        lstEmpleados.Add(objEmpleado);
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la lista
            return lstEmpleados;
        }

        //Metodo que obtiene un empleado a partir de su id
        public Empleado ObtenerEmpleado(int id)
        {
            Empleado objEmpleado = null;
            
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from empleado in contexto.Empleado
                                   where empleado.idEmpleado == id
                                   select empleado).FirstOrDefault();

                    //Almacenamos el empledo en el dto
                    objEmpleado = consulta;
                     
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos el empleado
            return objEmpleado;
        }

        //Metodo que modifica un Empleado
        public void ModificarEmpleado(Empleado objEmpleado)
        {
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from empleado in contexto.Empleado
                                    where objEmpleado.idEmpleado == empleado.idEmpleado
                                    select empleado).FirstOrDefault();

                    //Modificamos el empleado
                    consulta.idDepartamento = objEmpleado.idDepartamento;
                    consulta.correo = objEmpleado.correo;
                    consulta.usuario = objEmpleado.usuario;
                    consulta.contraseña = objEmpleado.contraseña;
                    consulta.nombre = objEmpleado.nombre;
                    consulta.apellido1 = objEmpleado.apellido1;
                    consulta.apellido2 = objEmpleado.apellido2;
                    consulta.fechaNacimiento = objEmpleado.fechaNacimiento;
                    consulta.direccion = objEmpleado.direccion;
                    consulta.ciudad = objEmpleado.ciudad;
                    consulta.pais = objEmpleado.pais;
                    consulta.telefono = objEmpleado.telefono;
                    consulta.foto = objEmpleado.foto;

                    //Guardamos los cambios
                    contexto.SaveChanges();
                }
            }
            catch (DbUpdateException)
            {
                throw new Exception("El empleado ya se encuentra en la base de datos");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que recibe una lista de empleados y los borra
        public void BorrarEmpleado(DTOEmpleado objEmpleados)
        {
            try
            {
                //Creamos el contexto e modificamos los campos para eliminarlo a nivel logica
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from empleado in contexto.Empleado
                                    where objEmpleados.idEmpleado == empleado.idEmpleado
                                    select empleado).FirstOrDefault();


                    //Modificamos los campos
                    consulta.activo = false;
                    consulta.correo = null;
                    consulta.usuario = null;

                    //Guardamos los cambios
                    contexto.SaveChanges();
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que recibe el id de un usuario y modificar la contraseña
        public Empleado ResetearContraseña(Empleado objEmpleado)
        {
            string contraseña = null;
            string contraseñaEncriptada = null;
            Empleado miEmpleado = null;
            try
            {

                using (var contexto = new DBEmpreGestEntities())
                {
                    //Obtenemos el usuario si se encuentra en la base de datos
                    var consulta = (from empleado in contexto.Empleado
                                    where (objEmpleado.idEmpleado.Equals(empleado.idEmpleado))
                                    select empleado).FirstOrDefault();

                    //Comprobamos la consulta encontró al usuario
                    if (consulta != null)
                    {
                        //Obtenemos una contraseña aleatoria
                        contraseña = this.GenerarContraseña();

                        //Obtenemos la contraseña encriptada
                        contraseñaEncriptada = this.EncriptarSHA256(contraseña);

                        //La añadimos al usuario la contraseña encriptada
                        consulta.contraseña = contraseñaEncriptada;

                        //Guardamos los cambios en el contexto
                        contexto.SaveChanges();

                        //Le añadimos al empleado que recibimos la contraseña encriptada para mantenerlo actualizado
                        objEmpleado.contraseña = contraseñaEncriptada;

                        //Le añadimos al usuario la contraseña sin encriptar para enviarla por correo
                        consulta.contraseña = contraseña;
                    }

                    //Añadimmos la consulta ala entidad
                    miEmpleado = consulta;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la entidad
            return miEmpleado;
        }

        //Metodo que genera una contraseña aleatoriamente y la devuelte
        public string GenerarContraseña()
        {
            string caracteres = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890%$#@";
            Random rdn = null;

            //Tamaño de la contraseña que generaremos
            int longitudContraseña = 16;

            //Letra que almacenará el caracter actaul que se añadira
            char letra;

            //Cadena que almacenará la contraseña
            string contraseñaGenerada = null;
            try
            {
                //Metodo que obtendra un numero aleatorio
                rdn = new Random();

                //Busque que se repetirá tantas veces como el tamaño de la contraseña decidamos
                for (int i = 0; i < longitudContraseña; i++)
                {
                    //Obtenemos una letra aleatoria de la cadena con los caracteres y le añadimos a la cadena la letra
                    letra = caracteres[rdn.Next(caracteres.Length)];
                    contraseñaGenerada += letra;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            return contraseñaGenerada;
        }

        //Metodo que crea un hash de una contraseña con el algoritmo SHA256 para guardarla en la base de datos o para comprobar si es correcta con una ya introducida
        public string EncriptarSHA256(string contraseña)
        {
            //Utilizaremos SHA256 para encriptar la contraseña
            SHA256 sha256 = null;

            //Cadena de bytes que almacenaran el hash generado por el algoritmo SHA256
            byte[] stream = null;

            //Obtendrá la codificación UTF8 de la cadena de la contraseña enviada
            UTF8Encoding codificacion = new UTF8Encoding();

            //ALmacenará la contraseña que se grabará en la base de datos
            StringBuilder sb = null;

            try
            {
                //Creamos una instancia del algoritmo
                using (sha256 = SHA256Managed.Create())
                {
                    sb = new StringBuilder();

                    //Añadimos al array de bytes la contraseña encriptada
                    stream = sha256.ComputeHash(codificacion.GetBytes(contraseña));

                    //Recorremos el array de bytes cogiendo dos valores, pasandolos a hexadecimal y guardandolo en el stringBuilder
                    for (int i = 0; i < stream.Length; i++)
                        sb.AppendFormat("{0:x2}", stream[i]);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la cadena que grabaremos en la base de datos
            return sb.ToString();
        }
    }
}
