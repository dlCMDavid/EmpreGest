using EmpreGest.Contexto;
using EmpreGest.Entidades;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//----------------------------------------------------------------------------------------------------------------------------------------------------------------
//  David de la Cruz Morán
//  Logica de negocio que se utiliza para obtener los diversos datos de los productos, asi como insertar o hacer diversas funciones en la vista
//  correspondiente a almacén
//----------------------------------------------------------------------------------------------------------------------------------------------------------------
namespace EmpreGest.Modelo
{
    public class DAOProductos
    {
        //Obtiene la lista de categorias y la devuelve
        public List<Categoria> ObtenerCategorias()
        {
            List<Categoria> lstCategorias = new List<Categoria>(); 

            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from categoria in contexto.Categoria
                                   select categoria;

                    //Recorremos la consulta añadiendo los departamentos
                    foreach (var item in consulta)
                        lstCategorias.Add(item);
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos las categorias
            return lstCategorias;
        }

        //Metodo que obtiene una lista con los prodcutos
        public List<DTOProducto> ObtenerGestionProductos()
        {
            List<DTOProducto> lstProductos = new List<DTOProducto>();
            DTOProducto objProducto = null;
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = from producto in contexto.Producto
                                   from categoria in contexto.Categoria
                                   where (categoria.idCategoria == producto.idCategoria)
                                   select new
                                   {
                                       producto.idProducto,
                                       producto.descripcion,
                                       producto.unidadesStock,
                                       producto.unidadesPedidas,
                                       producto.unidadesReservadas,
                                       producto.stockMinimo,
                                       NombreCategoria = categoria.descripcion,
                                       producto.reponer
                                   };

                    //Recorremos la consulta añadiendo los productos
                    foreach (var item in consulta)
                    {
                        //Cremaos un nuevo producto y le añadimos los datos
                        objProducto = new DTOProducto(item.idProducto, item.NombreCategoria, item.descripcion, item.unidadesStock, item.unidadesPedidas, item.unidadesReservadas, item.stockMinimo ,item.reponer);

                        //Añadimos el producto a la lista
                        lstProductos.Add(objProducto);
                    }
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos la lista
            return lstProductos;
        }

        //Metodo que permite dejar de reponer varios productos
        public void DejarVolverReponerProductos(DTOProducto objProductoSeleccionado)
        {
            try
            {
                //Creamos el contexto y obtenemos los productos que se encuentran en la lista
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from producto in contexto.Producto
                                    where objProductoSeleccionado.idProducto == producto.idProducto
                                    select producto).FirstOrDefault();

                    //Comprobamos si el campo esta en true o en false
                    if (consulta.reponer == true)
                        consulta.reponer = false;
                    else
                        consulta.reponer = true;

                    

                    //Guardamos los cambios
                    contexto.SaveChanges();
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que inserta un prodcuto en la base de datos 
        public void InsertarProducto(Producto objProducto)
        {
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    //Añadimos el empleado y guardamos el contexto
                    contexto.Producto.Add(objProducto);

                    contexto.SaveChanges();

                    //Obtenemos el id del producto
                    var consulta = (from prod in contexto.Producto
                                   where prod.descripcion == objProducto.descripcion
                                   select prod).FirstOrDefault();

                    objProducto.idProducto = consulta.idProducto;

                }

            }
            catch (DbUpdateException)
            {
                throw new Exception("El producto ya se encuentra en la base de datos");
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que obtiene un producto a partir del id
        public Producto ObtenerProducto(int idProducto)
        {
            Producto objProducto = null;
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from producto in contexto.Producto
                                    where (idProducto == producto.idProducto)
                                    select producto).FirstOrDefault();

                    //Añadimos los datos al producto
                    objProducto = consulta;
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }

            //Devolvemos el producto
            return objProducto;
        }

        //Metodo que realiza un pedido de un producto
        public void RealizarPedidoProducto(Producto objProducto)
        {
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from producto in contexto.Producto
                                    where (objProducto.idProducto == producto.idProducto)
                                    select producto).FirstOrDefault();

                    //Modificamos el empleado
                    consulta.unidadesPedidas = objProducto.unidadesPedidas + consulta.unidadesPedidas;

                    //Guardamos los cambios
                    contexto.SaveChanges();
                }
            }
            catch (Exception error)
            {
                throw new Exception(error.Message);
            }
        }

        //Metodo que confirma la llegada de los prodcutos pedidos
        public void ConfirmarLlegadaProducto(int idProducto, int cantidad)
        {
            try
            {
                //Creamos el contexto e insertamos la fila
                using (var contexto = new DBEmpreGestEntities())
                {
                    var consulta = (from producto in contexto.Producto
                                    where producto.idProducto == idProducto
                                    select producto).FirstOrDefault();

                    //Modificamos el empleado
                    consulta.unidadesStock = cantidad + consulta.unidadesStock;
                    consulta.unidadesPedidas = consulta.unidadesPedidas - cantidad;

                    //Guardamos los cambios
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
