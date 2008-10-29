namespace ConfigReader.Interfaces
{
    public interface IConfigurationBrowser
    {
        T Get<T>();
    }
}