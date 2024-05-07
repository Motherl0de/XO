using System.Linq;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using VContainer;

namespace XO
{
    public sealed class GameViewModel : MonoBehaviour
    {
        [SerializeField] private Button[] _buttons;
        [SerializeField] private Canvas _canvasWin;
        private GameField _gameField;
        private Turn _currentTurn;

        [Inject]
        internal void Construct(GameField gameField)
        {
            _gameField = gameField;
        }

        private void OnEnable()
        {
             _gameField.OnWinner(async (winner) =>
            {
                foreach (var button in _buttons)
                {
                    button.interactable = false;
                    _canvasWin.gameObject.SetActive(true);
                    _canvasWin.GetComponentInChildren<TMP_Text>().text = "Win " + winner.Name;
                    _canvasWin.GetComponentInChildren<RawImage>().transform.DOScale(new Vector3(4f,4f,4f),3f);
                    await UniTask.Delay(2000);
                    _canvasWin.gameObject.SetActive(false);
                    SceneManager.LoadSceneAsync("Scenes/SampleScene");
                }
            });


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

                    button.GetComponentInChildren<TMP_Text>().text = turn.Name;
                    button.interactable = false;
                });
                button.onClick.AddListener(() =>
                {
                    componentInChildren.color = _currentTurn is Turn.Cross ? Color.green : Color.red;
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
