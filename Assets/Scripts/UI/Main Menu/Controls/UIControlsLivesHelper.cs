using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIControlsLivesHelper : MonoBehaviour
{
    public Sprite BlueShipSprite;

    [Space]
    public Image LivesDisplayPrefab;

    public float LivesDecreaseTime;

    private List<Image> SpawnedLivesUIElements = new List<Image>();

    private int maxlives = 3;
    private int currentlives;

    private void OnEnable()
    {
        StartCoroutine(ChangeLives());
    }

    private IEnumerator ChangeLives()
    {
        UpdatePlayerLives(currentlives);
        currentlives--;
        if (currentlives < 0)
            currentlives = maxlives;
        yield return new WaitForSeconds(LivesDecreaseTime);
        StartCoroutine(ChangeLives());
    }

    public void UpdatePlayerLives(int _currentLives)
    {
        //check if the list is null and if we have enough images to display currentlives
        if (SpawnedLivesUIElements != null && SpawnedLivesUIElements.Count >= _currentLives)
        {
            //..if so we iterate all ui elements 
            for (int i = 0; i < SpawnedLivesUIElements.Count; i++)
            {
                //and disable all that are to much..
                if (i < _currentLives)
                    SpawnedLivesUIElements[i].enabled = true;
                else
                    SpawnedLivesUIElements[i].enabled = false;
            }
        }
        else if (SpawnedLivesUIElements.Count < _currentLives)
        {
            //if the list has not been initialized 
            if (SpawnedLivesUIElements == null)
                //initialize it
                SpawnedLivesUIElements = new List<Image>();

            //then iterate over the difference of the listlenght and the currentlives
            for (int i = SpawnedLivesUIElements.Count; i < _currentLives; i++)
            {
                //to create new ui elements
                CreateNewHealthSprite();
            }
        }
    }

    private void CreateNewHealthSprite()
    {
        if (BlueShipSprite != null)
        {
            //spawn image prefab
            Image spawned = Instantiate(LivesDisplayPrefab, transform);
            //..set the sprite to match the player color
            spawned.sprite = BlueShipSprite;
            //.. add to the list
            SpawnedLivesUIElements.Add(spawned);
        }
    }

    private void SetLivesSprite(Sprite _shipHealthSprite)
    {
        if (SpawnedLivesUIElements != null)
        {
            for (int i = 0; i < SpawnedLivesUIElements.Count; i++)
            {
                SpawnedLivesUIElements[i].sprite = _shipHealthSprite;
            }
        }
    }
}
