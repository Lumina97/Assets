using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILeaderboardsEntry : MonoBehaviour
{
    [SerializeField] private Text UsernameText;
    [SerializeField] private Text ScoreText;
    public void Initialize( string _username, float _score)
    {
        if(UsernameText)
        {
            UsernameText.text = _username;
        }

        if(ScoreText)
        {
            ScoreText.text = _score.ToString();
        }
    }
}
