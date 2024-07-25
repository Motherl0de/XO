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
                    _canvasWin.GetComponentInChildren<RawImage>().transform.DOScale(new Vector3(6f,6f,6f),3f);
                    await UniTask.Delay(4000);
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
                    DOTween.Sequence()
                        .Append(button.GetComponentInChildren<TMP_Text>().transform.DOScale(2f, 1f))
                        .AppendInterval(0.2f)
                        .Append(button.GetComponentInChildren<TMP_Text>().transform.DOScale(1f, 1f));

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
