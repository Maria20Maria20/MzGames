using System.Threading.Tasks;
using MzGames.Scripts.Data;
using MzGames.Scripts.Infra.AssetManagement;
using MzGames.Scripts.Infra.Factories.Interfaces;
using MzGames.Scripts.Infra.Services.SaveLoad;
using MzGames.Scripts.Simulation;
using MzGames.Scripts.Simulation.Interfaces;
using MzGames.Scripts.Simulation.View;
using UnityEngine;

namespace MzGames.Scripts.Infra.Factories
{
    public class SimulationFactory : ISimulationFactory
    {
        private const string GroundAddress = "Ground";

        private readonly IEntityFactory _entityFactory;
        private readonly ISimulationClock _clock;
        private readonly IAssetProvider _assetProvider;
        private readonly ISimulationSaveService _saveService;

        private GameObject _groundPrefab;

        public SimulationFactory(IEntityFactory entityFactory, ISimulationClock clock, IAssetProvider assetProvider,
            ISimulationSaveService saveService)
        {
            _entityFactory = entityFactory;
            _clock = clock;
            _assetProvider = assetProvider;
            _saveService = saveService;
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

        public Task<SimulationController> Create(SimulationConfig config) =>
            Task.FromResult(BuildWorld(config.Validated(), snapshot: null));

        public Task<SimulationController> Restore(SimulationSnapshot snapshot)
        {
            if (snapshot == null || !snapshot.IsValid)
                return Create(snapshot?.Config ?? SimulationConfig.Default);

            return Task.FromResult(BuildWorld(snapshot.Config.Validated(), snapshot));
        }

        private SimulationController BuildWorld(SimulationConfig config, SimulationSnapshot snapshot)
        {
            var world = new SimulationWorld(config, seed: System.Environment.TickCount);
            if (snapshot != null)
                world.Load(snapshot);
            else
                world.Build();

            var rootObject = new GameObject("Simulation");
            Transform root = rootObject.transform;
            CreateGround(config.GridSize, root);

            var view = new SimulationView(world, _entityFactory, root);

            var runner = new GameObject("SimulationRunner").AddComponent<SimulationRunner>();
            runner.transform.SetParent(root);
            runner.Initialize(world, view, _clock, config.Speed, _saveService);

            FrameCamera(config.GridSize);

            return new SimulationController(rootObject, view);
        }

        private void CreateGround(int gridSize, Transform parent)
        {
            var ground = Object.Instantiate(_groundPrefab, parent);

            float half = gridSize * 0.5f;
            ground.transform.position = new Vector3(half, -0.05f, half);
            ground.transform.localScale = new Vector3(gridSize, 0.1f, gridSize);
        }

        private void FrameCamera(int gridSize)
        {
            Camera camera = Camera.main;
            camera.GetComponent<SetGameCamera>().Frame(gridSize);
        }
    }
}
