using UnityEngine;

namespace Game.Features.Tetris
{
    public static class TetrisBoardLogic
    {
        public static bool CanPlace(int[,] board, int width, int totalHeight, Vector2Int[] cells, Vector2Int position)
        {
            foreach (var cell in cells)
            {
                var x = position.x + cell.x;
                var y = position.y + cell.y;
                if (x < 0 || x >= width)
                {
                    return false;
                }
                if (y < 0)
                {
                    return false;
                }
                if (y >= totalHeight)
                {
                    continue;
                }
                if (board[x, y] != 0)
                {
                    return false;
                }
            }
            return true;
        }

        public static Vector2Int CalculateDropPosition(int[,] board, int width, int totalHeight, Vector2Int[] cells, Vector2Int startPosition)
        {
            var position = startPosition;
            while (true)
            {
                var next = new Vector2Int(position.x, position.y - 1);
                if (!CanPlace(board, width, totalHeight, cells, next))
                {
                    break;
                }
                position = next;
            }
            return position;
        }

        public static int FindFullLines(int[,] board, int width, int playHeight, int[] outIndices)
        {
            var count = 0;
            for (var y = 0; y < playHeight; y++)
            {
                var full = true;
                for (var x = 0; x < width; x++)
                {
                    if (board[x, y] == 0)
                    {
                        full = false;
                        break;
                    }
                }
                if (full)
                {
                    if (count < outIndices.Length)
                    {
                        outIndices[count] = y;
                    }
                    count++;
                }
            }
            return count;
        }

        public static void CollapseLines(int[,] board, int width, int totalHeight, bool[] toClear, int playHeight)
        {
            var writeY = 0;
            for (var readY = 0; readY < totalHeight; readY++)
            {
                var clearedRow = readY < playHeight && toClear[readY];
                if (clearedRow)
                {
                    continue;
                }
                if (writeY != readY)
                {
                    for (var x = 0; x < width; x++)
                    {
                        board[x, writeY] = board[x, readY];
                    }
                }
                writeY++;
            }
            for (var y = writeY; y < totalHeight; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    board[x, y] = 0;
                }
            }
        }
    }
}
