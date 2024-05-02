using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace XO
{
    public sealed class GameViewModel : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        private GameField _gameField;
        private Turn _currentTurn;

        [Inject]
        internal void Construct(GameField gameField)
        {
            _gameField = gameField;
        }

        private void OnEnable()
        {
            foreach (var (button, cell) in _buttons.Select(static (button, i) => (button, new Vector2Int(x:i / 3, y: i % 3))))
            {
                button.GetComponentInChildren<TMP_Text>().text = string.Empty;
                var componentInChildren = button.GetComponentInChildren<Image>();
                button.onClick.AddListener(() =>
                {
                    var turn = GetCurrentTurn();
                    var result = _gameField.TryMakeTurn(cell, turn, out _);
                    if (!result)
                    {
                        return;
                    }

                    if (_currentTurn is Turn.Cross)
                    {
                        componentInChildren.color = Color.green;
                    }
                    else
                    {
                        componentInChildren.color = Color.red;
                    }

                    button.GetComponentInChildren<TMP_Text>().text = turn.Name;
                    button.interactable = false;
                });
            }
        }

        private Turn GetCurrentTurn()
        {
            if (_currentTurn is null)
            {
                return _currentTurn = new Turn.Cross();
            }
            else if (_currentTurn is Turn.Cross)
            {
                return _currentTurn = new Turn.Circle();
            }
            else if(_currentTurn is Turn.Circle)
            {
                return _currentTurn = new Turn.Cross();
            }

            return new Turn.Available();
        }
    }
}
