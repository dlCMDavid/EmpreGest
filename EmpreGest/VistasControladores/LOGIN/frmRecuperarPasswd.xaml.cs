using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using EmpreGest.Contexto;
using System.Net;
using EmpreGest.Entidades;
using EmpreGest.Modelo;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Controlador correspondiente a la ventana de recuperar contraseña
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    public partial class frmRecuperarPasswd : Window
    {
        //Realiza los operaciones con la base de datos
        DAOLogin objDAOLogin = new DAOLogin();
        DAOCorreo objDAOCorreo = new DAOCorreo();

        public frmRecuperarPasswd()
        {
            InitializeComponent();
        }

        //Cuando se sale del textbox, si el texto esta vacio
        private void tbxCorreo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (tbxCorreo.Text.Length == 0)
            {
                tbxCorreo.Text = "Introduzca el usuario/correo";
                tbxCorreo.Foreground = Brushes.Gray;
            }
        }

        //Si se entra al textbox y el texto es Introduza el correo lo borramos, cambiamos el color y añadimos la limitación de tamaño
        private void tbxCorreo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (tbxCorreo.Text == "Introduzca el usuario/correo")
            {
                tbxCorreo.Text = "";
                tbxCorreo.Foreground = Brushes.Black;
            }
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Comprueba si el correo o el usuario se encuentran en la base de datos y envia un correo al usuairo con la contraseña
        private void btnRecuperar_Click(object sender, RoutedEventArgs e)
        {
            Empleado objEmpleado = null;
            try
            {
                //Comprobamos que el campo correo no este vacio
                if (!tbxCorreo.Text.Equals("Introduzca el usuario/correo") && tbxCorreo.Text.Length > 0)
                {
                    //Obtenemos el dto
                    objEmpleado = objDAOLogin.RecuperarContraseña(tbxCorreo.Text.Trim());

                    //Comprobamos si la entidad es igual null
                    if (objEmpleado != null)
                    {
                        //Enviamos el correo
                        objDAOCorreo.CorreoRecuperarContraseña(objEmpleado);

                        //Mostramos un mensaje de que se envio correctamente
                        this.MostrarMensajes("El correo se envio correctamente");

                        //Añadimos el hint al correo
                        tbxCorreo.Text = "Introduzca el usuario/correo";
                        tbxCorreo.Foreground = Brushes.Gray;
                    }
                    else
                        this.MostrarMensajes("El usuario no se encuentra en el sistema");
                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Cierra el formulario
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Si se pulsa enter pulsamos el boton de Recuperar
        private void PulsarEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnRecuperar_Click(this, null);
        }

        //Método que muestra los mensajes de error por pantalla
        private void MostrarMensajes(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso de la aplicación", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
    }
}
