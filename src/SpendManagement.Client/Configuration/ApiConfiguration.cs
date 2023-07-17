namespace SpendManagement.Client.Configuration
{
    public class ApiConfiguration : IApiConfiguration
    {
        public string Endpoint { get; set; } = null!;

        public ApiVersion Version { get; }

        public ApiConfiguration(string baseUrl, ApiVersion version)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
            {
                throw new ArgumentNullException(nameof(baseUrl));
            }

            this.Endpoint = baseUrl;
            this.Version = version;
        }
    }
}
