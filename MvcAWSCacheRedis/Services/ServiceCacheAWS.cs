using MvcAWSCacheRedis.Helpers;
using MvcAWSCacheRedis.Models;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAWSCacheRedis.Services
{
    public class ServiceCacheAWS
    {
        private IDatabase cache;

        public ServiceCacheAWS()
        {
            this.cache = CacheRedisMultiplexer.Connection.GetDatabase();
        }

        //VAMOS A TENER UN METODO PARA ALMACENAR PRODUCTOS EN CACHE
        //DENTRO DEL CACHE, LO QUE ALMACENAREMOS SERA UNA COLECCION DE 
        //PRODUCTOS EN FORMATO JSON
        public void AddProductoCache(Producto producto)
        {
            List<Producto> productos;
            string json = this.cache.StringGet("productoscache");
            if (json == null)
            {
                productos = new List<Producto>();
            }
            else
            {
                productos =
                    JsonConvert.DeserializeObject<List<Producto>>(json);
            }
            productos.Add(producto);
            //VOLVEMOS A SERIALIZAR
            json = JsonConvert.SerializeObject(productos);
            //ALMACENAMOS LOS PRODUCTOS EN CACHE REDIS AWS
            this.cache.StringSet("productoscache", json, TimeSpan.FromMinutes(30));
        }

        public List<Producto> GetProductosCache()
        {
            string json = this.cache.StringGet("productoscache");
            if (json == null)
            {
                return null;
            }
            else
            {
                List<Producto> productos =
                    JsonConvert.DeserializeObject<List<Producto>>(json);
                return productos;
            }
        }

        public void EliminarProductoCache(int idProducto)
        {
            //RECUPERAMOS TODOS LOS PRODUCTOS
            List<Producto> productos = this.GetProductosCache();
            if (productos != null)
            {
                Producto producto =
                    productos.SingleOrDefault(x => x.IdProducto == idProducto);
                //ELIMINAMOS EL PRODUCTO DE LA COLECCION
                productos.Remove(producto);
                //SI NO HAY MAS PRODUCTOS, ELIMINAMOS LA CLAVE
                if (productos.Count == 0)
                {
                    this.cache.KeyDelete("productoscache");
                }
                else
                {
                    //SERIALIZAMOS Y ALMACENAMOS LOS DATOS SIN EL PRODUCTO ELIMINADO
                    String json = JsonConvert.SerializeObject(productos);
                    this.cache.StringSet
                        ("productoscache", json, TimeSpan.FromMinutes(30));
                }
            }
        }
    }
}
