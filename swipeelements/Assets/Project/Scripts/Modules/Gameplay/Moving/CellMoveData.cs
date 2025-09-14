namespace Project.Gameplay
{
    public struct CellMoveData
    {
        public CellMoveData((int X, int Y) to, CellMoveType cellMoveType)
        {
            To = to;
            CellMoveType = cellMoveType;
        }

        public (int X, int Y) To { get; }
        public CellMoveType CellMoveType { get; }
    }
}