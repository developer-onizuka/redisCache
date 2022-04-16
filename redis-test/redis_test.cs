using StackExchange.Redis;
using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using Newtonsoft.Json;

namespace RedisSample
{
    public class EmployeeEntity
    {
         [BsonId]
         [BsonRepresentation(BsonType.ObjectId)]
         public string Id { get; set; } = null!;
         public int EmployeeID { get; set; }
         public string FirstName { get; set; } = null!;
         public string LastName { get; set; } = null!;
    }

    public class RedisTest
    {
        static void Main(string[] args)
        {
            IDatabase cache = Connection.GetDatabase();
	    EmployeeEntity emp = new EmployeeEntity
            {
		Id = "000000000000000000000000",
                EmployeeID = 1,
                FirstName = "Yukichi",
                LastName = "Fukuzawa"
            };

            string Jemp = JsonConvert.SerializeObject(emp);
            cache.StringSet(emp.EmployeeID.ToString(), Jemp, new TimeSpan(0,0,60));

            string json = cache.StringGet(emp.EmployeeID.ToString());
            Console.WriteLine(emp.EmployeeID + " -> " + json);
        }

        private static Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
        { 
            var ipaddr = Environment.GetEnvironmentVariable("REDIS");
            if (string.IsNullOrEmpty(ipaddr)) {
            ipaddr = "127.0.0.1:6379";
        }
            var passwd = Environment.GetEnvironmentVariable("PASSWD");
            string connectionString = ipaddr + ",password=" + passwd;
            //Console.WriteLine(connectionString);
            //string cacheConnection = ConfigurationManager.AppSettings["CacheConnection"];
            return ConnectionMultiplexer.Connect(connectionString);
        });

        public static ConnectionMultiplexer Connection
        {
            get 
            {
                return lazyConnection.Value;
            }
        }
    }
}
