using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIGameOverManager : MonoBehaviour
{
    public RectTransform GameOverTextObject;
    public GameObject ButtonsObject;
    public Image BackgroundImage;
    public float BackgroundImageFadeSpeed;
    public float GameoverTextAppearSpeed;

    public SO_HighScore HighScore;
    private string HighScoreEntryName;

    private void OnEnable()
    {
        MouseManager.ToggleMouseLock(false);
        GameOverTextObject.localScale = Vector3.zero;
        BackgroundImage.color = new Color(0, 0, 0, 0);
        SetButtonActiveState(false);
    }

    private void SetButtonActiveState(bool _activestate)
    {
        ButtonsObject.SetActive(_activestate);
    }

    void Update()
    {
        if(GameOverTextObject && GameOverTextObject.localScale != Vector3.one)
        {
            GameOverTextObject.localScale = Vector3.Slerp(GameOverTextObject.localScale, Vector3.one, GameoverTextAppearSpeed * Time.deltaTime);
        }

        if (BackgroundImage && BackgroundImage.color.a != 1)
        {
            float a = Mathf.Lerp(BackgroundImage.color.a, 1, BackgroundImageFadeSpeed * Time.deltaTime);
            BackgroundImage.color = new Color(0,0,0,a);
        }

        if (GameOverTextObject.localScale.x >= 0.75f)
            SetButtonActiveState(true);
    }
    
    public void RestartGame()
    {
        GameManager.Instance.RestartGame();
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ChangeHighScoreEntryName(string _newName)
    {
        HighScoreEntryName = _newName;
    }

    public void SaveScore()
    {
        if(HighScore && LeaderboardsManager.Instance)
        {
            LeaderboardsManager.Instance.AddNewHighScore(HighScoreEntryName, HighScore.HighScore);
        }
    }
}
