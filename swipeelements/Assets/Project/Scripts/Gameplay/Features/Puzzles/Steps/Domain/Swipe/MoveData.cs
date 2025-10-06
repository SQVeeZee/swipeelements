namespace Project.Gameplay.Puzzles
{
    public readonly struct MoveData
    {
        public readonly (int X, int Y) From;
        public readonly (int X, int Y) To;

        public MoveData((int X, int Y) from, (int X, int Y) to)
        {
            From = from;
            To = to;
        }

        public override string ToString() => $"{From.X}:{From.Y} to {To.X}:{To.Y}";
    }
}