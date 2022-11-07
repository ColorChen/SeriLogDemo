using StackExchange.Redis;

namespace SeriLogDemo_Net6
{
    public class RedisConnectorHelper
    {
        static RedisConnectorHelper() {
            RedisConnectorHelper._connection = new Lazy<ConnectionMultiplexer>(() =>
            {
                // return ConnectionMultiplexer.Connect("localhost");
                return ConnectionMultiplexer.Connect(SeriLogDemoConfig.ConnectionStrings.RedisConnection);
            });
        }

        private static Lazy<ConnectionMultiplexer> _connection;

        public static ConnectionMultiplexer Connection
        {
            get
            {
                return _connection.Value;
            }
        }
    }
}
