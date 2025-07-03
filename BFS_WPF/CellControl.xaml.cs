using System.Windows.Controls;
using System.Windows.Media;

namespace BFS_WPF
{
    /// <summary>
    /// Interaction logic for CellControl.xaml
    /// </summary>
    public partial class CellControl : UserControl
    {
        public delegate void CellDelegate(CellControl cell);
        public event CellDelegate OnClick;

        public int Row { get; private set; }
        public int Column { get; private set; }
        public CellState CellState { get; private set; } = CellState.None;

        public CellControl(int row, int column)
        {
            InitializeComponent();
            Row = row;
            Column = column;
            TextTb.Text = $"{Row};{Column}";
        }

        private void CellBorder_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            OnClick?.Invoke(this);
        }

        public void SetState(CellState cellState)
        {
            CellState = cellState;
            if (CellState == CellState.PlayerBase) CellBorder.Background = Brushes.GreenYellow;
            else if (CellState == CellState.EnemyBase) CellBorder.Background = Brushes.IndianRed;
            else if (CellState == CellState.Obstacle) CellBorder.Background = Brushes.Gray;
            else CellBorder.Background = Brushes.Transparent;
        }

    }
}
