using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHighScoreManager : MonoBehaviour
{
    public Text ScoreValueText;
    HighscoreManager _highscoreManager;

    /// <summary>
    /// paramater can be ignored since we have a reference to the highscoremanager
    /// </summary>
    /// <param name="_scoreAdded"></param>
    private void UpdateScore(float _scoreAdded)
    {
        if (_highscoreManager)
        {
            if (_highscoreManager && ScoreValueText.isActiveAndEnabled)
            {
                ScoreValueText.text = _highscoreManager.CurrentHighScore.HighScore.ToString();
            }
        }
    }

    private void OnDestroy()
    {
        if (_highscoreManager)
        {
            _highscoreManager.OnHighScoreChanged -= UpdateScore;
        }
    }

    private void Initialize()
    {
        if (_highscoreManager == null)
        {
            _highscoreManager = GameManager.Instance.HighscoreManager;
        }

        if (_highscoreManager)
        {
            _highscoreManager.OnHighScoreChanged += UpdateScore;
            ScoreValueText.text = _highscoreManager.CurrentHighScore.HighScore.ToString();
        }
        else
            ScoreValueText.text = "Could not get HighScoreManager!";
    }

    private void Start()
    {
        Initialize();
    }
}
