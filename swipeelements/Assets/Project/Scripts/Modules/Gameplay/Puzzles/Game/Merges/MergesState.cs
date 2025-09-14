using Newtonsoft.Json;

namespace Project.Gameplay.Puzzles
{
    [JsonObject]
    public class MergesState
    {
        [JsonProperty] private MergesCell[,] _cells;

        [JsonProperty] public int Columns { get; private set; }
        [JsonProperty] public int Rows { get; private set; }

        public MergesCell this[int x, int y]
        {
            get => _cells[x, y];
            set => _cells[x, y] = value;
        }

        public MergesCell this[(int x, int y) coord]
        {
            get => _cells[coord.x, coord.y];
            set => _cells[coord.x, coord.y] = value;
        }

        [JsonConstructor] public MergesState() { }

        public MergesState(int columns, int rows, MergesCell defaultValue = default)
        {
            Columns = columns;
            Rows = rows;
            _cells = new MergesCell[columns, rows];
            for (var y = 0; y < rows; y++)
            {
                for (var x = 0; x < columns; x++)
                {
                    _cells[x, y] = defaultValue;
                }
            }
        }

        public MergesState(ILevelData levelData, MergesCell defaultValue = default)
            : this(levelData.Columns, levelData.Rows, defaultValue)
        {
            var dict = levelData.InitialValues?.ToDictionary();
            if (dict == null)
            {
                return;
            }

            foreach (var cell in dict)
            {
                var coord = cell.Key;

                if (_cells[coord.X, coord.Y].CellType == CellType.None)
                {
                    var cellType = cell.Value.CellType;
                    var cellState = cellType.IsTile() ? CellState.Idle : CellState.None;
                    _cells[coord.X, coord.Y] = new MergesCell(cellType, cellState);
                }
            }
        }

        public MergesState Clone()
        {
            var clone = new MergesState(Columns, Rows);
            for (var y = 0; y < Rows; y++)
            {
                for (var x = 0; x < Columns; x++)
                {
                    clone[x, y] = _cells[x, y];
                }
            }
            return clone;
        }
    }
}