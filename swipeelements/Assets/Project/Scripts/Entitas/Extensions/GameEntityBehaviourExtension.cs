namespace Project.Entitas
{
    public static class GameEntityBehaviourExtension
    {
        public static bool IsInteractable(this GameEntity entity)
            => entity.isTileTag &&
               !entity.hasMove;

        public static bool IsDestroyable(this GameEntity entity)
            => entity.isTileTag &&
               !entity.hasMove;
    }
}