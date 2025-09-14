using System;
using Project.Gameplay.Puzzles;
using UnityEngine;

namespace Project.Gameplay
{
    public static class StateLogger
    {
        public static void LogState(this MergesState state, string title = "MergesState")
        {
            var output = $"{title} ({state.Columns}x{state.Rows}):\n";

            for (var y = state.Rows - 1; y >= 0; y--)
            {
                for (var x = 0; x < state.Columns; x++)
                {
                    var cell = state[x, y];
                    var text = $"[{cell.CellType.ToText()} : {cell.CellState.ToText()}]";

                    var color = GetColor(cell);
                    output += $"<color={color}>{text}</color> ";
                }

                output += "\n";
            }

            Debug.Log(output);
        }

        private static string ToText(this CellType cellType) =>
            cellType switch
            {
                CellType.Empty => "Empty",
                CellType.Type1 => "Type1",
                CellType.Type2 => "Type2",
                _ => throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null)
            };

        private static string ToText(this CellState cellType) =>
            cellType switch
            {
                CellState.None => "None",
                CellState.Idle => "Idle",
                CellState.Moving => "Move",
                CellState.Falling => "Fall",
                CellState.Destroyed => "Kill",
                _ => throw new ArgumentOutOfRangeException(nameof(cellType), cellType, null)
            };

        private static string GetColor(MergesCell cell)
        {
            return cell.CellState switch
            {
                CellState.Moving => "red",
                CellState.Falling => "yellow",
                CellState.Destroyed => "black",
                CellState.Idle => "green",
                _ => "white"
            };
        }
    }
}