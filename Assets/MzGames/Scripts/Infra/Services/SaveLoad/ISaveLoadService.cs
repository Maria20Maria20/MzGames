namespace MzGames.Scripts.Infra.Services.SaveLoad
{
    public interface ISaveLoadService
    {
        void WriteToFile<T>(string path, T data);
        T ReadFromFile<T>(string path) where T : class;
    }
}
