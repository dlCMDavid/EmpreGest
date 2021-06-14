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
//  Controlador correspondiente a la ventana que permite tanto insertar un empleado o modificar uno si lo recibe como parametro el constructor
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    /// <summary>
    /// Lógica de interacción para frmInsertarModificarEmpleados.xaml
    /// </summary>
    public partial class frmInsertarModificarEmpleados : Window
    {
        //Realiza las operaciones en la base de dtaos
        DAOEmpleados objDAOEmpleados = new DAOEmpleados();

        //Envia los correos de la aplicación
        DAOCorreo objDAOCorreo = new DAOCorreo();

        //Almacena el empleado
        Empleado objEmpleado = null;

        //Contructor para insertar
        public frmInsertarModificarEmpleados()
        {
            InitializeComponent();

            //Cargamos el datepicker con una fecha valida ya quye para trabajar se debe tener mas de 16 años
            dtpEmpleadoFechaNacimiento.DisplayDate = DateTime.Now.AddYears(-16);

            //Ocultamos los controles correspondientes a la contraseña ya que se generará aleatoriamente
            tbxContraseña.Visibility = Visibility.Collapsed;
            btnResetearContraseña.Visibility = Visibility.Collapsed;

            try
            {
                //Cargamos el combobox
                this.CargarDatosComboBoxDepartamentos(cbxEmpleadoDepartamento);
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }   
        }

        //Contructor para modificar
        public frmInsertarModificarEmpleados(int idEmpleado)
        {
            InitializeComponent();

            //Cambiamos el nombre al boton Añadir y el textblock superior
            btnEmpleadoInsertarModificarEmpleados.Content = "Modificar empleado";
            txtEmpleadoIntroduzcaCampos.Text = "Introduza los siguiente campos para modificar el empleado: ";

            try
            {
                //Cargamos el combobox
                this.CargarDatosComboBoxDepartamentos(cbxEmpleadoDepartamento);

                //Cargamos los datos
                objEmpleado = objDAOEmpleados.ObtenerEmpleado(idEmpleado);

                //Cargamos los datos a los componentes
                this.CargarDatosControles(objEmpleado);

                //Oculatmos el boton limpiar
                btnEmpleadoLimpiarCampos.Visibility = Visibility.Collapsed;
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
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
                cbxEmpleadoDepartamento.SelectedIndex = this.ObtenerPosicionDepartamento(objEmpleado.idDepartamento);
                tbxEmpleadoPrimerApellido.Text = objEmpleado.apellido1;
                tbxEmpleadoSegundoApellido.Text = objEmpleado.apellido2;
                dtpEmpleadoFechaNacimiento.SelectedDate = objEmpleado.fechaNacimiento;
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

        //Metodo el que recibe un id deparamento y devuleve su posicion en el combobox
        private int ObtenerPosicionDepartamento(int idDepartamento)
        {
            try
            {
                //Recorremos la lista del combobox buscan el elemento que el tag corresponda con el idDepartamento
                for (int it = 0; it < cbxEmpleadoDepartamento.Items.Count; it++)
                {
                    if ((int)((ComboBoxItem)cbxEmpleadoDepartamento.Items[it]).Tag == idDepartamento)
                        return it;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Si no encuentra el area devolvemos -1
            return -1;
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Metodo que llena el combobox con los departamentos que tienen empleado
        private void CargarDatosComboBoxDepartamentos(ComboBox cbxDatos)
        {
            ComboBoxItem cbiItem = null;

            try
            {
                //Limpiamos el combobox
                cbxDatos.Items.Clear();

                //Cargamos la lista de despartamentos en el combobox
                List<Departamento> lstDepartamentos = objDAOEmpleados.ObtenerDepartamentos();

                //Los añadimos al combobox
                foreach (Departamento miDepartamento in lstDepartamentos)
                {
                    cbiItem = new ComboBoxItem();
                    cbiItem.Content = miDepartamento.descripcion;
                    cbiItem.Tag = miDepartamento.idDepartamento;
                    cbxDatos.Items.Add(cbiItem);
                }

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
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

        //Metodo que limpia los controles
        private void limpiarControles()
        {
            try
            {
                imvEmpleadoImagen.Source = (ImageSource)FindResource("INSERTARMODIFICAR_CargarImagen");
                imvEmpleadoImagen.Tag = null;
                tbxEmpleadoCorreo.Clear();
                tbxEmpleadoNombreUsuario.Clear();
                cbxEmpleadoDepartamento.SelectedIndex = -1;
                tbxEmpleadoNombre.Clear();
                tbxEmpleadoPrimerApellido.Clear();
                tbxEmpleadoSegundoApellido.Clear();
                dtpEmpleadoFechaNacimiento.SelectedDate = null;
                tbxEmpleadoDireccion.Clear();
                tbxEmpleadoCiudad.Clear();
                tbxEmpleadoPais.Clear();
                tbxEmpleadoTelefono.Clear();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Método que rellena los datos del dto
        private void CrearDTOEmpleado(Empleado objEmpleado)
        {
            try
            {
                //Comprobamos si el tag es null, si es asi significa que no se cambio la imagen
                if (imvEmpleadoImagen.Tag != null)
                    objEmpleado.foto = this.ConvertirImagenAByte(imvEmpleadoImagen.Tag.ToString());
                else
                    objEmpleado.foto = null;

                //Resto de campos
                objEmpleado.correo = tbxEmpleadoCorreo.Text.Trim();
                objEmpleado.usuario = tbxEmpleadoNombreUsuario.Text.Trim();
                objEmpleado.idDepartamento = (int)((ComboBoxItem)cbxEmpleadoDepartamento.SelectedItem).Tag;
                objEmpleado.nombre = tbxEmpleadoNombre.Text.Trim();
                objEmpleado.apellido1 = tbxEmpleadoPrimerApellido.Text.Trim();
                objEmpleado.apellido2 = tbxEmpleadoSegundoApellido.Text.Trim();
                objEmpleado.fechaNacimiento = dtpEmpleadoFechaNacimiento.SelectedDate.GetValueOrDefault();
                objEmpleado.direccion = tbxEmpleadoDireccion.Text.Trim();
                objEmpleado.ciudad = tbxEmpleadoCiudad.Text.Trim();
                objEmpleado.pais = tbxEmpleadoPais.Text.Trim();
                objEmpleado.activo = true;
                objEmpleado.telefono = Convert.ToInt32(tbxEmpleadoTelefono.Text.Trim());

                //No añadimos la contraseña, la cual añadirá el dao, de forma que podremos enviar la contraseña al usuario sin enciptar
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que controla los botones de la ventana
        private void ControlBotones_Click(object sender, RoutedEventArgs e)
        {
            Empleado miEmpleado = null;
            try
            {
                //Si se pulso insertar o modificar 
                if (btnEmpleadoInsertarModificarEmpleados == sender)
                {
                    //Validamos los campos
                    if (this.ValidarCamposEmpleado())
                    {
                        //Si el atributo objEmplado es nulo, es modificar, si no es asi, es insertar
                        if (objEmpleado == null)
                        {
                            //Insertamos el empleado y almacenamos el empleado con la contraseña sin encriptar
                            miEmpleado = this.InsertarEmpleado();

                            //Mostrmaos que se añadio con exito
                            this.MostrarMensajes("Se ha añadido con exito");

                            //Enviamos el correo
                            objDAOCorreo.CorreoInsertarUsuario(miEmpleado);

                            //Limpiamos los campos
                            this.limpiarControles();

                        }
                        else
                        {
                            this.ModificarEmpleado();

                            //Le enviamos un correo
                            objDAOCorreo.CorreoModificarDatos(objEmpleado);

                            //Mostrmaos que se añadio con exito
                            this.MostrarMensajes("Se ha modificado con exito");

                            //Cerramos el formulario
                            this.Close();
                        }

                    }
                }
                //Si se pulso resetear
                if (btnResetearContraseña == sender)
                {
                    //Reseteamos la contraseña del usuario
                    miEmpleado = objDAOEmpleados.ResetearContraseña(objEmpleado);

                    //Le enviamos un correo avisandole
                    objDAOCorreo.CorreoRecuperarContraseña(miEmpleado);

                    //Mostramos un mensaje de que se reseteo con exito
                    this.MostrarMensajes("La contraseña se ha reseteado con exito");
                }

                //Si se pulso limpiar
                if (btnEmpleadoLimpiarCampos == sender)
                {
                    this.limpiarControles();
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
                this.CrearDTOEmpleado(objEmpleado);

                //Insertamos el campo
                objDAOEmpleados.ModificarEmpleado(objEmpleado);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que añade un empleado a la base de datos
        private Empleado InsertarEmpleado()
        {
            //Creamos el empleado
            Empleado objEmpleado = null;

            try
            {
                //Creamos el empleado
                objEmpleado = new Empleado();

                //Rellenamos el dto
                this.CrearDTOEmpleado(objEmpleado);

                //Insertamos el campo
                objDAOEmpleados.InsertarEmpleado(objEmpleado);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos el empleado
            return objEmpleado;

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
                if (cbxEmpleadoDepartamento.SelectedItem == null)
                {
                    this.MostrarMensajes("No se ha seleccionado ninguna categoria");
                    return false;
                }
                else
                //Validamos el nombre
                if (tbxEmpleadoNombre.Text.Length == 0)
                {
                    this.MostrarMensajes("El nombre no es valido");
                    return false;
                }
                else
                //Validamos el Primer apellido
                if (tbxEmpleadoPrimerApellido.Text.Length == 0)
                {
                    this.MostrarMensajes("El primer apellido no es valido");
                    return false;
                }
                else
                //Validamos el Segundo apellido
                if (tbxEmpleadoSegundoApellido.Text.Length == 0)
                {
                    this.MostrarMensajes("El segundo apellido no es valido");
                    return false;
                }
                else
                //Validamos el fecha teniendo en cuenta que debe tener al menos 16 años
                if (!dtpEmpleadoFechaNacimiento.SelectedDate.HasValue || dtpEmpleadoFechaNacimiento.SelectedDate.Value.AddYears(16) > DateTime.Now)
                {
                    this.MostrarMensajes("La fecha no es valida");
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
