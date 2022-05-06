using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MvcAWSCacheRedis.Helpers
{
    public class CacheRedisMultiplexer
    {
        private static Lazy<ConnectionMultiplexer> GenerarConexion =
            new Lazy<ConnectionMultiplexer>(() =>
            {
                //AQUI LE PASAMOS EL PUNTO DE CONEXION
                //PRINCIPAL DE NUESTRO AWS CACHE
                return ConnectionMultiplexer.Connect("cache-productos.luffwz.ng.0001.use1.cache.amazonaws.com:6379");
            });

        public static ConnectionMultiplexer Connection
        {
            get { return GenerarConexion.Value; }
        }
    }
}
