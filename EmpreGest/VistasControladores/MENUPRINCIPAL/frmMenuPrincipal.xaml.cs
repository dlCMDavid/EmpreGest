using EmpreGest.Contexto;
using EmpreGest.Entidades;
using EmpreGest.Modelo;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Controlador correspondiente a la ventana principal que dispone del menu y todos los paneles de la funcionalidad de la aplicación
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.VistasControladores
{
    /// <summary>
    /// Lógica de interacción para frmPrincipal.xaml
    /// </summary>
    public partial class frmMenuPrincipal : Window
    {
        //Objeto que realiza el intercambio de datos con las base de datos de los empleados
        DAOEmpleados objDAOEmpleados = new DAOEmpleados();

        //Objeto que realiza el intercambio de datos con las base de datos de los productos
        DAOProductos objDAOProductos = new DAOProductos();

        //Objeto que realiza el intercambio de datos para los pedidos
        DAOPedidos objDAOPedidos = new DAOPedidos();

        //Envia los correos de la aplicación
        DAOCorreo objDAOCorreo = new DAOCorreo();

        //Lista que almacena a los empleados introducidos en el datagrid
        List<DTOEmpleado> lstEmpleados = null;

        //Almacena la lista de productos
        List<DTOProducto> lstGestionProductos = null;

        //Almacena la lista de pedidos
        List<DTOPedido> lstPedidos = null;

        //Almacena el usuario que se encuentra logeado actualmente
        Empleado objEmpleadoLogueado = null;

        public frmMenuPrincipal(Empleado objEmpleadoLogueado)
        {
            InitializeComponent();

            //Aplicamos al formulario un tgamaño maximo para que no pise la barra de tareas
            this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth;

            //Almacenamos el empleado que se logueo
            this.objEmpleadoLogueado = objEmpleadoLogueado;

            //Cargamos su nombre en la barra superior
            tbxMenuSuperiorUsuario.Header = this.objEmpleadoLogueado.usuario;

            //Metodo que muestra el formulario acorde al departamento del usuario
            this.ControlAccesoEmpleados();

            //Mostramos el panel de inicio
            pInicio.Visibility = Visibility.Visible;

            //Cargamos los datos del panel de inicio
            this.CargarPanelInicio();
        }

        //Metodo que carga todos los datos a los controles del panel Inicio
        private void CargarPanelInicio()
        {
            //Creamos el timer
            DispatcherTimer reloj = null;

            try
            {
                reloj = new DispatcherTimer();
                reloj.Interval = TimeSpan.FromSeconds(1);

                //Añadimos el evento tic que se ejecutará para actualizar la hora
                reloj.Tick += reloj_Tick;

                reloj.Start();
            }
            catch(Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que actualiza la hora en el textblock
        private void reloj_Tick(object sender, EventArgs e)
        {
            tbxHoraActual.Text = DateTime.Now.ToLongTimeString();
        }

        //Controla el aceeso a la aplicación segun el departamento
        private void ControlAccesoEmpleados()
        {
            //Si el empleado logueado es de recursos humanos
            if (objEmpleadoLogueado.idDepartamento == 2)
            {
                btnProductos.Visibility = Visibility.Collapsed;
                btnPedidos.Visibility = Visibility.Collapsed;
                btnVentas.Visibility = Visibility.Collapsed;
            }

            //Si el empleado logueado es de ventas
            if (objEmpleadoLogueado.idDepartamento == 3)
            {
                btnEmpleado.Visibility = Visibility.Collapsed;
                grdBtnProductos.Visibility = Visibility.Collapsed;
            }

            //Si el empleado logueado es de ventas
            if (objEmpleadoLogueado.idDepartamento == 4)
            {
                btnEmpleado.Visibility = Visibility.Collapsed;
                btnVentas.Visibility = Visibility.Collapsed;
                btnPedidos.Visibility = Visibility.Collapsed;
                btnClientes.Visibility = Visibility.Collapsed;

            }
        }

        //Permite mover el formulario por pantalla
        private void Window_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        //Método que controla los botones de la barra superior
        private void btnBarraSuperior_Click(object sender, RoutedEventArgs e)
        {
            //Si se pulso salir
            if (btnCerrar.Equals(sender))
            {
                this.Close();
            }

            //Si se pulso minimizar
            if (btnMinimizar.Equals(sender))
            {
                this.WindowState = WindowState.Minimized;
            }

            //Si se pulso maximizar o Normal
            if (btnMaximizarNormal.Equals(sender))
            {
                //Comprobamos si la ventana esta maximizada
                if (this.WindowState == WindowState.Maximized)
                {
                    this.WindowState = WindowState.Normal;
                    imvMaximizarNormal.Source = (BitmapImage)this.FindResource("MENUPRINCIPAL_iconMaximizar");
                    
                }
                else 
                    //Comprobamos si la ventana esta normal
                    if (this.WindowState == WindowState.Normal)
                    {
                        this.WindowState = WindowState.Maximized;
                        imvMaximizarNormal.Source = (BitmapImage)this.FindResource("MENUPRINCIPAL_iconNormal");
                    }


            }
        }

        //Controla los botones de los menus correspondientes al nombre del usuario
        private void miMenuUsuario_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Si se pulso datos usuario
                if (miDatosUsuario == sender)
                {
                    //Creamos el formulario pasandole el id
                    frmDatosUsuario objfrmDatosUsuario = new frmDatosUsuario(objEmpleadoLogueado.idEmpleado);

                    //Oscurecemos el formulario principal
                    this.Opacity = 0.5;

                    //Mostramos el formulario
                    objfrmDatosUsuario.ShowDialog();

                    //hacemos que el formulario se aclare de nuevo
                    this.Opacity = 100;

                    //Actualizamos el empleado por si se modifico algun dato
                    objEmpleadoLogueado = objDAOEmpleados.ObtenerEmpleado(objEmpleadoLogueado.idEmpleado);

                    //Recargamos el datagrid
                    this.CargarDatosDatagridEmpleados();

                }

                //Si se pulso cerra sesion
                if (miCerrarSesion == sender)
                {
                    //Devolvemos que se cerro con la sesion
                    this.DialogResult = true;

                    //Cerramos el formulario
                    this.Close();
                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que oculta todos los botones
        private void OcultarBotones()
        {
            pMenuEmpleado.Visibility = Visibility.Collapsed;
            pMenuProductos.Visibility = Visibility.Collapsed;
            pMenuPedidos.Visibility = Visibility.Collapsed;
            pMenuClientes.Visibility = Visibility.Collapsed;
            pMenuVentas.Visibility = Visibility.Collapsed;

        }

        //Controla los botones del menu lateral
        private void btnMenuLateral_Click(object sender, RoutedEventArgs e)
        {
            //Si se pulso inicio
            if (btnIncio == sender)
            {
                //Ocultamos los botones visibles
                this.OcultarBotones();

                //Ocultamos los paneles
                this.OcultarPaneles();

                //Mostramos el panel de inicio
                pInicio.Visibility = Visibility.Visible;
            }

            //Si se pulso Empleado
            if (btnEmpleado == sender)
            {
                if (pMenuEmpleado.Visibility == Visibility.Visible)
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();
                }
                else
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();

                    //Mostramos el panel
                    pMenuEmpleado.Visibility = Visibility.Visible;
                }
                    
            }

            //Si se pulso Producto
            if (btnProductos == sender) 
            {
                if (pMenuProductos.Visibility == Visibility.Visible)
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();
                }
                else
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();

                    //Mostramos el panel
                    pMenuProductos.Visibility = Visibility.Visible;
                }
            }

            //Si se pulso Producto
            if (btnPedidos == sender) 
            {
                if (pMenuPedidos.Visibility == Visibility.Visible)
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();
                }
                else
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();

                    //Mostramos el panel
                    pMenuPedidos.Visibility = Visibility.Visible;
                }
            }

            //Si se pulso clientes
            if (btnClientes == sender)
            {
                if (pMenuClientes.Visibility == Visibility.Visible)
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();
                }
                else
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();

                    //Mostramos el panel
                    pMenuClientes.Visibility = Visibility.Visible;
                }
            }

            //Si se pulso ventas
            if (btnVentas == sender)
            {
                if (pMenuVentas.Visibility == Visibility.Visible)
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();
                }
                else
                {
                    //Ocultamos los botones visibles
                    this.OcultarBotones();

                    //Mostramos el panel
                    pMenuVentas.Visibility = Visibility.Visible;
                }
            }
        } 

        //Metodo que oculta los paneles del programa
        private void OcultarPaneles()
        {
            pGestionStockProductos.Visibility = Visibility.Hidden;
            pConsultaEmpleado.Visibility = Visibility.Hidden;
            pConsultaPedidos.Visibility = Visibility.Hidden;
            pGestionClientes.Visibility = Visibility.Hidden;
            pRealizarVentas.Visibility = Visibility.Hidden;
            pInicio.Visibility = Visibility.Hidden;
        }

        //Muestra los mensajes de la aplicacion
        private void MostrarMensajes(string mensaje)
        {
            MessageBox.Show(mensaje, "Aviso de la aplicación", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        //---------------------------------Panel ConsultaEmpleado ------------------------------------
        //Metodo que controla los botones laterales correspondiente a los empleados
        private void btnControlMenuEmpleados_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Comprobamos que se pulso
                if (btnEmpleadoOperaciones == sender)
                {
                    //Ocultamos todos los paneles
                    this.OcultarPaneles();

                    //Mostramos el panel
                    pConsultaEmpleado.Visibility = Visibility.Visible;

                    //Cargamos los datos al datagrid
                    this.CargarDatosDatagridEmpleados();

                    //Cargar combobox
                    this.CargarDatosComboBoxDepartamentos(cbxFiltroEmpleadoDepartamento);

                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que cargar el panel correspondiente a las operaciones
        private void CargarDatosDatagridEmpleados()
        {
            try
            {
                //Obtenemos los datos
                lstEmpleados = objDAOEmpleados.ObtenerEmpleados();

                //Comprobamos si hay elementos en la lista
                if (lstEmpleados.Count > 0)
                {
                    //Añadimos al datagrid los empleados
                    dgEmpleadosInsMod.ItemsSource = lstEmpleados;
                }
                else
                    this.MostrarMensajes("No hay Empleados que mostrar");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
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

        //Controla los filtros del combobox empleados
        private void cbxFiltroEmpleadoDepartamento_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Comprobamos que no sea null el item seleccionado
            this.FiltroEmpleados();
        }

        //Metodo que controla los filtros de los tbx empleados
        private void tbxFiltroEmpleados_TextChanged(object sender, TextChangedEventArgs e)
        {
            this.FiltroEmpleados();
        }

        //Metodo que realiza el griltro a los empleados
        private void FiltroEmpleados()
        {

            try
            {
                //Comprobamos si el combobox no es nulo, en la consulta añadimos su dato
                if (cbxFiltroEmpleadoDepartamento.SelectedValue != null)
                {
                    //Obtenemos la lista de empleados
                    dgEmpleadosInsMod.ItemsSource = lstEmpleados.Where(x => (x.pais.ToLower().StartsWith(tbxEmpleadoFiltroPais.Text.ToLower())) &&
                                                                            (x.ciudad.ToLower().StartsWith(tbxEmpleadoFiltroCiudad.Text.ToLower())) &&
                                                                            (x.nombre.ToLower().StartsWith(tbxEmpleadoFiltroNombre.Text.ToLower()) &&
                                                                            (x.nombreDepartamento.Equals(((ComboBoxItem)cbxFiltroEmpleadoDepartamento.SelectedValue).Content.ToString())))).ToList();
                }
                else
                {
                    //Obtenemos la lista de empleados
                    dgEmpleadosInsMod.ItemsSource = lstEmpleados.Where(x => (x.pais.ToLower().StartsWith(tbxEmpleadoFiltroPais.Text.ToLower())) &&
                                                                            (x.ciudad.ToLower().StartsWith(tbxEmpleadoFiltroCiudad.Text.ToLower())) &&
                                                                            (x.nombre.ToLower().StartsWith(tbxEmpleadoFiltroNombre.Text.ToLower()))).ToList();
                }              
            }
            catch(Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Controla los botones del panel empleado
        private void BtnPanelEmpleados_Click(object sender, RoutedEventArgs e)
        {
            frmInsertarModificarEmpleados objFrmInsertarModificar = null;

            try
            {
                //Si se pulso limpiar en el comboxbox
                if (btnEmpleadoLimpiarDespartamento == sender)
                {
                    //Seleccionamos el elemento -1
                    cbxFiltroEmpleadoDepartamento.SelectedIndex = -1;
                }

                //Si se pulso limpiar filtros
                if (btnEmpleadoLimpiarFiltro == sender)
                {
                    cbxFiltroEmpleadoDepartamento.SelectedIndex = -1;
                    tbxEmpleadoFiltroCiudad.Clear();
                    tbxEmpleadoFiltroPais.Clear();
                    tbxEmpleadoFiltroNombre.Clear();
                }

                //Si se pulso añadir empleado
                if (btnEmpleadoInsertarEmpleado == sender)
                {
                    //Creamos el fomulario
                    objFrmInsertarModificar = new frmInsertarModificarEmpleados();
                    
                    //Oscurecemos el formulario principal
                    this.Opacity = 0.5;

                    //Mostramos el dialogo
                    objFrmInsertarModificar.ShowDialog();

                    //Recargamos el datagrid
                    this.CargarDatosDatagridEmpleados();

                    //hacemos que el formulario se aclare de nuevo
                    this.Opacity = 100;

                }

                //Si se pulso modificar empleado
                if (btnEmpleadoModificarEmpleado == sender)
                {
                    //Comrpobamos si hay algun elemento seleccionado
                    if (dgEmpleadosInsMod.SelectedItems.Count == 1)
                    {
                        //Obtenemos el dto
                        int id = ((DTOEmpleado)dgEmpleadosInsMod.SelectedItem).idEmpleado;

                        //Oscurecemos el formulario principal
                        this.Opacity = 0.5;

                        //Creamos el fomulario enviando el dto
                        objFrmInsertarModificar = new frmInsertarModificarEmpleados(id);
                        
                        //Mostramos el dialogo
                        objFrmInsertarModificar.ShowDialog();

                        //Recargamos el datagrid
                        this.CargarDatosDatagridEmpleados();
                        
                        //hacemos que el formulario se aclare de nuevo
                        this.Opacity = 100;
                    }
                    else
                    {
                        this.MostrarMensajes("Debe seleccionar un solo elemento de la lista para modificarlo");
                    }
                }

                //Si se pulso borra empleado
                if (btnEmpleadoEliminarEmpleado == sender)
                {
                    //Comprobamos si se selecciono algun elemento
                    if (dgEmpleadosInsMod.SelectedItems.Count > 0)
                    {
                        //Almacenamos el empleado seleccionado
                        DTOEmpleado miEmpleado = (DTOEmpleado)dgEmpleadosInsMod.SelectedItem;

                        //Comprobamos si el usuario es administrador
                        if (miEmpleado.idEmpleado != 1 && objEmpleadoLogueado.idEmpleado != miEmpleado.idEmpleado)
                        {
                            //Mostramos un mensaje de aviso de borrado
                            MessageBoxResult result = MessageBox.Show("¿Seguro que desea eliminar el empleado?", "Aviso de la aplicación", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);

                            //Comprobamos la decision del usuario
                            if (MessageBoxResult.Yes == result)
                            {
                                //Borramos el empleado seleccionado
                                objDAOEmpleados.BorrarEmpleado(miEmpleado);

                                //Recargamos el datagrid
                                this.CargarDatosDatagridEmpleados();

                                //Avisamos de que se borro con exito
                                this.MostrarMensajes("Se ha despedido con exito");

                                //Enviamos el correo
                                objDAOCorreo.CorreoDepedirUsuario(miEmpleado);
                            }
                        }
                        else
                            this.MostrarMensajes("No se puede borrar al empleado seleccionado");

                    }
                    else
                        this.MostrarMensajes("Debe seleccionar un empleado de la lista");
                }

            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }


        //---------------------------------Panel Gestion de Stock Productos ------------------------------------
        //Metodo que controla los botones laterales correspondiente a los empleados
        private void btnControlMenuProductos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Comprobamos que se pulso
                if (btnGestionStockProducto == sender)
                {
                    //Ocultamos todos los paneles
                    this.OcultarPaneles();

                    //Mostramos el panel
                    pGestionStockProductos.Visibility = Visibility.Visible;

                    //Cargamos los datos al datagrid
                    this.CargarDatosDatagridProductos();

                    //Cargar combobox correspondiente al filtro de Categorias
                    this.CargarDatosComboBoxCategorias(cbxFiltroProductoCategorias);

                    //Cargamos el combobox correspondiente al filtro de reponer
                    this.CargarDatosComboBoxReponer(cbxFiltroProductoReponer);

                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que cargar el panel correspondiente a las operaciones
        private void CargarDatosDatagridProductos()
        {
            try
            {
                //Obtenemos los datos
                lstGestionProductos = objDAOProductos.ObtenerGestionProductos();

                //Comprobamos si hay elementos en la lista
                if (lstGestionProductos.Count > 0)
                {
                    //Añadimos al datagrid los productos que tengan el tick de reponer en true
                    dgProductosStock.ItemsSource = lstGestionProductos.Where(x => (x.reponer == "Sí") || (x.unidadesPedidas > 0) || (x.unidadesStock) > 0);

                }
                else
                    this.MostrarMensajes("No hay Productos que mostrar");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
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

        //Añadimos los valores Sí y no al combobox correspndiente al filtro por reponer
        private void CargarDatosComboBoxReponer(ComboBox cbxDatos)
        {
            try
            {
                //Limpiamos el combobox
                cbxDatos.Items.Clear();

                //Añadimos si y no
                cbxDatos.Items.Add("Sí");
                cbxDatos.Items.Add("No");


            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Realiza el control del combobox filtrado en productos
        private void cbxFiltroProductoCategorias_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.FiltroGestionProductos();

            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que controla el filtro del textbox
        private void tbxGestProductoIDFiltro_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.FiltroGestionProductos();
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que realiza el griltro a los empleados
        private void FiltroGestionProductos()
        {
            List<DTOProducto> lstProductosFiltrado = lstGestionProductos;

            try
            {
                //Comprobamos si el textbox no se encuentra varcio
                if (tbxGestProductoIDFiltro.Text != "")
                {
                    lstProductosFiltrado = lstProductosFiltrado.Where(x => x.idProducto.Equals(Convert.ToInt32(tbxGestProductoIDFiltro.Text))).ToList();
                }

                //Comprobamos si el combobox no es nulo y realizamos la consulta
                if (cbxFiltroProductoCategorias.SelectedValue != null)
                {
                    //Obtenemos la lista de pedidos
                    lstProductosFiltrado = lstProductosFiltrado.Where(x => x.nombreCategoria.Equals(((ComboBoxItem)cbxFiltroProductoCategorias.SelectedValue).Content)).ToList();
                }

                //Comprobamos si el combobox no es nulo y realizamos la consulta
                if (cbxFiltroProductoReponer.SelectedValue != null)
                {
                    //Obtenemos la lista de pedidos que cumplen el filtro
                    lstProductosFiltrado = lstProductosFiltrado.Where(x => x.reponer.Equals(cbxFiltroProductoReponer.SelectedValue.ToString())).ToList();
                }

                //Cargamos la lista
                dgProductosStock.ItemsSource = lstProductosFiltrado;

            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Controla los botones del panel empleado
        private void BtnPanelProductos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Si se pulso limpiar en el comboxbox categorias
                if (btnProductoLimpiarCategoria == sender)
                {
                    //Seleccionamos el elemento -1
                    cbxFiltroProductoCategorias.SelectedIndex = -1;
                }

                //Si se pulso limpiar en el comboxbox reponer
                if (btnProductoLimpiarReponer == sender)
                {
                    //Seleccionamos el elemento -1
                    cbxFiltroProductoReponer.SelectedIndex = -1;
                }

                //Si se pulso insertar Producto
                if (btnProductoInsertarProducto == sender)
                {
                    //Creamos el fomulario
                    frmInserarProducto objFrmInsertarProducto= new frmInserarProducto();

                    //Oscurecemos el formulario principal
                    this.Opacity = 0.5;

                    //Mostramos el dialogo
                    objFrmInsertarProducto.ShowDialog();

                    //Recargamos el datagrid
                    this.CargarDatosDatagridProductos();

                    //hacemos que el formulario se aclare de nuevo
                    this.Opacity = 100;

                }

                //Si se pulso Pedir producto
                if (btnProductoPedirProducto == sender)
                {
                    //Comrpobamos si hay algun elemento seleccionado
                    if (dgProductosStock.SelectedItems.Count == 1)
                    {
                        //Creamos el fomulario
                        frmPedirProducto objFrmPedirProducto = new frmPedirProducto(((DTOProducto)dgProductosStock.SelectedItem).idProducto);

                        //Oscurecemos el formulario principal
                        this.Opacity = 0.5;

                        //Mostramos el formulario
                        objFrmPedirProducto.ShowDialog();

                        //Recargamos el datagrid
                        this.CargarDatosDatagridProductos();

                        //Realizamos el filtro
                        this.cbxFiltroProductoCategorias_SelectionChanged(null, null);

                        //hacemos que el formulario se aclare de nuevo
                        this.Opacity = 100;
                    }
                    else
                    {
                        this.MostrarMensajes("Debe seleccionar un producto para pedirlo");
                    }
                }

                //Si se pulso modificar empleado
                if (btnProductoConfirmarLlegada == sender)
                {
                    //Comrpobamos si hay algun elemento seleccionado
                    if (dgProductosStock.SelectedItems.Count == 1)
                    {
                        //Comprobamos que el producto tenga unidades pedidas
                        if (((DTOProducto)dgProductosStock.SelectedItem).unidadesPedidas > 0)
                        {

                            //Oscurecemos el formulario principal
                            this.Opacity = 0.5;

                            //Creamos el fomulario enviando el dto
                            frmConfirmarLlegadaProducto objfrmConfirmarLlegada = new frmConfirmarLlegadaProducto(((DTOProducto)dgProductosStock.SelectedItem).idProducto);

                            //Mostramos el dialogo
                            objfrmConfirmarLlegada.ShowDialog();

                            //Recargamos el datagrid
                            this.CargarDatosDatagridProductos();

                            //hacemos que el formulario se aclare de nuevo
                            this.Opacity = 100;

                            //Realizamos el filtro
                            this.cbxFiltroProductoCategorias_SelectionChanged(null, null);
                        }
                        else
                        {
                            this.MostrarMensajes("El producto seleccionado no tiene unidades pedidas");
                        }
                    }
                    else
                    {
                        this.MostrarMensajes("Debe seleccionar un solo elemento para confirmar su llegada");
                    }
                }

                //Si se pulso borra empleado
                if (btnProductoDejarReponerProducto == sender)
                {
                    //Comprobamos si se selecciono algun elemento
                    if (dgProductosStock.SelectedItems.Count > 0)
                    {
                        //Almacenamos la lista de productos seleccionado
                        DTOProducto objProductoSeleccionado = (DTOProducto)dgProductosStock.SelectedItem;

                        //Enviamos la lista de productos que vamos a dejar de reponer
                        objDAOProductos.DejarVolverReponerProductos(objProductoSeleccionado);

                        //Recargamos el datagrid
                        this.CargarDatosDatagridProductos();

                        //Avisamos de que se dejo con exito
                        this.MostrarMensajes("Se ha dejado o vuelto a reponer con exito");
                    }
                    else
                        this.MostrarMensajes("Debe seleccionar al menos un producto de la lista");
                }

            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }


        //---------------------------------Panel Consulta Pedidos ------------------------------------

        //Metodo que controla los botones laterales correspondiente a los pedidos
        private void btnControlMenuPedidos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Comprobamos que se pulso
                if (btnConsultarPedidos == sender)
                {
                    //Ocultamos todos los paneles
                    this.OcultarPaneles();

                    //Mostramos el panel
                    pConsultaPedidos.Visibility = Visibility.Visible;

                    //Cargamos los datos al datagrid
                    this.CargarDatosDatagridPedidos();

                    //Cargar combobox correspondiente a los pedidos
                    this.CargarDatosComboBoxClientesPedidos(cbxFiltroPedidosClientesPedidos);

                    //Cargar combobox
                    this.CargarDatosComboBoxEstadoPedido(cbxFiltroPedidosEstado);

                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Carga los datos del datagrid pedidos
        private void CargarDatosDatagridPedidos()
        {
            try
            {
                //Obtenemos los datos
                lstPedidos = objDAOPedidos.ObtenerPedidos();

                //Comprobamos si hay elementos en la lista
                if (lstPedidos.Count > 0)
                {
                    //Añadimos al datagrid los empleados
                    dgPedidos.ItemsSource = lstPedidos;
                }
                else
                    this.MostrarMensajes("No hay pedidos que mostrar");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Controla los botones del panel Consultar pedidos
        private void BtnPanelPedidos_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Si se pulso limpiar en el comboxbox
                if (btnPedidosLimpiarCliente == sender)
                {
                    //Seleccionamos el elemento -1
                    cbxFiltroPedidosClientesPedidos.SelectedIndex = -1;
                }

                //Si se pulso limpiar en el comboxbox
                if (btnPedidosLimpiarEstado == sender)
                {
                    //Seleccionamos el elemento -1
                    cbxFiltroPedidosEstado.SelectedIndex = -1;
                }

                //Si se pulso limpiar filtros
                if (btnPedidosLimpiarFiltro == sender)
                {
                    cbxFiltroPedidosClientesPedidos.SelectedIndex = -1;
                    cbxFiltroPedidosEstado.SelectedIndex = -1;
                    tbxPedidoNumeroPedidoFiltro.Clear();
                }

                //Si se pulso detalles de pedido
                if(btnPedidosDetalles == sender)
                {
                    if (dgPedidos.SelectedItems.Count > 0)
                    {
                        //Creamos el fomulario
                        frmDetallesPedido objfrmDetallePedido = new frmDetallesPedido(((DTOPedido)dgPedidos.SelectedItem).idPedido);

                        //Oscurecemos el formulario principal
                        this.Opacity = 0.5;

                        //Mostramos el dialogo
                        objfrmDetallePedido.ShowDialog();

                        //hacemos que el formulario se aclare de nuevo
                        this.Opacity = 100;
                    }
                    else
                        this.MostrarMensajes("Debe seleccionar al menos un pedido de la lista para mostrar sus detalles");

                }

            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que llena el combobox con los categorias 
        private void CargarDatosComboBoxClientesPedidos(ComboBox cbxDatos)
        {
            ComboBoxItem cbiItem = null;

            try
            {
                //Limpiamos el combobox
                cbxDatos.Items.Clear();

                //Cargamos la lista de despartamentos en el combobox
                List<DTOCliente> lstClientes = objDAOPedidos.ObtenerClientes();

                //Los añadimos al combobox
                foreach (DTOCliente miCliente in lstClientes)
                {
                    cbiItem = new ComboBoxItem();
                    cbiItem.Content = miCliente.nombreApellidos;
                    cbiItem.Tag = miCliente.idCliente;
                    cbxDatos.Items.Add(cbiItem);
                }

            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Añadimos los estados de los pedidos para filtrar
        private void CargarDatosComboBoxEstadoPedido(ComboBox cbxDatos)
        {
            try
            {
                //Limpiamos el combobox
                cbxDatos.Items.Clear();

                //Añadimos los estados de los pedidos
                cbxDatos.Items.Add("Preparando");
                cbxDatos.Items.Add("Enviado");
                cbxDatos.Items.Add("Entregado");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que solo permite que se introduzcan numeros enteros
        private void tbxNumerosEnteros_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if ((e.Key >= Key.D0 && e.Key <= Key.D9) || (e.Key >= Key.NumPad0 && e.Key <= Key.NumPad9) || (e.Key == Key.Back))
            {
                e.Handled = false;
            }
            else
                e.Handled = true;
        }

        //Metodo que controla el filtro del textbox
        private void tbxFiltroIDPedidos_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                this.FiltroPedidos();
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Controla los combobox para filtrar
        private void cbxFiltroPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                this.FiltroPedidos();
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

        //Metodo que realiza el griltro a los empleados
        private void FiltroPedidos()
        {
            List<DTOPedido> lstPedidosFiltrado = lstPedidos;

            try
            {
                //Comprobamos si el textbox no se encuentra varcio
                if (tbxPedidoNumeroPedidoFiltro.Text != "")
                {
                    lstPedidosFiltrado = lstPedidosFiltrado.Where(x => x.idPedido.Equals(Convert.ToInt32(tbxPedidoNumeroPedidoFiltro.Text))).ToList();
                }

                //Comprobamos si el combobox no es nulo y realizamos la consulta
                if (cbxFiltroPedidosClientesPedidos.SelectedValue != null)
                {
                    //Obtenemos la lista de pedidos
                    lstPedidosFiltrado = lstPedidosFiltrado.Where(x => x.idCliente.Equals(((ComboBoxItem)cbxFiltroPedidosClientesPedidos.SelectedValue).Tag)).ToList();
                }

                //Comprobamos si el combobox no es nulo y realizamos la consulta
                if (cbxFiltroPedidosEstado.SelectedValue != null)
                {
                    //Obtenemos la lista de pedidos que cumplen el filtro
                    lstPedidosFiltrado = lstPedidosFiltrado.Where(x => x.estado.Equals(cbxFiltroPedidosEstado.SelectedValue.ToString())).ToList();
                }

                //Cargamos la lista
                dgPedidos.ItemsSource = lstPedidosFiltrado;

            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }


        //---------------------------------Panel Realizar Ventas ------------------------------------

        //Metodo que controla los botones laterales correspondiente a los pedidos
        private void btnControlMenuVentas_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Comprobamos que se pulso
                if (btnRealizarVentas == sender)
                {
                    //Ocultamos todos los paneles
                    this.OcultarPaneles();

                    //Mostramos el panel
                    pRealizarVentas.Visibility = Visibility.Visible;

                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }


        //---------------------------------Panel Gestion Clientes ------------------------------------
        private void btnControlMenuClientes_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Comprobamos que se pulso
                if (btnGestionClientes == sender)
                {
                    //Ocultamos todos los paneles
                    this.OcultarPaneles();

                    //Mostramos el panel
                    pGestionClientes.Visibility = Visibility.Visible;

                }
            }
            catch (Exception error)
            {
                this.MostrarMensajes(error.Message);
            }
        }

    }
}
