using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Saves and loades Scores to "Dreamlo"
/// based on  "https://www.youtube.com/watch?v=KZuqEyxYZCc"
/// </summary>
public class LeaderboardsManager : MonoBehaviour
{
    public static LeaderboardsManager Instance;
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        GetHighScoresList();
    }

    const string privateCode = "ifb7eTmVAkq_9Jp2aj2evwSYWZPn4b606Y5sIUjaRdcw";
    const string publicCode = "5a50930dd6026605287e96ae";
    const string webURL = "http://dreamlo.com/lb/";

    HighScoreEntries[] HighscoreList;

    public void AddNewHighScore(string _username, float _score)
    {
        StartCoroutine(UploadNewHighscore(_username, _score));
    }

    public HighScoreEntries[] GetHighScoresList()
    {
        StartCoroutine("DownloadHighscores");
        return HighscoreList;
    }

    private IEnumerator UploadNewHighscore(string _username, float _score)
    {
        WWW www = new WWW(webURL + privateCode + "/add/" + WWW.EscapeURL(_username) + "/" + _score);
        yield return www;

        if (string.IsNullOrEmpty(www.error) == false)
            print("Error while uploading score: " + www.error);
    }

    private IEnumerator DownloadHighscores()
    {
        WWW www = new WWW(webURL + publicCode + "/pipe/");
        yield return www;

        //check if the error message is null or empty
        if (string.IsNullOrEmpty(www.error))
        {
            FormatHighScores(www.text);
        }
        else
            print("Error while Downloading score: " + www.error);
    }

    private void FormatHighScores(string _textStream)
    {
        string[] entries = _textStream.Split(new char[] { '\n' }, System.StringSplitOptions.RemoveEmptyEntries);
        HighscoreList = new HighScoreEntries[entries.Length];

        for (int i = 0; i < entries.Length; i++)
        {
            string[] entryinfo = entries[i].Split(new char[] { '|' });
            HighscoreList[i] = new HighScoreEntries(entryinfo[0], float.Parse(entryinfo[1]));
        }
    }
}

public struct HighScoreEntries
{
    public HighScoreEntries(string username, float score)
    {
        Username = username;
        Score = score;
    }
    public string Username;
    public float Score;
}