namespace SeriLogDemo_Net6
{
    public class SeriLogDemoConfig
    {
        /// <summary>
        /// 連線相關
        /// </summary>
        public static ConnectionStrings ConnectionStrings { get; set; }

        /// <summary>
        /// 執行環境名稱
        /// </summary>
        public string EnvironmentName { get; set; }
    }
    public class ConnectionStrings
    {
        /// <summary>
        /// SQL ConnectionString
        /// </summary>
        public string DefaultConnection { get; set; }

        /// <summary>
        /// Redis Connection
        /// </summary>
        public string RedisConnection { get; set; }
    }
}
