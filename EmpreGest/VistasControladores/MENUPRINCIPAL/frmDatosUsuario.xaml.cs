using EmpreGest.Contexto;
using EmpreGest.Modelo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
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

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Controlador correspondiente a la ventana que muestra los datos del usuario que se encuentra autentificado en la aplicacion
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    public partial class frmDatosUsuario : Window
    {
        //Realiza las operaciones en la base de dtaos
        DAOEmpleados objDAOEmpleados = new DAOEmpleados();

        //Envia los correos de la aplicación
        DAOCorreo objDAOCorreo = new DAOCorreo();

        //Almacena el empleado
        Empleado objEmpleado = null;
       

        //Contructor para modificar
        public frmDatosUsuario(int idEmpleado)
        {
            InitializeComponent();

            try
            {
                //Cargamos los datos
                objEmpleado = objDAOEmpleados.ObtenerEmpleado(idEmpleado);

                //Cargamos los datos a los componentes
                this.CargarDatosControles(objEmpleado);
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
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

        //Metodo que carga los datos en los controles
        private void CargarDatosControles(Empleado objEmpleado)
        {
            try
            {
                //Comprobamos si el tag es null, si es asi significa que no se cambio la imagen
                if (objEmpleado.foto != null)
                {
                    imvEmpleadoImagen.Source = this.ConvertirByteAImagen(objEmpleado.foto);
                }

                //Resto de campos
                tbxEmpleadoCorreo.Text = objEmpleado.correo.ToString();
                tbxEmpleadoNombreUsuario.Text = objEmpleado.usuario;
                tbxEmpleadoNombre.Text = objEmpleado.nombre;
                tbxEmpleadoDepartamento.Text = objDAOEmpleados.NombreDepartamento(objEmpleado.idDepartamento);
                tbxEmpleadoPrimerApellido.Text = objEmpleado.apellido1;
                tbxEmpleadoSegundoApellido.Text = objEmpleado.apellido2;
                tbxEmpleadoFechaNacimiento.Text = objEmpleado.fechaNacimiento.ToLongDateString();
                tbxEmpleadoDireccion.Text = objEmpleado.direccion;
                tbxEmpleadoCiudad.Text = objEmpleado.ciudad;
                tbxEmpleadoPais.Text = objEmpleado.pais;
                tbxEmpleadoTelefono.Text = objEmpleado.telefono.ToString();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Método que permite buscar una imagen y cargarla
        private void btnBuscarImagen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Imagenes PNG(.png)|*.png";
            opf.Title = "Buscar imagen en el disco";

            try
            {
                //Mostramos el dialogo esperando respuesta, que si es positiva añadimos la imagen al correspondiente image
                if (opf.ShowDialog() == true)
                {
                    //Comprobamos que control envio el mensaje
                    if (sender == btnEmpleadoImagen)
                    {
                        imvEmpleadoImagen.Source = new BitmapImage(new Uri(opf.FileName));
                        imvEmpleadoImagen.Tag = opf.FileName;
                    }
                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }

        }

        //Metodo que solo permite que se introduzcan numeros, Se utiliza principalmente para los numeros de telefono
        private void tbxNumerosEnteros_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        //Metodo que bloquea el uso de espacions en el correo
        private void tbxImpedirEspacios_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key != Key.Space))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        //Método que valida si el correo es valido
        private bool ValidarCorreo(String correo)
        {
            try
            {
                new MailAddress(correo);
                return true;
            }
            catch
            {
                return false;
            }
        }

        //Método que rellena los datos del dto
        private void ModificarDTOEmpleado(Empleado objEmpleado)
        {
            try
            {
                //Comprobamos si el tag es null, si es asi significa que no se cambio la imagen
                if (imvEmpleadoImagen.Tag != null)
                    objEmpleado.foto = this.ConvertirImagenAByte(imvEmpleadoImagen.Tag.ToString());


                //Deshabilitamos el checkbox para qeu se gaurdae la contraseña en el passwordbox
                cbxMostrarPasswd.IsChecked = false;

                //Comprobamos si la comtraseña cambio, si es asi la encriptamos y la añadimos
                if (pabPasswd.Password != "contraseña")
                {
                    objEmpleado.contraseña =  objDAOEmpleados.EncriptarSHA256(pabPasswd.Password);
                }

                //Resto de campos
                objEmpleado.correo = tbxEmpleadoCorreo.Text.Trim();
                objEmpleado.usuario = tbxEmpleadoNombreUsuario.Text.Trim();
                objEmpleado.direccion = tbxEmpleadoDireccion.Text.Trim();
                objEmpleado.ciudad = tbxEmpleadoCiudad.Text.Trim();
                objEmpleado.pais = tbxEmpleadoPais.Text.Trim();
                objEmpleado.telefono = Convert.ToInt32(tbxEmpleadoTelefono.Text.Trim());
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Controla el boton de mostrar contraseña
        private void cbxMostrarOcultarContraseña_Changed(object sender, RoutedEventArgs e)
        {
            if (pabPasswd.Password != "contraseña")
            {
                //Si esta marcado mostramos la contraseña
                if (cbxMostrarPasswd.IsChecked == true)
                {
                    tbxPasswd.Text = pabPasswd.Password;
                    pabPasswd.Visibility = Visibility.Hidden;
                    tbxPasswd.Visibility = Visibility.Visible;
                }
                else
                {
                    pabPasswd.Password = tbxPasswd.Text;
                    pabPasswd.Visibility = Visibility.Visible;
                    tbxPasswd.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                //Desmarcamos
                cbxMostrarPasswd.IsChecked = false;
            }
        }

        //Metodo que controla los botones de la ventana
        private void btnModificar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Validamos los campos
                if (this.ValidarCamposEmpleado())
                {
                    //Modificamos el emleado
                    this.ModificarEmpleado();

                    //Mostrmaos que se añadio con exito
                    this.MostrarMensajes("Se ha modificado con exito");

                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);

            }
        }

        //Metodo que modifica un empleado de la base de datos
        private void ModificarEmpleado()
        {

            try
            {
                //Rellenamois el dto atributo que corresponde al que recibimos
                this.ModificarDTOEmpleado(objEmpleado);

                //Insertamos el campo
                objDAOEmpleados.ModificarEmpleado(objEmpleado);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        //valida los campos antes de ser enviados a la base de datos
        private bool ValidarCamposEmpleado()
        {
            try
            {
                //Validamos el correo
                if (tbxEmpleadoCorreo.Text.Length == 0 || !this.ValidarCorreo(tbxEmpleadoCorreo.Text))
                {
                    this.MostrarMensajes("El correo no es valido");
                    return false;
                }
                else
                //Validamos el usuario
                if (tbxEmpleadoNombreUsuario.Text.Length == 0 || tbxEmpleadoNombreUsuario.Text.Contains(" "))
                {
                    this.MostrarMensajes("El nombre de usuario no es valido");
                    return false;
                }
                else
                //Validamos el direccion
                if (tbxEmpleadoDireccion.Text.Length == 0)
                {
                    this.MostrarMensajes("La direccion no es valido");
                    return false;
                }
                else
                //Validamos el ciudad
                if (tbxEmpleadoCiudad.Text.Length == 0)
                {
                    this.MostrarMensajes("La ciudad no es valida");
                    return false;
                }
                else
                //Validamos el pais
                if (tbxEmpleadoPais.Text.Length == 0)
                {
                    this.MostrarMensajes("El pais no es valido");
                    return false;
                }
                else
                //Validamos el ciudad
                if (tbxEmpleadoTelefono.Text.Length == 0 || tbxEmpleadoTelefono.Text.Length < 9 || !long.TryParse(tbxEmpleadoTelefono.Text.ToString(), out long i))
                {
                    this.MostrarMensajes("El telefono no es valido");
                    return false;
                }

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            return true;
        }

        //Metodo que obtiene un array de bytes a partir de un string
        private byte[] ConvertirImagenAByte(string filename)
        {
            byte[] arrImagen = null;

            try
            {
                //Almmacenamos la imagen en el arry
                arrImagen = File.ReadAllBytes(filename);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la imagen
            return arrImagen;
        }

        //Convierte un arry de bites en un BitmapImage
        private BitmapImage ConvertirByteAImagen(byte[] arrImagen)
        {
            BitmapImage imagen = null;
            MemoryStream ms = null;

            try
            {
                using (ms = new MemoryStream(arrImagen))
                {
                    //Creamos la imagen y le cargamos el memorystream
                    imagen = new BitmapImage();
                    imagen.BeginInit();
                    imagen.StreamSource = ms;
                    imagen.CacheOption = BitmapCacheOption.OnLoad;
                    imagen.EndInit();
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la imagen
            return imagen;
        }


        //Cierra el formulario
        private void btnSalir_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //Muestra los mensajes de la aplicacion
        private void MostrarMensajes(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso de la aplicación", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

    }
}
