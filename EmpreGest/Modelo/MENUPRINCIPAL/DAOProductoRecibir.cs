using EmpreGest.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Logica que que interactua con la tabla ProductosRecibir para realizar diversas operaciones con dicha tabla
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Modelo
{
    public class DAOProductoRecibir
    {
        //Obtiene la lista de pedidos de un producto que va a recibir el almacen
        public List<ProductoRecibir> ObtenerProductosRecibir(int idProducto)
        {
            List<ProductoRecibir> lstProducRecibir = new List<ProductoRecibir>();

            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from prodRec in contexto.ProductoRecibir
                                   where prodRec.fechaEntregado == null && prodRec.idProducto == idProducto
                                   select prodRec;

                    //Añadimos los datos a la lista
                    foreach (var item in consulta)
                        lstProducRecibir.Add(item);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la lista de pedidos
            return lstProducRecibir;
        }

        //AÑade la fecha de entrega y confirma que ese pedido ha ssido entregado
        public void ConfirmarEntrega(int idPedido)
        {
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from prodRec in contexto.ProductoRecibir
                                   where prodRec.idPedido == idPedido
                                   select prodRec).FirstOrDefault();

                    //Modificamos los datos
                    consulta.fechaEntregado = DateTime.Now;

                    //Guardamos los cambios
                    contexto.SaveChanges();

                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Inserta un Producto en la tabla que almacena los pedidos que recibir
        public void PedirProductoRecibir(int idProducto, Decimal precioUnitario, int cantidad)
        {
            try
            {
                //Creamos el objeto
                ProductoRecibir objProductoRecibir = new ProductoRecibir();
                objProductoRecibir.idProducto = idProducto;
                objProductoRecibir.precioUnitario = precioUnitario;
                objProductoRecibir.cantidad = cantidad;
                objProductoRecibir.fechaPedido = DateTime.Now;

                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                     //Añadimos el empleado y guardamos el contexto
                     contexto.ProductoRecibir.Add(objProductoRecibir);
                     contexto.SaveChanges();
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }
    }
}
