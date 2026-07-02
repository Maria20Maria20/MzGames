using System.Threading.Tasks;
using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Simulation;
using MzGames.Scripts.Simulation.Interfaces;
using MzGames.Scripts.Simulation.View;
using UnityEngine;

namespace MzGames.Scripts.Infra.Factories
{
    public sealed class SimulationFactory : ISimulationFactory
    {
        private const string GroundAddress = "Ground";

        private readonly IEntityFactory _entityFactory;
        private readonly ISimulationClock _clock;
        private readonly IAssetProvider _assetProvider;

        private GameObject _groundPrefab;

        public SimulationFactory(IEntityFactory entityFactory, ISimulationClock clock, IAssetProvider assetProvider)
        {
            _entityFactory = entityFactory;
            _clock = clock;
            _assetProvider = assetProvider;
        }

        public async Task<bool> WarmUp()
        {
            _groundPrefab = await _assetProvider.Load<GameObject>(GroundAddress);
            return _groundPrefab != null;
        }

        public void Cleanup()
        {
            _assetProvider.Release(GroundAddress);
            _groundPrefab = null;
        }

        public async Task<SimulationController> Create(SimulationConfig config)
        {
            config = config.Validated();

            var world = new SimulationWorld(config, seed: System.Environment.TickCount);
            world.Build();

            var rootObject = new GameObject("Simulation");
            Transform root = rootObject.transform;
            CreateGround(config.GridSize, root);

            var view = new SimulationView(world, _entityFactory, root);

            var runner = new GameObject("SimulationRunner").AddComponent<SimulationRunner>();
            runner.transform.SetParent(root);
            runner.Initialize(world, view, _clock, config.Speed);

            ConfigureCamera(config.GridSize);

            return new SimulationController(rootObject, view, _entityFactory, this);
        }

        private void CreateGround(int gridSize, Transform parent)
        {
            var ground = Object.Instantiate(_groundPrefab, parent);

            float half = gridSize * 0.5f;
            ground.transform.position = new Vector3(half, -0.05f, half);
            ground.transform.localScale = new Vector3(gridSize, 0.1f, gridSize);
        }

        private static void ConfigureCamera(int gridSize)
        {
            Camera camera = Camera.main;

            float half = gridSize * 0.5f;
            float aspect = camera.aspect > 0.0001f ? camera.aspect : 16f / 9f;
            const float margin = 1.5f;

            camera.orthographic = true;
            camera.orthographicSize = half / Mathf.Min(1f, aspect) + margin;
            camera.transform.position = new Vector3(half, gridSize + 10f, half);
            camera.transform.rotation = Quaternion.Euler(90f, 0f, 0f);
            camera.nearClipPlane = 0.3f;
            camera.farClipPlane = gridSize * 2f + 50f;
        }
    }
}
