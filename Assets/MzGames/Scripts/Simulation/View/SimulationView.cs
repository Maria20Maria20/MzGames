using MzGames.Scripts.Infra.Factories.Interfaces;
using UnityEngine;

namespace MzGames.Scripts.Simulation.View
{
    public class SimulationView : System.IDisposable
    {
        private const float EntityY = 0.5f; 

        private readonly SimulationWorld _world;
        private readonly IEntityFactory _entityFactory;
        private readonly Transform _root;

        private Transform[] _animalTransforms;
        private Transform[] _foodTransforms;

        public SimulationView(SimulationWorld world, IEntityFactory entityFactory, Transform root)
        {
            _world = world;
            _entityFactory = entityFactory;
            _root = root;

            CreateEntities();
            _world.FoodEaten += OnFoodEaten;
        }

        private void CreateEntities()
        {
            int count = _world.Count;
            _animalTransforms = new Transform[count];
            _foodTransforms = new Transform[count];

            for (int i = 0; i < count; i++)
            {
                Color color = EntityPalette.ColorFor(i);
                _animalTransforms[i] = _entityFactory.CreateAnimal(color, _root).transform;
                _foodTransforms[i] = _entityFactory.CreateFood(color, _root).transform;
            }

            SyncTransforms();
        }

        public void SyncTransforms()
        {
            Animal[] animals = _world.Animals;
            Food[] foods = _world.Foods;

            for (int i = 0; i < _animalTransforms.Length; i++)
            {
                _animalTransforms[i].position = ToWorld(animals[i].Position);
                _foodTransforms[i].position = ToWorld(foods[i].Position);
            }
        }

        private void OnFoodEaten(Vector2 position) =>
            _entityFactory.SpawnEatEffect(ToWorld(position));

        private Vector3 ToWorld(Vector2 p) => new Vector3(p.x, EntityY, p.y);

        public void Dispose() => _world.FoodEaten -= OnFoodEaten;
    }
}
