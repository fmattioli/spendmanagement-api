namespace SpendManagement.Infra.CrossCutting.Conf
{
    public class SpendManagementReadModelSettings
    {
        public string Url { get; set; } = null!;
        public short MaxRetry { get; set; }
    }
}
