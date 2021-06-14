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
//  Controlador correspondiente al a ventana que permite confirmar la llegada de los productos
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    public partial class frmConfirmarLlegadaProducto : Window
    {
        //Realiza la consulta en la base de datos para obtener los pedidos
        DAOProductoRecibir objDAOProductoRecibir = new DAOProductoRecibir();

        //Objeto que realiza el intercambio de datos con las base de datos de los productos
        DAOProductos objDAOProductos = new DAOProductos();

        public frmConfirmarLlegadaProducto(int idProducto)
        {
            InitializeComponent();
            
            //Cargamos el datagrid
            this.CargarDataGridProductosConfirmar(idProducto);
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Metodo que carga el datagrid con los pedidos
        private void CargarDataGridProductosConfirmar(int idPedido)
        {
            try
            {
                //Obtenemos los datos y los añadimos al datagrid
                dgProductosConfirmar.ItemsSource = objDAOProductoRecibir.ObtenerProductosRecibir(idPedido);

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


        //Confirma la llegade del producto seleccionada
        private void btnConfirmarLlegada_Click(object sender, RoutedEventArgs e)
        {
            if (dgProductosConfirmar.SelectedItems.Count == 1)
            {
                int idProducto = ((ProductoRecibir)dgProductosConfirmar.SelectedItem).idProducto;
                int cantidad = ((ProductoRecibir)dgProductosConfirmar.SelectedItem).cantidad;

                //Confirmamos la llegada
                objDAOProductoRecibir.ConfirmarEntrega(((ProductoRecibir)dgProductosConfirmar.SelectedItem).idPedido);

                //Actualizamos la tabla productos
                objDAOProductos.ConfirmarLlegadaProducto(idProducto, cantidad);


                this.Close();
            }
        }
    }
}
