using MzGames.Scripts.Data;

namespace MzGames.Scripts.Infra.Services.PersistentData
{
    public class PersistentDataService: IPersistentDataService
    {
        public SimulationConfig Config { get; set; }
    }
}
