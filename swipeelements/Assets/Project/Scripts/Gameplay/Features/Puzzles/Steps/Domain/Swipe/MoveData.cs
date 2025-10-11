using Project.Entitas;

namespace Project.Gameplay.Puzzles
{
    public readonly struct MoveData
    {
        public readonly Coord From;
        public readonly Coord To;

        public MoveData(Coord from, Coord to)
        {
            From = from;
            To = to;
        }

        public override string ToString() => $"{From.X}:{From.Y} to {To.X}:{To.Y}";
    }

    public static class MoveDataExtensions
    {
        public static MoveData Switch(this MoveData moveData) => new(moveData.To, moveData.From);
    }
}