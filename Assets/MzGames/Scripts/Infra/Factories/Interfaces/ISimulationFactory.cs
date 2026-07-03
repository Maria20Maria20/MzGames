using System.Threading.Tasks;
using MzGames.Scripts.Data;
using MzGames.Scripts.Simulation;

namespace MzGames.Scripts.Infra.Factories.Interfaces
{
    public interface ISimulationFactory
    {
        Task<bool> WarmUp();
        Task<SimulationController> Create(SimulationConfig config);
        Task<SimulationController> Restore(SimulationSnapshot snapshot);
        void Cleanup();
    }
}
