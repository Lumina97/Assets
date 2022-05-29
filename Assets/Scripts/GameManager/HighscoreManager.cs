using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighscoreManager : MonoBehaviour
{
    public SO_HighScore HighScoreSave;
    public SO_HighScore CurrentHighScore;

    public delegate void OnHighScoreChangedDelegate(float _ScoreToAdd);
    public OnHighScoreChangedDelegate OnHighScoreChanged;

    public void Initialize(GameManager _gameManager)
    {
        OnHighScoreChanged += AddScore;

        if (CurrentHighScore)
            CurrentHighScore.HighScore = 0;

        //subscribe to the game over delegate
        if (_gameManager != null)
            _gameManager.OnGameOver += OnGameOver;
    }

    public void AddScore(float _scoreToAdd)
    {
        if(CurrentHighScore)
            CurrentHighScore.HighScore += _scoreToAdd; ;
    }

    public void OnGameOver()
    {
        //replace highscoresave value with the current
        if (CurrentHighScore.HighScore > HighScoreSave.HighScore)
        {
            HighScoreSave.HighScore = CurrentHighScore.HighScore;
        }
        OnHighScoreChanged -= AddScore;
    }
}
