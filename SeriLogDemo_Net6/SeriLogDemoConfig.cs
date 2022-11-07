namespace SeriLogDemo_Net6
{
    public class SeriLogDemoConfig
    {
        public static ConnectionStrings ConnectionStrings { get; set; }

        public string EnvironmentName { get; set; }
    }
    public class ConnectionStrings
    {
        public string DefaultConnection { get; set; }
    }
}
