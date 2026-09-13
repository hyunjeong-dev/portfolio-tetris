using UnityEngine;
using UnityEngine.UI;

namespace Game.Features.Tetris
{
    public class UITetrisScorePanel : MonoBehaviour
    {
        [SerializeField] Text _scoreText;
        [SerializeField] Text _linesText;
        [SerializeField] Text _levelText;

        int _lastScore = int.MinValue;
        int _lastLines = int.MinValue;
        int _lastLevel = int.MinValue;

        public void UpdateView(TetrisScoreData data)
        {
            if (data == null)
            {
                return;
            }
            if (_scoreText != null && data.Score != _lastScore)
            {
                _scoreText.text = "Score " + data.Score;
                _lastScore = data.Score;
            }
            if (_linesText != null && data.Lines != _lastLines)
            {
                _linesText.text = "Lines " + data.Lines;
                _lastLines = data.Lines;
            }
            if (_levelText != null && data.Level != _lastLevel)
            {
                _levelText.text = "Lv " + data.Level;
                _lastLevel = data.Level;
            }
        }
    }
}
