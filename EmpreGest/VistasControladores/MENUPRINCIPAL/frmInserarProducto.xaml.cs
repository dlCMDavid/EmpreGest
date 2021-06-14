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
using System.Windows.Shapes;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Controlador correspondiente a la ventana que permite Insertar un nuevo producto en la base de datos
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    public partial class frmInserarProducto : Window
    {

        //Realiza las operaciones en la base de dtaos
        DAOProductos objDAOProductos = new DAOProductos();

        //Realiza la consulta en la base de datos para obtener los pedidos
        DAOProductoRecibir objDAOProductoRecibir = new DAOProductoRecibir();

        //Contructor para insertar
        public frmInserarProducto()
        {
            InitializeComponent();

            try
            {
                //Cargamos el combobox
                this.CargarDatosComboBoxCategorias(cbxProductoCategorias);
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que llena el combobox con los categorias 
        private void CargarDatosComboBoxCategorias(ComboBox cbxDatos)
        {
            ComboBoxItem cbiItem = null;

            try
            {
                //Limpiamos el combobox
                cbxDatos.Items.Clear();

                //Cargamos la lista de despartamentos en el combobox
                List<Categoria> lstCategorias = objDAOProductos.ObtenerCategorias();

                //Los añadimos al combobox
                foreach (Categoria miCategoria in lstCategorias)
                {
                    cbiItem = new ComboBoxItem();
                    cbiItem.Content = miCategoria.descripcion;
                    cbiItem.Tag = miCategoria.idCategoria;
                    cbxDatos.Items.Add(cbiItem);
                }

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

        //Valida los numeros decimales
        private void tbxNumerosDecimales_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            TextBox tbxTexto = (TextBox)sender;
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || 
                (e.Key == Key.Back) || (e.Key == Key.OemComma && !tbxTexto.Text.Contains(",")))
            {
                 e.Handled = false;
            }
            else
                e.Handled = true;
        }

        //Metodo que limpia los controles
        private void limpiarControles()
        {
            try
            {
                tbxProductoDescripcion.Clear();
                cbxProductoCategorias.SelectedIndex = -1;
                tbxProductoPrecio.Clear();
                tbxProductoUnidadesPedir.Clear();
                tbxProductoBeneficio.Clear();
                tbxProductoStockMinimo.Clear();
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Método que rellena los datos del dto
        private void CrearDTOProducto(Producto objProducto)
        {
            try
            {
                //Resto de campos
                objProducto.descripcion = tbxProductoDescripcion.Text.Trim();
                objProducto.idCategoria = (int)((ComboBoxItem)cbxProductoCategorias.SelectedItem).Tag;
                objProducto.unidadesStock = 0;
                objProducto.stockMinimo = Convert.ToInt32(tbxProductoStockMinimo.Text.Trim());
                objProducto.unidadesPedidas = Convert.ToInt32(tbxProductoUnidadesPedir.Text.Trim());
                objProducto.beneficio = Convert.ToInt32(tbxProductoBeneficio.Text.Trim());
                objProducto.reponer = true;
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que controla los botones de la ventana
        private void ControlBotones_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Si se pulso insertar o modificar 
                if (btnProductosInsertarProductos == sender)
                {
                    //Validamos los campos
                    if (this.ValidarCamposProducto())
                    {
                        //Insertamos el prodcuto
                        this.InsertarProducto();

                        //Mostrmaos que se añadio con exito
                        this.MostrarMensajes("Se ha añadido con exito");

                        //Limpiamos los campos
                        this.limpiarControles();
                    }
                }

                //Si se pulso limpiar
                if (btnProductosLimpiarCampos == sender)
                {
                    this.limpiarControles();
                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);

            }
        }

        //Metodo que añade un producto a la base de datos
        private void InsertarProducto()
        {
            //Creamos el empleado
            Producto miProducto = null;

            try
            {
                //Creamos el empleado
                miProducto = new Producto();

                //Rellenamos el dto
                this.CrearDTOProducto(miProducto);

                //Insertamos el campo
                objDAOProductos.InsertarProducto(miProducto);

                //AÑadimos el pedido
                objDAOProductoRecibir.PedirProductoRecibir(miProducto.idProducto, Convert.ToDecimal(tbxProductoPrecio.Text.Trim()), miProducto.unidadesPedidas);
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //valida los campos antes de ser enviados a la base de datos
        private bool ValidarCamposProducto()
        {
            try
            {
                //Validamos la descripcion
                if (tbxProductoDescripcion.Text.Length == 0)
                {
                    this.MostrarMensajes("Descripción no valida");
                    return false;
                }
                else
                //Validamos la categoria
                if (cbxProductoCategorias.SelectedItem == null)
                {
                    this.MostrarMensajes("No se ha seleccionado ninguna categoria");
                    return false;
                }
                else
                //Validamos el Precio
                if (tbxProductoPrecio.Text.Length == 0 || !decimal.TryParse(tbxProductoPrecio.Text.ToString(), out decimal precProd) || precProd <= 0)
                {
                    this.MostrarMensajes("El precio no es valido");
                    return false;
                }
                else
                //Validamos el beneficio
                if (tbxProductoBeneficio.Text.Length == 0 || !int.TryParse(tbxProductoBeneficio.Text.ToString(), out int beneficio) || beneficio < 0)
                {
                    this.MostrarMensajes("El beneficio no es valido");
                    return false;
                }
                else
                //Validamos el Stock minimo
                if (tbxProductoStockMinimo.Text.Length == 0 || !int.TryParse(tbxProductoStockMinimo.Text.ToString(), out int stockMinimo) || stockMinimo < 0)
                {
                    this.MostrarMensajes("El stock mínimo no es valido");
                    return false;
                }
                else
                //Validamos el Unidades pedir
                if (tbxProductoUnidadesPedir.Text.Length == 0 || !int.TryParse(tbxProductoUnidadesPedir.Text.ToString(), out int unidPedir) || unidPedir <= 0)
                {
                    this.MostrarMensajes("La cantidad que desea pedir no es valida");
                    return false;
                }
                else
                if(unidPedir < stockMinimo)
                {
                    this.MostrarMensajes("Debe pedir una cantidad de unidades mayor que el stock mínimo");
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
