using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public interface IBoardView
    {
        ICellView SpawnTile(CellType type, Coord coord);
        void MoveTile(MoveData moveData);
        void DespawnTile(ICellView view);
    }
}

