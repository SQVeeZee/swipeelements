using System.Threading;
using Cysharp.Threading.Tasks;
using Project.Entitas;
using Project.Gameplay.Puzzles;

namespace Project.Gameplay
{
    public abstract class ItemContainer<TCellObject>
        where TCellObject : CellObject
    {
    }
}