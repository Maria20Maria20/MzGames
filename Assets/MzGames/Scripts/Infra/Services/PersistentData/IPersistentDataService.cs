using MzGames.Scripts.Data;

namespace MzGames.Scripts.Infra.Services.PersistentData
{
    public interface IPersistentDataService
    {
        SimulationConfig Config { get; set; }
    }
}
