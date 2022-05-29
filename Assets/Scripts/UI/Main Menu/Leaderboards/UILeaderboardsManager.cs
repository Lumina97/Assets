using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UILeaderboardsManager : MonoBehaviour
{
    public RectTransform LeaderboardsEntriesParent;
    public UILeaderboardsEntry LeaderboardsEntryPrefab;
    public GameObject LoadingHighscoresTextObj;

    private GameObject[] _highscoreEntriesObjects;

    private void OnEnable()
    {
        if (LoadingHighscoresTextObj)
        {
            LoadingHighscoresTextObj.SetActive(true);
        }
        StartCoroutine("GetLeaderboards");
    }

    private IEnumerator GetLeaderboards()
    {
        ClearList();
        if (LeaderboardsManager.Instance != null && LeaderboardsEntriesParent && LeaderboardsEntryPrefab)
        {
            HighScoreEntries[] entries = LeaderboardsManager.Instance.GetHighScoresList();
            if (entries != null)
            {
                _highscoreEntriesObjects = new GameObject[entries.Length];
                if (LoadingHighscoresTextObj)
                {
                    LoadingHighscoresTextObj.SetActive(false);
                }

                for (int i = 0; i < entries.Length; i++)
                {
                    UILeaderboardsEntry entry = Instantiate(LeaderboardsEntryPrefab, LeaderboardsEntriesParent);
                    entry.Initialize(entries[i].Username, entries[i].Score);

                    _highscoreEntriesObjects[i] = entry.gameObject;
                }
            }
            else if ( entries == null)
            {
                if (LoadingHighscoresTextObj)
                {
                    LoadingHighscoresTextObj.SetActive(true);
                }
                StartCoroutine("GetLeaderboards");
            }
            yield return null;
        }
    }
    
    private void ClearList()
    {
        if(_highscoreEntriesObjects != null)
        {
            for (int i = 0; i < _highscoreEntriesObjects.Length; i++)
            {
                Destroy(_highscoreEntriesObjects[i]);
            }
            _highscoreEntriesObjects = null;
        }

        if(LeaderboardsEntriesParent && LeaderboardsEntriesParent.childCount > 0)
        {
            for (int i = 0; i <= LeaderboardsEntriesParent.childCount; i++)
            {
                Destroy(LeaderboardsEntriesParent.GetChild(0).gameObject);
            }
        }
    }

    private void OnDisable()
    {
        if (LeaderboardsEntriesParent && LeaderboardsEntriesParent.childCount > 0)
        {
            for (int i = 0; i <= LeaderboardsEntriesParent.childCount; i++)
            {
                Destroy(LeaderboardsEntriesParent.GetChild(0).gameObject);
            }
        }
    }
}
