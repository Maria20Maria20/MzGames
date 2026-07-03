using System;
using System.IO;
using Newtonsoft.Json;
using static Newtonsoft.Json.JsonConvert;

namespace MzGames.Scripts.Infra.Services.SaveLoad
{
    public class SaveLoadService : ISaveLoadService
    {
        public void WriteToFile<T>(string path, T data)
        {
            try
            {
                File.WriteAllText(path, SerializeObject(data, Formatting.Indented));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[SaveLoad] Failed to write '{path}': {e.Message}");
            }
        }

        public T ReadFromFile<T>(string path) where T : class
        {
            if (!File.Exists(path))
                return null;

            try
            {
                return DeserializeObject<T>(File.ReadAllText(path));
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogError($"[SaveLoad] Failed to read '{path}': {e.Message}");
                return null;
            }
        }
    }
}
