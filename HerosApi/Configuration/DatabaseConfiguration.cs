namespace HerosApi.Configuration
{
    public class DatabaseConfiguration : IDatabaseConfiguration
    {
        public string HerosCollectionName { get; set; }
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }

    public interface IDatabaseConfiguration
    {
        string HerosCollectionName { get; }

        string ConnectionString { get; }

        string DatabaseName { get; }
    }
}
