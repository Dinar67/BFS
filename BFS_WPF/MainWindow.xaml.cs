using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Media;

namespace BFS_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region PROPERTY_CHANGED
        public event PropertyChangedEventHandler? PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion


        public int _sizeX = 10;
        public int _sizeY = 10;

        public List<Mode> modes = new List<Mode>() { Mode.None, Mode.SetPlayerBase, Mode.SetEnemyGate, Mode.SetObstacle };
        
        private Mode mode;
        public Mode Mode {
            get => mode;
            set
            {
                mode = value;
                OnPropertyChanged();
            }
        }

        public List<CellControl> Cells { get; private set; } = new List<CellControl>();

        public MainWindow()
        {
            InitializeComponent();
            Initialize();
        }


        #region INITIALIZE
        private void Initialize()
        {
            DataContext = this;
            InitilizeGrid();
            SetCells();
            ModeCb.ItemsSource = modes.ToList();
            Mode = Mode.None;
        }
        private void InitilizeGrid()
        {
            MainGrid.Columns = _sizeX;
            MainGrid.Rows = _sizeX;
        }
        private void SetCells()
        {
            ForEachCell((row, column) => {
                var cell = new CellControl(row, column);
                cell.OnClick += Cell_Click;
                Cells.Add(cell);
                MainGrid.Children.Add(cell);
            });
        }
        #endregion


        private void Cell_Click(CellControl cell)
        {
            cell.SetState(new ModeCellStateConverter().Convert(Mode));
        }


        #region HELPERS
        private void ForEachCell(Action<int, int> action)
        {
            for (int row = 0; row < _sizeY; row++)
                for (int column = 0; column < _sizeX; column++)
                    action?.Invoke(row, column);
        }
        #endregion

        private void CheckBtn_Click(object sender, RoutedEventArgs e)
        {
            ForEachCell((row, column) =>
            {
                var cell = Cells.First(x => x.Row == row && x.Column == column);
                if (cell.CellState == CellState.None) cell.CellBorder.Background = Brushes.Transparent;
            });
            var checker = new GridCheckPath(Cells, _sizeY, _sizeX);
            var result = checker.CheckPathWithVisualisation();
            MessageBox.Show($"{result}");
        }
    }
}