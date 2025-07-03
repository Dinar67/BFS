using System.Windows.Media;

namespace BFS_WPF
{
    public class GridCheckPath
    {
        private Queue<CellControl> _queue;
        private List<CellControl> _cells;
        private int _rows, _columns;
        private Dictionary<CellControl, CellControl> _parents;
        private CellControl _lastCell;

        public GridCheckPath(List<CellControl> cells, int rows, int columns)
        {
            _cells = cells;
            _rows = rows;
            _columns = columns;
        }

        //Check if has path from EnemyBase to PlayerBase usses BFS (Breadth-First-Search)
        public bool CheckPath()
        {
            _queue = new Queue<CellControl>();

            if (!CheckEnemyGate(out CellControl enemyGate)) return false;
            if (!CheckPlayerBase(out CellControl playerBase)) return false;

            _queue.Enqueue(enemyGate);
            var checkedCells = new bool[_rows, _columns];
            while (_queue.Count > 0)
            {
                var cell = _queue.Dequeue();
                if (cell.CellState == CellState.PlayerBase) return true;
                checkedCells[cell.Row, cell.Column] = true;

                CheckNeighbour(checkedCells, cell.Row + 1, cell.Column);
                CheckNeighbour(checkedCells, cell.Row - 1, cell.Column);
                CheckNeighbour(checkedCells, cell.Row, cell.Column + 1);
                CheckNeighbour(checkedCells, cell.Row, cell.Column - 1);
            }
            return false;
        }
        public bool CheckPathWithVisualisation()
        {
            _queue = new Queue<CellControl>();
            _parents = new Dictionary<CellControl, CellControl>();

            if (!CheckEnemyGate(out CellControl enemyGate)) return false;
            if (!CheckPlayerBase(out CellControl playerBase)) return false;

            _queue.Enqueue(enemyGate);
            var checkedCells = new bool[_rows, _columns];
            while (_queue.Count > 0)
            {
                var cell = _queue.Dequeue();
                if (cell.CellState == CellState.PlayerBase)
                {
                    VisualisePath(cell);
                    return true;
                }
                checkedCells[cell.Row, cell.Column] = true;
                _lastCell = cell;
                CheckNeighbourWithVisualisation(checkedCells, cell.Row + 1, cell.Column);
                CheckNeighbourWithVisualisation(checkedCells, cell.Row - 1, cell.Column);
                CheckNeighbourWithVisualisation(checkedCells, cell.Row, cell.Column + 1);
                CheckNeighbourWithVisualisation(checkedCells, cell.Row, cell.Column - 1);
            }
            return false;
        }
        private void VisualisePath(CellControl cell)
        {
            var cellParent = _parents.FirstOrDefault(x =>
                x.Key.Row == cell.Row &&
                x.Key.Column == cell.Column &&
                x.Value.CellState != CellState.PlayerBase).Value;
            while (cellParent != null)
            {
                cellParent.CellBorder.Background = Brushes.Yellow;
                cellParent = _parents.FirstOrDefault(x =>
                    x.Key.Row == cellParent.Row &&
                    x.Key.Column == cellParent.Column &&
                    x.Value.CellState != CellState.PlayerBase &&
                    x.Value.CellState != CellState.EnemyBase).Value;
            }
        }

        private bool CheckEnemyGate(out CellControl enemyGate)
        {
            enemyGate = _cells.FirstOrDefault(x => x.CellState == CellState.EnemyBase);
            if (enemyGate != null) return true;
            else return false;
        }
        private bool CheckPlayerBase(out CellControl playerBase)
        {
            playerBase = _cells.FirstOrDefault(x => x.CellState == CellState.PlayerBase);
            if (playerBase != null) return true;
            else return false;
        }
        private void CheckNeighbour(bool[,] checkedCells, int row, int column)
        {
            if (row < 0 || row >= _rows || column < 0 || column >= _columns) return;
            if (checkedCells[row, column] || _queue.Any(x => x.Row == row && x.Column == column)) return;
            if (_cells.FirstOrDefault(x => x.Row == row && x.Column == column
                && (x.CellState == CellState.None
                    || x.CellState == CellState.EnemyBase
                    || x.CellState == CellState.PlayerBase)) != null)
                _queue.Enqueue(_cells.First(x => x.Row == row && x.Column == column));
        }
        private void CheckNeighbourWithVisualisation(bool[,] checkedCells, int row, int column)
        {
            if (row < 0 || row >= _rows || column < 0 || column >= _columns) return;
            if (checkedCells[row, column] || _queue.Any(x => x.Row == row && x.Column == column)) return;
            if (_cells.FirstOrDefault(x => x.Row == row && x.Column == column
                && (x.CellState == CellState.None
                    || x.CellState == CellState.EnemyBase
                    || x.CellState == CellState.PlayerBase)) != null)
            {
                var cell = _cells.First(x => x.Row == row && x.Column == column);
                _queue.Enqueue(cell);
                if (_lastCell != null &&
                    !_parents.Any(x =>
                        x.Key.Row == cell.Row &&
                        x.Key.Column == cell.Column)) _parents.Add(cell, _lastCell);
            }
        }
    }
}
