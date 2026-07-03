using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class SpatialHash
    {
        private readonly SimulationGrid _grid;
        private readonly int[] _head;
        private int[] _next;

        public SpatialHash(SimulationGrid grid, int capacity)
        {
            _grid = grid;
            _head = new int[grid.CellCount];
            _next = new int[Mathf.Max(capacity, 1)];
        }

        public void Rebuild(Animal[] animals, int count)
        {
            if (_next.Length < count)
                _next = new int[count];

            for (int i = 0; i < _head.Length; i++)
                _head[i] = -1;

            for (int i = 0; i < count; i++)
            {
                int cell = _grid.CellOf(animals[i].Position);
                _next[i] = _head[cell];
                _head[cell] = i;
            }
        }

        public NeighborEnumerator Neighbors(Vector2 position) => new NeighborEnumerator(this, _grid, position);

        public struct NeighborEnumerator
        {
            private readonly SpatialHash _hash;
            private readonly SimulationGrid _grid;
            private readonly int _cx;
            private readonly int _cz;
            private int _dx;
            private int _dz;
            private int _node;

            public NeighborEnumerator(SpatialHash hash, SimulationGrid grid, Vector2 position)
            {
                _hash = hash;
                _grid = grid;
                grid.CellCoords(grid.CellOf(position), out _cx, out _cz);
                _dx = -2; 
                _dz = -1;
                _node = -1;
            }

            public NeighborEnumerator GetEnumerator() => this;

            public int Current => _node;

            public bool MoveNext()
            {
                if (_node >= 0)
                {
                    _node = _hash._next[_node];
                    if (_node >= 0)
                        return true;
                }

                while (NextCell())
                {
                    int nx = _cx + _dx;
                    int nz = _cz + _dz;
                    if (!_grid.InBounds(nx, nz))
                        continue;

                    _node = _hash._head[_grid.CellIndex(nx, nz)];
                    if (_node >= 0)
                        return true;
                }

                return false;
            }

            private bool NextCell()
            {
                _dx++;
                if (_dx > 1)
                {
                    _dx = -1;
                    _dz++;
                }

                return _dz <= 1;
            }
        }
    }
}
