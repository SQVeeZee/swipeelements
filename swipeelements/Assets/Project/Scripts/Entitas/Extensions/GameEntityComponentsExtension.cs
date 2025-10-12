using Project.Gameplay.Puzzles;

namespace Project.Entitas
{
    public static class GameEntityComponentsExtension
    {
        public static void AddMoveComponent(this GameEntity gameEntity, GameContext gameContext, MoveData moveData)
        {
            var moveSettings = gameContext.moveConfig.MoveSettings;
            var fromPosition = gameContext.GetEntityWithCellCoord(moveData.From).cellPosition.value;
            var toPosition = gameContext.GetEntityWithCellCoord(moveData.To).cellPosition.value;
            gameEntity.RemoveTileCoord();
            gameEntity.AddMove(moveData, fromPosition, toPosition, 0f, moveSettings.Duration, moveSettings.Curve);
        }

        public static void RemoveMoveComponent(this GameEntity gameEntity, MoveData moveData)
        {
            gameEntity.RemoveMove();
            gameEntity.AddMoveFinished(moveData);
        }

        public static void AddFallingComponent(this GameEntity gameEntity, GameContext gameContext, MoveData moveData)
        {
            var fromPosition = gameContext.GetEntityWithCellCoord(moveData.From).cellPosition.value;
            var toPosition = gameContext.GetEntityWithCellCoord(moveData.To).cellPosition.value;
            gameEntity.RemoveTileCoord();
            gameEntity.AddFalling(moveData, fromPosition, toPosition, 0, 0);
        }

        public static void ReplaceFallingComponent(this GameEntity gameEntity, GameContext gameContext, MoveData moveData)
        {
            var moveSettings = gameContext.fallingConfigEntity.falling;
            var fromPosition = gameContext.GetEntityWithCellCoord(moveData.From).cellPosition.value;
            var toPosition = gameContext.GetEntityWithCellCoord(moveData.To).cellPosition.value;
            gameEntity.RemoveTileCoord();
            gameEntity.ReplaceFalling(moveData, fromPosition, toPosition, moveSettings.speed, moveSettings.elapsed);
        }

        public static void RemoveFallingComponent(this GameEntity gameEntity, MoveData moveData)
        {
            gameEntity.AddTileCoord(moveData.To);
            gameEntity.RemoveFalling();
            gameEntity.AddFallingFinished(moveData);
        }
    }
}