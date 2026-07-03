using UnityEngine;

namespace MzGames.Scripts.Simulation
{
    public sealed class SimulationGrid
    {
        public int Size { get; }

        public SimulationGrid(int size) => Size = size;

        public int CellCount => Size * Size;

        public int CellIndex(int cx, int cz) => cz * Size + cx;

        public void CellCoords(int cell, out int cx, out int cz)
        {
            cx = cell % Size;
            cz = cell / Size;
        }

        public bool InBounds(int cx, int cz) => cx >= 0 && cx < Size && cz >= 0 && cz < Size;

        public Vector2 CellCenter(int cx, int cz) => new Vector2(cx + 0.5f, cz + 0.5f);

        public Vector2 CellCenter(int cell)
        {
            CellCoords(cell, out int cx, out int cz);
            return CellCenter(cx, cz);
        }

        public int CellOf(Vector2 position)
        {
            int cx = Mathf.Clamp((int)position.x, 0, Size - 1);
            int cz = Mathf.Clamp((int)position.y, 0, Size - 1);
            return CellIndex(cx, cz);
        }

        public Vector2 Clamp(Vector2 position)
        {
            const float edge = 0.0001f;
            return new Vector2(
                Mathf.Clamp(position.x, 0f, Size - edge),
                Mathf.Clamp(position.y, 0f, Size - edge));
        }
        
        public RingEnumerator RingCells(int originX, int originZ) => new RingEnumerator(this, originX, originZ);

        public struct RingEnumerator
        {
            private readonly SimulationGrid _grid;
            private readonly int _originX;
            private readonly int _originZ;
            private int _radius;
            private int _cx;
            private int _cz;
            private int _current;

            public RingEnumerator(SimulationGrid grid, int originX, int originZ)
            {
                _grid = grid;
                _originX = originX;
                _originZ = originZ;
                _radius = 0;
                _cz = originZ;
                _cx = originX - 1;
                _current = -1;
            }

            public RingEnumerator GetEnumerator() => this;

            public int Current => _current;

            public bool MoveNext()
            {
                while (NextCell())
                {
                    bool onRing = Mathf.Abs(_cx - _originX) == _radius || Mathf.Abs(_cz - _originZ) == _radius;
                    if (!onRing)
                        continue;
                    if (!_grid.InBounds(_cx, _cz))
                        continue;

                    _current = _grid.CellIndex(_cx, _cz);
                    return true;
                }

                return false;
            }

            private bool NextCell()
            {
                _cx++;
                if (_cx > _originX + _radius)
                {
                    _cx = _originX - _radius;
                    _cz++;
                    if (_cz > _originZ + _radius)
                    {
                        _radius++;
                        _cz = _originZ - _radius;
                        _cx = _originX - _radius;
                    }
                }

                return _radius < _grid.Size;
            }
        }
    }
}
