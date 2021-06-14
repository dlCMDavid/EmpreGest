using EmpreGest.Contexto;
using EmpreGest.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Logica que se utiliza en la vista correspondiente a los pedidos
//----------------------------------------------------------------------------------------------------------------------------------------------------------------

namespace EmpreGest.Modelo
{
    public class DAOPedidos
    {

        //Obtiene una lista con los clientes
        public List<DTOCliente> ObtenerClientes()
        {
            List<DTOCliente> lstClientes = new List<DTOCliente>();

            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from cliente in contexto.Cliente
                                   select new { cliente.idCliente, cliente.nombre, cliente.apellido1, cliente.apellido2 };

                    //Recorremos la consulta añadiendo los departamentos
                    foreach (var item in consulta)
                        lstClientes.Add(new DTOCliente(item.idCliente, item.nombre, item.apellido1, item.apellido2));
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos las categorias
            return lstClientes;
        }

        //Metodo que obtiene una lista con los pedidos
        public List<DTOPedido> ObtenerPedidos()
        {
            List<DTOPedido> lstPedidos = new List<DTOPedido>();
            DTOPedido objPedido = null;
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from pedido in contexto.Pedido
                                   from cliente in contexto.Cliente
                                   where (pedido.idCliente == cliente.idCliente && (from detalle in contexto.DetallePedido where detalle.idPedido == pedido.idPedido select detalle).Any())
                                   select new
                                   {
                                       pedido.idPedido,
                                       pedido.idCliente,
                                       NombreApellido = cliente.nombre + ", " + cliente.apellido1 + " " + cliente.apellido2,
                                       pedido.fechaPedido,
                                       pedido.fechaEntrega,
                                       pedido.estado,
                                       PrecioFinal = (from detalle in contexto.DetallePedido where detalle.idPedido == pedido.idPedido select detalle.precioFinal).Sum()
                                   };

                    //Recorremos la consulta añadiendo los productos
                    foreach (var item in consulta)
                    {
                            //Cremaos un nuevo producto y le añadimos los datos
                            objPedido = new DTOPedido(item.idPedido, item.idCliente, item.NombreApellido, item.fechaPedido, item.fechaEntrega.Value, item.estado, item.PrecioFinal);

                            //Añadimos el producto a la lista
                            lstPedidos.Add(objPedido);
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la lista
            return lstPedidos;
        }

        //Metodo que obtiene la lista de prodcutos de un pedido, el descuento y precio total
        public List<DTODetallesPedido> ObtenerDetallesPedidos(int idPedido, ref decimal precioTotal)
        {
            List<DTODetallesPedido> lstDetallesPedidos = new List<DTODetallesPedido>();
            DTODetallesPedido objDetallePedido = null;
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from producto in contexto.Producto
                                   from detalle in contexto.DetallePedido
                                   from categoria in contexto.Categoria
                                   where (idPedido == detalle.idPedido && producto.idProducto == detalle.idProducto && categoria.idCategoria == producto.idCategoria)
                                   select new
                                   {
                                       producto.idProducto,
                                       NombreCategoria = categoria.descripcion,
                                       producto.descripcion,
                                       detalle.precioUnitario,
                                       detalle.cantidad,
                                       precioFinal = (detalle.precioUnitario * detalle.cantidad)
                                   };

                    //Recorremos la consulta añadiendo los productos
                    foreach (var item in consulta)
                    { 
                        //Cremaos un nuevo producto y le añadimos los datos
                        objDetallePedido = new DTODetallesPedido(item.idProducto, item.descripcion, item.NombreCategoria, item.precioUnitario, item.cantidad, item.precioFinal);

                        //Añadimos el producto a la lista
                        lstDetallesPedidos.Add(objDetallePedido);

                    }

                    //Obtenemos el descuento
                    precioTotal = consulta.Select(x => x.precioFinal).Sum();
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la lista
            return lstDetallesPedidos;
        }

    }
}
