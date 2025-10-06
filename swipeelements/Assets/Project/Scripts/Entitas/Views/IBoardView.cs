using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public interface IBoardView
    {
        ICellView SpawnTile(CellType type, (int x, int y) coord); // логика -> вью
        void MoveTile(ICellView view, (int x, int y) coord); // телепорт (позже заменишь на анимацию)
        void DespawnTile(ICellView view);
    }
}

