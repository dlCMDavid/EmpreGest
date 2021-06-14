using EmpreGest.Contexto;
using EmpreGest.Modelo;
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

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Controlador correspondiente a la ventana que permite pedir un producto que se selecciono anteriormente y se recibe como parametro
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    /// <summary>
    /// Lógica de interacción para frmPedirProducto.xaml
    /// </summary>
    public partial class frmPedirProducto : Window
    {
        //Realiza las operaciones en la base de dtaos
        DAOProductos objDAOProductos = new DAOProductos();

        //Realiza la consulta en la base de datos para obtener los pedidos
        DAOProductoRecibir objDAOProductoRecibir = new DAOProductoRecibir();

        //Almacenamos el producto
        Producto objProducto = null;

        //Contructor para insertar
        public frmPedirProducto(int idProducto)
        {
            InitializeComponent();

            try
            {
                //Obtenemos el producto correspondiente a ese id
                this.objProducto = objDAOProductos.ObtenerProducto(idProducto);

                //Cargamos los datos en los componentes
                this.CargarDatosControles();
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
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

        //Cargamos los datos en los controles
        private void CargarDatosControles()
        {
            try
            {
                tbxProductoDescripcion.Text = objProducto.descripcion;
                tbxProductoUnidadesStock.Text = objProducto.unidadesStock.ToString();
                tbxProductoUnidadesPedir.Text = objProducto.unidadesPedidas.ToString();
                tbxUnidadesReservadas.Text = objProducto.unidadesReservadas.ToString();

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }


        //Metodo que pide la cantidad de productos que indica el textbox
        private void btnProductosPedir_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Validamos los campos
                if (this.ValidarUnidadesPedir())
                {
                    //Insertamos el prodcuto
                    this.PedirProducto();

                    //Mostrmaos que se añadio con exito
                    this.MostrarMensajes("Se ha pedido con exito");

                    //Limpiamos los campos
                    this.Close();

                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);

            }
        }


        //Metodo que añade un producto a la base de datos
        private void PedirProducto()
        { 
            try
            {
                //Le sumamos al apartado pedido la cantidad que desea pedir el usuario
                objProducto.unidadesPedidas =+ Convert.ToInt32(tbxProductoUnidadesDeseaPedir.Text);

                //Insertamos el campo
                objDAOProductos.RealizarPedidoProducto(objProducto);

                //Añadimos el prodcuto en al lista de recibir
                objDAOProductoRecibir.PedirProductoRecibir(objProducto.idProducto, (Decimal)objProducto.precioCompraUnitario , Convert.ToInt32(tbxProductoUnidadesDeseaPedir.Text));

                this.Close();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

        }

        //valida los campos antes de ser enviados a la base de datos
        private bool ValidarUnidadesPedir()
        {
            try
            {

                //Validamos el Primer apellido
                if (tbxProductoUnidadesDeseaPedir.Text.Length == 0 || !int.TryParse(tbxProductoUnidadesDeseaPedir.Text.ToString(), out int a))
                {
                    this.MostrarMensajes("El primer apellido no es valido");
                    return false;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
            return true;
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
