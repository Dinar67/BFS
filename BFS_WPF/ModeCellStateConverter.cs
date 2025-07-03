using System.Globalization;
using System.Windows.Data;

namespace BFS_WPF
{
    [ValueConversion(typeof(Mode), typeof(CellState))]
    public class ModeCellStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is Mode mode)
            {
                switch (mode)
                {
                    case Mode.None:
                        return CellState.None;
                    case Mode.SetPlayerBase:
                        return CellState.PlayerBase;
                    case Mode.SetEnemyGate:
                        return CellState.EnemyBase;
                    case Mode.SetObstacle:
                        return CellState.Obstacle;
                    default: return CellState.None;
                }
            }
            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if(value is CellState state)
            {
                switch (state)
                {
                    case CellState.None:
                        return Mode.None;
                    case CellState.PlayerBase:
                        return Mode.SetPlayerBase;
                    case CellState.EnemyBase:
                        return Mode.SetEnemyGate;
                    case CellState.Obstacle:
                        return Mode.SetObstacle;
                    default: return Mode.None;
                }
            }
            return null;
        }

        public CellState Convert(Mode mode)
        {
            var value = Convert(mode, typeof(CellState), null, null);
            if (value != null && value is CellState cellState) return cellState;
            return CellState.None;
        }
        public Mode ConvertBack(CellState state)
        {
            var value = ConvertBack(state, typeof(Mode), null, null);
            if (value != null && value is Mode mode) return mode;
            return Mode.None;
        }
    }
}
