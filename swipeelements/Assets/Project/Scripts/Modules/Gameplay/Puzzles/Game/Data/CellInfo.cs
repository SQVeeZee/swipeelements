namespace Project.Gameplay.Puzzles
{
    public struct CellInfo
    {
        public (int X, int Y) Coord { get; set; }
        public MergesCell Cell { get; set; }

        public CellInfo((int X, int Y) coord, MergesCell cell)
        {
            Coord = coord;
            Cell = cell;
        }
    }
}