using EmpreGest.Contexto;
using EmpreGest.Entidades;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Logica de negocio que se utiliza para enviar los correos de la aplicacion
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Modelo
{
    public class DAOCorreo
    {

        DAOEmpleados objDAOEmpleado = new DAOEmpleados();

        //Método que envia un correo al usuario con la contraseña
        public void CorreoRecuperarContraseña(Empleado objEmpleado)
        {
            //Datos del correo
            MailMessage correo = null;
            string emisorCorreo = "proyectoempregest@gmail.com";
            string emisonNombre = "David de la Cruz";
            string asunto = "Contraseña olvidada en EmpreGest ( " + DateTime.Now.ToString() + " ) ";
            string cuerpo = "Sus datos de inicio de sesión son: <br> Correo = " + objEmpleado.correo + "<br> Usuario = " + objEmpleado.usuario +  "<br> Contraseña = " + objEmpleado.contraseña + "<br> Gracias por trabajar con nosotros";

            try
            {
                //Creamos el mensaje de correo
                correo = new MailMessage();
                correo.To.Add(new MailAddress(objEmpleado.correo));
                correo.From = new MailAddress(emisorCorreo, emisonNombre);
                correo.Subject = asunto;
                correo.Body = cuerpo;
                correo.IsBodyHtml = true;
                correo.BodyEncoding = Encoding.UTF8;

                //Enviamos el mensaje
                this.EnviarCorreo(correo);

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Método que envia un correo al usuario con la contraseña
        public void CorreoModificarDatos(Empleado objEmpleado)
        {
            //Datos del correo
            MailMessage correo = null;
            string emisorCorreo = "proyectoempregest@gmail.com";
            string emisonNombre = "David de la Cruz";
            string asunto = "Datos Modificados en EmpreGest ( " + DateTime.Now.ToString() + " ) ";
            string cuerpo = "Sus datos han sido modificados, actualmente son: " +
                "<br> Correo = " + objEmpleado.correo + 
                "<br> Usuario = " + objEmpleado.usuario + 
                "<br> Nombre = " + objEmpleado.nombre +
                "<br> Departamento = " + objDAOEmpleado.NombreDepartamento(objEmpleado.idDepartamento) +
                "<br> Primer apellido = " + objEmpleado.apellido1 +
                "<br> Segundo apellido = " + objEmpleado.apellido2 +
                "<br> Fecha de nacimiento = " + objEmpleado.fechaNacimiento +
                "<br> Dirección = " + objEmpleado.direccion +
                "<br> Ciudad = " + objEmpleado.ciudad +
                "<br> País = " + objEmpleado.pais +
                "<br> Teléfono = " + objEmpleado.telefono +
                "<br> Gracias por trabajar con nosotros";

            try
            {
                //Creamos el mensaje de correo
                correo = new MailMessage();
                correo.To.Add(new MailAddress(objEmpleado.correo));
                correo.From = new MailAddress(emisorCorreo, emisonNombre);
                correo.Subject = asunto;
                correo.Body = cuerpo;
                correo.IsBodyHtml = true;
                correo.BodyEncoding = Encoding.UTF8;

                //Enviamos el mensaje
                this.EnviarCorreo(correo);

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Método que envia un correo al usuario indicandole que se encuentra de alta en el programa
        public void CorreoInsertarUsuario(Empleado objEmpleado)
        {
            //Datos del correo
            MailMessage correo = null;
            string emisorCorreo = "proyectoempregest@gmail.com";
            string emisonNombre = "David de la Cruz";
            string asunto = "Bienvenido a EmpreGest ( " + DateTime.Now.ToString() + " ) ";
            string cuerpo = "Sus datos de inicio de sesión son: <br> Correo = " + objEmpleado.correo + "<br> Usuario = " + objEmpleado.usuario + "<br> Contraseña = " + objEmpleado.contraseña + "<br> Gracias por trabajar con nosotros";

            try
            {
                //Creamos el mensaje de correo
                correo = new MailMessage();
                correo.To.Add(new MailAddress(objEmpleado.correo));
                correo.From = new MailAddress(emisorCorreo, emisonNombre);
                correo.Subject = asunto;
                correo.Body = cuerpo;
                correo.IsBodyHtml = true;
                correo.BodyEncoding = Encoding.UTF8;

                //Enviamos el mensaje
                this.EnviarCorreo(correo);

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Método que envia un mensaje notificando al empleado que ha sido despedido
        public void CorreoDepedirUsuario(DTOEmpleado objEmpleado)
        {
            //Datos del correo
            MailMessage correo = null;
            string emisorCorreo = "proyectoempregest@gmail.com";
            string emisonNombre = "David de la Cruz";
            string asunto = "Despido de EmpreGest ( " + DateTime.Now.ToString() + " ) ";
            string cuerpo = "Lo sentimos " + objEmpleado.nombre + " " + objEmpleado.apellido1 + " " + objEmpleado.apellido2  + ", pero hemos decidido depedirte...<br> Gracias por haber trabajado para nosotros";

            try
            {
                //Creamos el mensaje de correo
                correo = new MailMessage();
                correo.To.Add(new MailAddress(objEmpleado.correo));
                correo.From = new MailAddress(emisorCorreo, emisonNombre);
                correo.Subject = asunto;
                correo.Body = cuerpo;
                correo.IsBodyHtml = true;
                correo.BodyEncoding = Encoding.UTF8;

                //Enviamos el mensaje
                this.EnviarCorreo(correo);

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que reibe un mailMessage y lo envia
        private void EnviarCorreo(MailMessage correo)
        {
            SmtpClient smtp = null;
            string correoNetworkCredential = null;
            string contraseñaNetworkCredential = null;
            try
            {
                //Obtenemos el correo y la contraseña del archivo appconfig
                correoNetworkCredential = ConfigurationManager.AppSettings["Correo"].ToString();
                contraseñaNetworkCredential = ConfigurationManager.AppSettings["Contraseña"].ToString();

                //Creamos el cliente de correo
                using (smtp = new SmtpClient())
                {
                    smtp.Credentials = new NetworkCredential(correoNetworkCredential, contraseñaNetworkCredential);
                    smtp.Port = 587;
                    smtp.EnableSsl = true;
                    smtp.Host = "smtp.gmail.com";


                    //Enviamos el correo
                    smtp.Send(correo);
                    correo.Dispose();
                }
            }
            catch (SmtpException error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}
