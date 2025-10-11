using Entitas;

namespace Project.Gameplay
{
    public interface ITileView
    {
        GameEntity Entity { get; }
        void Link(IEntity entity);
    }
}