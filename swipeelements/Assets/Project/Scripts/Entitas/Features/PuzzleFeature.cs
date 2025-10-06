namespace Project.Entitas
{
    public sealed class PuzzleFeature : Feature
    {
        public PuzzleFeature(Contexts c, IBoardView boardView) : base("Puzzle")
        {
            // Инициализация борда/рандома/конфига
            Add(new BoardInitialSystem(c));
            // Синхронизация данных -> вью
            Add(new BoardInitialSpawnSystem(c, boardView));
            // Add(new ViewSyncSystem(c, boardView));

            // Дальше ты добавишь:
            // Add(new InputToSwapSystem(c));
            // Add(new ApplySwapSystem(c));
            // Add(new FindMatchesSystem(c));
            // Add(new Destroy/Collapse/Refill...)
            // Add(new CleanupTransientSystem(c));
        }
    }
}