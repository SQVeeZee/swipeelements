using Newtonsoft.Json;

namespace Project.Gameplay.Puzzles
{
    public partial struct MergesCell
    {
        [JsonIgnore]
        public bool IsVoid => CellType.IsVoid();
        [JsonIgnore]
        public bool IsEmpty => CellType.IsEmpty();
        [JsonIgnore]
        public bool IsRegular => CellType.IsRegular();
        [JsonIgnore]
        public bool IsTile => CellType.IsTile();

        [JsonIgnore]
        public bool IsInteractable => CellType.IsInteractable() && CellState.IsInteractable();
        [JsonIgnore]
        public bool CanBeSwiped => CellType.CanBeSwiped() && CellState.CanBeSwiped();
        [JsonIgnore]
        public bool IsDestroyable => CellState.IsDestroyable();

        [JsonIgnore]
        public bool CanFallInto => CellType.IsEmpty() && CellState != CellState.Destroyed && CellState != CellState.Moving ||
                                   (CellType.IsTile() && CellState.IsFalling());
        [JsonIgnore]
        public bool CanFalling => CellType.CanFalling() && CellState.CanFalling();
    }
}