using EmpreGest.Contexto;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Logica de negocio que se utiliza en la ventana correspondiente al Login
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Entidades
{
    public class DAOLogin
    {
        //Metodo que obtiene el usuario correspondiente al correo o usuario recibidio con una contraseña nueva
        public Empleado RecuperarContraseña(string correoUsuario)
        {
            Empleado objDAOEmpleado = null;
            string contraseña = null;
            string contraseñaEncriptada = null;

            try
            {
                
                using (var contexto = new DBEmpreGestEntities())
                {
                    //Obtenemos el usuario si se encuentra en la base de datos
                    var consulta = (from empleado in contexto.Empleado
                                    where ((correoUsuario.Equals(empleado.correo) || correoUsuario.Equals(empleado.usuario)) && empleado.activo == true)
                                    select empleado).FirstOrDefault();

                    //Comprobamos la consulta encontró al usuario
                    if (consulta != null)
                    {
                        //Obtenemos una contraseña aleatoria
                        contraseña = this.GenerarContraseña();

                        //La encriptamos
                        contraseñaEncriptada = this.EncriptarSHA256(contraseña);

                        //La añadimos al usuario
                        consulta.contraseña = contraseñaEncriptada;

                        //Guardamos los cambios en el contexto
                        contexto.SaveChanges();

                        //Le añadimos al usuario la contraseña sin encriptar para enviarla por correo
                        consulta.contraseña = contraseña;
                    }

                    //Añadimmos la consulta ala entidad
                    objDAOEmpleado = consulta;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la entidad
            return objDAOEmpleado;
        }

        //Metodo que genera una contraseña aleatoriamente y la devuelte
        private String GenerarContraseña()
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
            catch(Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la cadena que grabaremos en la base de datos
            return sb.ToString();
        }

        //Metodo que devuelve un dto con los datos que se utilizaran en el login
        public Empleado ObtenerEmpleado(string correoUsuario)
        {
            Empleado objEmpleado = null;

            try
            {
                //Realizamos la consulta para comprobar si el usuario se encuentra en la base de datos y obtiene el correo y la contraseña
                using (var contexto = new DBEmpreGestEntities())
                {
                    //Realizamos una consulta para comprobar si el usuario existe en la base de datos, si existe obtenemos su constraseña
                    var consulta = (from empleado in contexto.Empleado
                                           where ((correoUsuario.Equals(empleado.correo) || correoUsuario.Equals(empleado.usuario)) && empleado.activo == true)
                                           select empleado).FirstOrDefault();

                    //Comprobamos si la consulta no esta vacia
                    if (consulta != null)
                        objEmpleado = consulta;
                }
            }


            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la entidad
            return objEmpleado;
        }
    }
}
