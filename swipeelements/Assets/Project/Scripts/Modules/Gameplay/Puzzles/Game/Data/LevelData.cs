using System;

namespace Project.Gameplay.Puzzles
{
    [Serializable]
    public class LevelData : ILevelData
    {
        public int Columns { get; set; }
        public int Rows { get; set; }
        public InitialGridData InitialValues { get; set; }

        public override string ToString()
        {
            var sb = new System.Text.StringBuilder();

            sb.AppendLine("=== Level Info ===");
            sb.AppendLine($"Columns: {Columns}");
            sb.AppendLine($"Rows: {Rows}");

            if (InitialValues?.Cells != null)
            {
                sb.AppendLine("Grid:");

                for (var y = 0; y < Rows; y++)
                {
                    for (var x = 0; x < Columns; x++)
                    {
                        var cell = InitialValues.Cells[x][y];
                        sb.Append($"{cell.CellType.ToString(),-7} ");
                    }
                    sb.AppendLine();
                }
            }
            else
            {
                sb.AppendLine("No grid data");
            }

            return sb.ToString();
        }
    }
}