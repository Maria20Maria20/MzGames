using System.Threading.Tasks;
using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factories.Interfaces;
using UnityEngine;

namespace MzGames.Scripts.Infra.Factories
{
    public class EntityFactory : IEntityFactory
    {
        private const string AnimalAddress = "Animal";
        private const string FoodAddress = "Food";
        private const string EatEffectAddress = "EatEffect";
        private const float EffectLifetime = 1f;

        private readonly int _baseColorId = Shader.PropertyToID("_BaseColor"); // URP
        private readonly int _colorId = Shader.PropertyToID("_Color");         // Standard / legacy

        private readonly IAssetProvider _assetProvider;
        private readonly MaterialPropertyBlock _mpb = new MaterialPropertyBlock();

        private GameObject _animalPrefab;
        private GameObject _foodPrefab;
        private GameObject _eatEffectPrefab; 

        public EntityFactory(IAssetProvider assetProvider) => _assetProvider = assetProvider;

        public async Task<bool> WarmUp()
        {
            _animalPrefab = await _assetProvider.Load<GameObject>(AnimalAddress);
            _foodPrefab = await _assetProvider.Load<GameObject>(FoodAddress);
            _eatEffectPrefab = await _assetProvider.Load<GameObject>(EatEffectAddress);

            return _animalPrefab != null && _foodPrefab != null && _eatEffectPrefab != null;
        }

        public GameObject CreateAnimal(Color color, Transform parent)
        {
            return Spawn(_animalPrefab, color, parent);
        }

        public GameObject CreateFood(Color color, Transform parent)
        {
            return Spawn(_foodPrefab, color, parent);
        }

        public void SpawnEatEffect(Vector3 position)
        {
            GameObject fx = Object.Instantiate(_eatEffectPrefab, position, Quaternion.identity);
            Object.Destroy(fx, EffectLifetime);
        }

        public void Cleanup()
        {
            _assetProvider.Release(AnimalAddress);
            _assetProvider.Release(FoodAddress);
            _assetProvider.Release(EatEffectAddress);

            _animalPrefab = null;
            _foodPrefab = null;
            _eatEffectPrefab = null;
        }

        private GameObject Spawn(GameObject prefab, Color color, Transform parent)
        {
            GameObject instance = Object.Instantiate(prefab, parent);
            Tint(instance, color);
            return instance;
        }

        private void Tint(GameObject go, Color color)
        {
            var renderer = go.GetComponentInChildren<MeshRenderer>();
            if (renderer == null)
                return;

            renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(_baseColorId, color); 
            _mpb.SetColor(_colorId, color);
            renderer.SetPropertyBlock(_mpb);
        }
    }
}
