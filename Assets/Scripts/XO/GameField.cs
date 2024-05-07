using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;

namespace XO
{
    [UsedImplicitly]
    internal sealed class GameField
    {
        private readonly Turn[,] _turns = new Turn[3, 3];
        private int _counter;
        private Action _moveExecuted;

        public bool TryMakeTurn(Vector2Int cell, Turn turn, out IEnumerable<Vector2Int> cells)
        {
            var check = CheckAvailableTurns();
            if (check is false)
            {
                cells = Enumerable.Empty<Vector2Int>();
                return false;
            }

            if (TryNextTurn(turn, cell) is false)
            {
                cells = Enumerable.Empty<Vector2Int>();
                return false;
            }

            cells = AvailableCells();
            _moveExecuted?.Invoke();
            return true;
        }

        public void OnWinner(Action<Turn> whenWinner)
        {
            _moveExecuted = () =>
            {
                if (TryGetWinner(out var winner))
                {
                    whenWinner.Invoke(winner);
                }
            };
        }

        public bool TryGetWinner(out Turn candidate)
        {
            var crossPattern = GamePatterns.GetPattern<Turn.Cross, Turn>(_turns);
            var circlePattern = GamePatterns.GetPattern<Turn.Circle, Turn>(_turns);
            foreach (var pattern in GamePatterns.WinPatterns)
            {
                if ((crossPattern & pattern) == pattern)
                {
                    candidate = new Turn.Cross();
                    return true;
                }
                else if ((circlePattern & pattern) == pattern)
                {
                    candidate = new Turn.Circle();
                    return true;
                }
            }

            candidate = default;
            return false;
        }

        private IEnumerable<Vector2Int> AvailableCells()
        {
            for (var x = 0; x < _turns.GetLength(0); x++)
            {
                for (var y = 0; y < _turns.GetLength(1); y++)
                {
                    var cell = _turns[x, y];
                    if (cell is not Turn.Available)
                    {
                        yield return new Vector2Int(x, y);
                    }
                }
            }
        }

        private bool TryNextTurn(Turn turn, Vector2Int cell)
        {
            switch (_counter % 2)
            {
                case 0 when turn is Turn.Cross cross:
                    _counter++;
                    _turns[cell.x, cell.y] = cross;
                    return true;
                case 1 when turn is Turn.Circle circle:
                    _counter++;
                    _turns[cell.x, cell.y] = circle;
                    return true;
                default:
                    return false;
            }
        }

        private bool CheckAvailableTurns()
        {
            if (_counter >= _turns.Length)
            {
                return false;
            }

            return GameIsFinished is false;
        }

        private bool GameIsFinished => TryGetWinner(out _);
    }
}
