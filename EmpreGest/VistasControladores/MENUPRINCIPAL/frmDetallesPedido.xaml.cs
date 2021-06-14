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
//  Controlador correspondiente a la ventana que muestra los detalles del pedido que se ha seleccionado
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    public partial class frmDetallesPedido : Window
    {
        //Realiza la consulta en la base de datos para obtener los pedidos
        DAOPedidos objDAOPedido = new DAOPedidos();

        public frmDetallesPedido(int idPedido)
        {
            InitializeComponent();

            //Cargamos el datagrid
            this.CargarDataGridProductos(idPedido);
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Metodo que carga el datagrid con los productos
        private void CargarDataGridProductos(int idPedido)
        {
            decimal precioTotal = 0.00m;

            try
            {
                //Obtenemos los datos y los añadimos al datagrid
                dgProductosPedidos.ItemsSource = objDAOPedido.ObtenerDetallesPedidos(idPedido, ref precioTotal);

                //Añadimos a los textbox los datos
                tbxPrecioTotal.Text = precioTotal.ToString("0.00");
                
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

        //Muestra los mensajes de la aplicacion
        private void MostrarMensajes(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso de la aplicación", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

    }
}
