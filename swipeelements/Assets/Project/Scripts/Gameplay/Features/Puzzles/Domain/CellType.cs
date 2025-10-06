using System.Runtime.Serialization;

namespace Project.Gameplay.Puzzles
{
    public enum CellType : short
    {
        [EnumMember(Value = "none")]
        None = 0,
        [EnumMember(Value = "void")]
        Void = 1,
        [EnumMember(Value = "empty")]
        Empty = 2,

        [EnumMember(Value = "any_cell")]
        AnyCell = 10,
        [EnumMember(Value = "type_1")]
        Type1 = 11,
        [EnumMember(Value = "type_2")]
        Type2 = 12
    }
}