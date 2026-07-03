using MzGames.Scripts.Data;

namespace MzGames.Scripts.Infra.Services.SaveLoad
{
    public interface ISimulationSaveService
    {
        bool HasSave { get; }
        void Save(SimulationSnapshot snapshot);
        SimulationSnapshot Load();
    }
}
