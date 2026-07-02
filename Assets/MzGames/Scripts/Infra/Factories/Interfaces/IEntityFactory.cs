using System.Threading.Tasks;
using UnityEngine;

namespace MzGames.Scripts.Infra.Factories.Interfaces
{
    public interface IEntityFactory
    {
        Task<bool> WarmUp();

        GameObject CreateAnimal(Color color, Transform parent);
        GameObject CreateFood(Color color, Transform parent);
        void SpawnEatEffect(Vector3 position);
        void Cleanup();
    }
}
