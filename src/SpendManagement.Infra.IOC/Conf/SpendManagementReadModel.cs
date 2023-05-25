namespace SpendManagement.Infra.CrossCutting.Conf
{
    public class SpendManagementReadModel
    {
        public string Url { get; set; } = null!;
        public short MaxRetry { get; set; }
    }
}
