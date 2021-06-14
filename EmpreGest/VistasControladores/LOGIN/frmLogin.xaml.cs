using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using EmpreGest.Contexto;
using EmpreGest.Entidades;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Controlador correspondiente a la ventana login
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    public partial class frmLogin : Window
    {
        //Realiza las operaciones con la base de datos
        DAOLogin objDAOLogin = new DAOLogin();

        public frmLogin()
        {
            InitializeComponent();
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Se produce cuando se pulsa en contraseña olvidada y abre un formulario que enviar una nueva contraseña al correo
        private void tbkPassOlvidada_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            //abre le formulario de recuperar contraseña
            frmRecuperarPasswd objFrmRecuperarPasswd = null;
          
            try
            {
                //Ocultamos el formulario
                this.Hide();

                //Llamamos al fromulario
                objFrmRecuperarPasswd = new frmRecuperarPasswd();
                objFrmRecuperarPasswd.ShowDialog();

                //Mostramos el formulario
                this.Show();
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

        //Se produce cuando se sale del pabPasswd y realiza el hint
        private void pabPasswd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (pabPasswd.Password.Length == 0)
            {
                pabPasswd.Password = "contraseña";
                pabPasswd.Foreground = Brushes.Gray;
            }
        }
        
        //Se produce cuando se entra en pabPasswd y realiza el hint
        private void pabPasswd_GotFocus(object sender, RoutedEventArgs e)
        {
            if (pabPasswd.Password == "contraseña")
            {
                pabPasswd.Password = "";
                pabPasswd.Foreground = Brushes.Black;
            }
        }

        //Valida la contraseña y el usuario/correo y entra en la aplicación
        private void btnEntrar_Click(object sender, RoutedEventArgs e)
        {
            frmMenuPrincipal objFrmPrincipal = null;
            Empleado objEmpleado = null;
            string contreñaEncriptada = null;

            try
            {
                if (this.validarTextBox())
                {
                    //Obtenemos los datos del usuario que se va a loguea
                    objEmpleado = objDAOLogin.ObtenerEmpleado(tbxCorreo.Text.Trim());

                    //Comprobamos si la consulta es null
                    if (objEmpleado != null)
                    {
                        //Generamos el hash de la contraseña introducida por el usuario y lo comparamos con la de la base de datos
                        contreñaEncriptada = objDAOLogin.EncriptarSHA256(pabPasswd.Password.Trim());
                        //Comprobamos si la contraseña es valida , si es asi entramos en la aplicación
                        if (objEmpleado.contraseña.ToLower().Equals(contreñaEncriptada))
                        {

                            //Ocultamos el formulario
                            this.Hide();

                            //Llamamos al formulario esperando respuesta
                            objFrmPrincipal = new frmMenuPrincipal(objEmpleado);
                            bool resultado = (bool)objFrmPrincipal.ShowDialog();

                            //Si se devuelve que se cerro sesion, limpiamos los textbox
                            if (resultado == true)
                            {
                                //Mostramos el formulario
                                this.Show();

                                //Añadimos los hint
                                this.InsetarHint();
                            }
                            else
                                //Si no cerramos el formulario
                                this.Close();
                        }
                        else
                            this.MostrarMensajes("La contraseña no es correcta para ese usuario");
                    }
                    else
                        this.MostrarMensajes("El usuario no se encuentra en la base de datos");
                }
                else
                    this.MostrarMensajes("No puede dejar ningun campo vacio");
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que añade los hint a los textbox
        private void InsetarHint()
        {
            //Correo
            tbxCorreo.Text = "Introduzca el usuario/correo";
            tbxCorreo.Foreground = Brushes.Gray;

            //Contraseña
            pabPasswd.Password = "contraseña";
            pabPasswd.Foreground = Brushes.Gray;

        }
        //Método que comprueba si los textbox estan vacios
        private bool validarTextBox()
        {
            //Comprobamos que el campo correo no este vacio
            if ((!tbxCorreo.Text.Equals("Introduzca el usuario/correo") && tbxCorreo.Text.Length > 0) &&
                (!pabPasswd.Password.Equals("contraseña") && pabPasswd.Password.Length > 0))
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //Método que muestra los mensajes de error por pantalla
        private void MostrarMensajes(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso de la aplicación", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        //Si se pulsa enter pulsamos el boton de entrar
        private void PulsarEnter_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnEntrar_Click(this, null);
        }
    }
}
