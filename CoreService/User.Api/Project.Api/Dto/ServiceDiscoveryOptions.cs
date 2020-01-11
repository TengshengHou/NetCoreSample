namespace Project.Api.Dto
{
    public class ServiceDisvoveryOptions
                 
    {
        public string ServiceName { get; set; }

        public ConsulOptions Consul { get; set; }
    }
}
