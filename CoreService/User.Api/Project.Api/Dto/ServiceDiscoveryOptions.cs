namespace Project.Api.Dto
{
    public class ServiceDisvoveryOptions
                 
    {
        public string UserServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
