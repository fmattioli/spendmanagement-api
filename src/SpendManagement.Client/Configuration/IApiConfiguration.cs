namespace SpendManagement.Client.Configuration
{
    public interface IApiConfiguration
    {
        string Endpoint { get; }

        ApiVersion Version { get; }
    }
}
