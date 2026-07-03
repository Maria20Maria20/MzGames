using System.IO;
using MzGames.Scripts.Data;
using UnityEngine;

namespace MzGames.Scripts.Infra.Services.SaveLoad
{
    public class SimulationSaveService : ISimulationSaveService
    {
        private const string FileName = "simulation.json";

        private readonly ISaveLoadService _saveLoadService;

        public SimulationSaveService(ISaveLoadService saveLoadService) => _saveLoadService = saveLoadService;

        private string FilePath => Path.Combine(Application.persistentDataPath, FileName);

        public bool HasSave => File.Exists(FilePath);

        public void Save(SimulationSnapshot snapshot) => _saveLoadService.WriteToFile(FilePath, snapshot);

        public SimulationSnapshot Load() => _saveLoadService.ReadFromFile<SimulationSnapshot>(FilePath);
    }
}
