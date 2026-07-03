using System;
using MzGames.Scripts.Data;
using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class SimulationWorld
    {
        private const float Perception = 1.0f;
        private const float SeparationWeight = 1.2f;
        private const float MinSeparation = 0.85f;
        public const float EatRadius = 0.5f;
        private const float FoodTravelSeconds = 5f;
        private const int PlacementAttempts = 48;
        private const float Epsilon = 1e-3f;

        private readonly SimulationConfig _config;
        private readonly SimulationGrid _grid;
        private readonly SpatialHash _hash;
        private readonly System.Random _rng;
        private readonly float _foodRadius;

        private Animal[] _animals;
        private Food[] _foods;
        private bool[] _foodCells;

        public SimulationWorld(SimulationConfig config, int seed)
        {
            _config = config;
            _grid = new SimulationGrid(config.GridSize);
            _hash = new SpatialHash(_grid, config.Count);
            _rng = new System.Random(seed);
            _foodRadius = FoodTravelSeconds * config.Speed;
        }

        public int Count => _config.Count;
        public Animal[] Animals => _animals;
        public Food[] Foods => _foods;

        public event Action<Vector2> FoodEaten;

        public void Build()
        {
            int m = _config.Count;
            _animals = new Animal[m];
            _foods = new Food[m];
            _foodCells = new bool[_grid.CellCount];

            var animalCells = new bool[_grid.CellCount];

            for (int i = 0; i < m; i++)
            {
                int cell = RandomFreeCell(animalCells);
                animalCells[cell] = true;
                _animals[i] = new Animal(_grid.CellCenter(cell));
            }

            for (int i = 0; i < m; i++)
            {
                int cell = RandomFoodCellNear(_animals[i].Position);
                _foodCells[cell] = true;
                _foods[i] = new Food(cell, _grid.CellCenter(cell));
            }
        }

        public void Tick(float dt)
        {
            if (dt <= 0f || _animals == null || _animals.Length == 0)
                return;

            _hash.Rebuild(_animals, _animals.Length);

            Steer(dt);
            ResolveOverlaps();
            Eat();
        }

        // Movement

        private void Steer(float dt)
        {
            int count = _animals.Length;
            for (int i = 0; i < count; i++)
            {
                Animal a = _animals[i];

                Vector2 toFood = (_foods[i].Position - a.Position).normalized;
                Vector2 heading = toFood + Separation(i, a.Position) * SeparationWeight;

                if (heading.sqrMagnitude < Epsilon * Epsilon)
                    heading = toFood;

                a.Velocity = heading.normalized * _config.Speed;
                a.Position = _grid.Clamp(a.Position + a.Velocity * dt);
            }
        }

        private Vector2 Separation(int self, Vector2 position)
        {
            Vector2 steer = Vector2.zero;

            foreach (int j in _hash.Neighbors(position))
            {
                if (j == self)
                    continue;

                Vector2 offset = position - _animals[j].Position;
                float distance = offset.magnitude;
                if (distance <= Epsilon || distance >= Perception)
                    continue;

                Vector2 away = offset / distance;
                float strength = (Perception - distance) / Perception;
                steer += away * strength;
            }

            return steer;
        }

        private void ResolveOverlaps()
        {
            int count = _animals.Length;
            for (int i = 0; i < count; i++)
            {
                Animal a = _animals[i];

                foreach (int j in _hash.Neighbors(a.Position))
                {
                    if (j <= i)
                        continue;

                    Animal b = _animals[j];
                    Vector2 delta = b.Position - a.Position;
                    float distance = delta.magnitude;
                    if (distance >= MinSeparation)
                        continue;

                    float backoff = (MinSeparation - distance) * 0.5f;
                    Vector2 apart = distance > Epsilon ? delta / distance : Vector2.right;

                    a.Position = _grid.Clamp(a.Position - apart * backoff);
                    b.Position = _grid.Clamp(b.Position + apart * backoff);
                }
            }
        }

        // Eating & respawn

        private void Eat()
        {
            int count = _animals.Length;
            for (int i = 0; i < count; i++)
            {
                Food food = _foods[i];
                Vector2 pos = _animals[i].Position;

                if ((food.Position - pos).sqrMagnitude > EatRadius * EatRadius)
                    continue;

                Vector2 eatenAt = food.Position;
                _foodCells[food.Cell] = false;

                int cell = RandomFoodCellNear(pos);
                _foodCells[cell] = true;
                food.Cell = cell;
                food.Position = _grid.CellCenter(cell);

                FoodEaten?.Invoke(eatenAt);
            }
        }

        // Cell picking

        private int RandomFreeCell(bool[] occupied)
        {
            int cellCount = _grid.CellCount;

            for (int attempt = 0; attempt < PlacementAttempts; attempt++)
            {
                int cell = _rng.Next(cellCount);
                if (!occupied[cell])
                    return cell;
            }

            for (int cell = 0; cell < cellCount; cell++)
                if (!occupied[cell])
                    return cell;

            return 0;
        }

        private int RandomFoodCellNear(Vector2 origin)
        {
            int reach = Mathf.Max(1, Mathf.CeilToInt(_foodRadius));
            int originX = Mathf.Clamp((int)origin.x, 0, _grid.Size - 1);
            int originZ = Mathf.Clamp((int)origin.y, 0, _grid.Size - 1);
            float maxDistanceSqr = _foodRadius * _foodRadius;

            for (int attempt = 0; attempt < PlacementAttempts; attempt++)
            {
                int cx = originX + _rng.Next(-reach, reach + 1);
                int cz = originZ + _rng.Next(-reach, reach + 1);
                if (!_grid.InBounds(cx, cz))
                    continue;

                int cell = _grid.CellIndex(cx, cz);
                if (_foodCells[cell])
                    continue;

                if ((_grid.CellCenter(cx, cz) - origin).sqrMagnitude > maxDistanceSqr)
                    continue;

                return cell;
            }

            return NearestFreeFoodCell(originX, originZ);
        }

        private int NearestFreeFoodCell(int originX, int originZ)
        {
            foreach (int cell in _grid.RingCells(originX, originZ))
                if (!_foodCells[cell])
                    return cell;

            return 0;
        }
    }
}
