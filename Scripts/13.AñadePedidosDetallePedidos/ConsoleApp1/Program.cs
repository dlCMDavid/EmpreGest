using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;


//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
// David de la Cruz Morán
// Lee los pedidos y los detalles de los pedidos de dos archivos XML, elimina detallesPedido repetidos, carga los precios de los detalle pedidos segun el precio del producto y carga los pedidos 
// y los detalles de los pedidos en la base de datos
//---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------


namespace ConsoleApp1
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            try
            {
                TratarElementos objTratar = new TratarElementos();

                //Obtiene los pedidos del archivo XML
                objTratar.obtenerXMLPedidos();

                //Obtiene los detalles del archivo XML
                objTratar.ObtenerXMLDetallesPedido();

                //Carga los pedidos en la base de datos
                objTratar.CargarPedidos();

                //Carga los detalles de los pedidos en la base de datos
                objTratar.CargarDetallesPedido();

                //Mensaje de que se completo con exito
                Console.WriteLine("Se ha completado con exito, Pulse alguna tecla para cerrar");
                Console.ReadKey();
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }
    }

    public class TratarElementos
    {
        List<Pedido> lstPedidos = new List<Pedido>();

        List<DetallePedido> lstDetallePedido = new List<DetallePedido>();


        //Lee el archivo xml correspondiente a los pedidos
        public void obtenerXMLPedidos()
        {
            try
            {
                XDocument xdoc = XDocument.Load("..\\..\\..\\ConsoleApp1\\pedido.xml");

                //Realizamos la consulta
                var consulta = xdoc.Descendants("record")
                                    .Select(x => new
                                    {
                                        idEmpleado = x.Element("idEmpleado").Value,
                                        idCliente = x.Element("idCliente").Value,
                                        fechaPedido = x.Element("fechaPedido").Value,
                                    });

                //Mostramos la consulta
                foreach (var item in consulta)
                {
                    Pedido objPedido = new Pedido();
                    objPedido.idEmpleado = Convert.ToInt32(item.idEmpleado);
                    objPedido.idCliente = Convert.ToInt32(item.idCliente);
                    objPedido.fechaPedido = Convert.ToDateTime(item.fechaPedido);
                    objPedido.fechaEntrega = objPedido.fechaPedido.AddDays(5);
                    objPedido.estado = "Entregado";

                    lstPedidos.Add(objPedido);
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Carga los peiddos en la base de datos
        public void CargarPedidos()
        {
            try
            {
                using (var contexto = new DBEmpreGestEntities())
                {
                    foreach (Pedido objPedido in lstPedidos)
                        contexto.Pedido.Add(objPedido);


                    contexto.SaveChanges();
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
            
        }

        //Lee el archivo xml correspondiente a los detalles de los pedidos
        public void ObtenerXMLDetallesPedido()
        {
            try
            {
                XDocument xdoc = XDocument.Load("..\\..\\..\\ConsoleApp1\\detallePedido.xml");

                //Realizamos la consulta
                var consulta = xdoc.Descendants("record")
                                    .Select(x => new
                                    {
                                        idPedido = x.Element("idPedido").Value,
                                        idProducto = x.Element("idProducto").Value,
                                        cantidad = x.Element("cantidad").Value
                                    });

                //Mostramos la consulta
                foreach (var item in consulta)
                {
                    DetallePedido objPedido = new DetallePedido();
                    objPedido.idPedido = Convert.ToInt32(item.idPedido);
                    objPedido.idProducto = Convert.ToInt32(item.idProducto);
                    objPedido.cantidad = Convert.ToInt32(item.cantidad);
                    objPedido.precioUnitario = 0;
                    objPedido.precioFinal = 0;

                    lstDetallePedido.Add(objPedido);

                }

                //Eliminamos los repetidos
                this.QuitarRepetidos();

                //Añadimos los precios
                this.CargarPrecios();

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Metodo que quita los repetidos
        private void QuitarRepetidos()
        {
            try
            {
                var consulta = lstDetallePedido.GroupBy(x => new { x.idPedido, x.idProducto})
                                    .Select(x => new 
                                    { 
                                        x.Key.idProducto, 
                                        x.Key.idPedido, 
                                        cantidad = x.Select(y => y.cantidad).FirstOrDefault(), 
                                        precioUnitario = x.Select(y => y.precioUnitario).FirstOrDefault(), 
                                        precioFinal = x.Select(y => y.precioFinal).FirstOrDefault()
                                    }).Distinct().ToList();

                lstDetallePedido.Clear();

                //Mostramos la consulta
                foreach (var item in consulta)
                {
                    DetallePedido objPedido = new DetallePedido();
                    objPedido.idPedido = Convert.ToInt32(item.idPedido);
                    objPedido.idProducto = Convert.ToInt32(item.idProducto);
                    objPedido.cantidad = Convert.ToInt32(item.cantidad);
                    objPedido.precioUnitario = 0;
                    objPedido.precioFinal = 0;

                    lstDetallePedido.Add(objPedido);

                }

            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Metodo que realiza una consulta la base de datos para obtener los precios de los productos y los añade a los pedidos
        private void CargarPrecios()
        {
            try
            {
                using (var contexto = new DBEmpreGestEntities())
                {
                    foreach (DetallePedido objPedido in lstDetallePedido)
                    {
                        objPedido.precioUnitario = (decimal)(from producto in contexto.Producto
                                                    where objPedido.idProducto == producto.idProducto
                                                    select producto.precioVentaUnitario).FirstOrDefault();
                        objPedido.precioFinal = Math.Round(objPedido.cantidad * objPedido.precioUnitario);

                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }

        //Metodo que carga los detalles de los pedidos
        public void CargarDetallesPedido()
        {
            try
            {
                using (var contexto = new DBEmpreGestEntities())
                {

                    foreach (DetallePedido objPedido in lstDetallePedido)
                    {
                        contexto.DetallePedido.Add(objPedido);


                        contexto.SaveChanges();
                    }
                }
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
            }
        }


    }
}
