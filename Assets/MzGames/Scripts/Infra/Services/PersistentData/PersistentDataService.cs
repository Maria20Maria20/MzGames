using MzGames.Scripts.Simulation;

namespace MzGames.Scripts.Infra.Services.PersistentData
{
    public class PersistentDataService: IPersistentDataService
    {
        public SimulationConfig Config { get; set; }
    }
}
