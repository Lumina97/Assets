using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPlayerLivesManager : MonoBehaviour
{
    public Sprite RedShipSprite;
    public Sprite BlueShipSprite;
    public Sprite OrangeShipSprite;
    public Sprite GreenShipSprite;

    [Space]
    public Image LivesDisplayPrefab;

    private List<Image> SpawnedLivesUIElements = new List<Image>();
    private Sprite _spriteToUseForCurrentColor;

    public void Initialize(Sprite _healthSprite, ShipHealth _shipHealth)
    {
        //subscribe to shipHealth onHealthChangedDelegate 
        //to get notified when the health changes
        _shipHealth.OnShipHealthChanged += UpdatePlayerLives;

        _spriteToUseForCurrentColor = _healthSprite;

        SetLivesSprite();
        UpdatePlayerLives(_shipHealth.CurrentLives);
    }

    public void UpdatePlayerLives(int _currentLives,bool _tookDamage = false)
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
        else if( SpawnedLivesUIElements.Count < _currentLives)
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
            //and call this function again to enable/disable the propper amount of ui elements
            UpdatePlayerLives(_currentLives);
        }
    }

    private void CreateNewHealthSprite()
    {
        if(_spriteToUseForCurrentColor != null)
        {
            //spawn image prefab
            Image spawned = Instantiate(LivesDisplayPrefab, transform);
            //..set the sprite to match the player color
            spawned.sprite = _spriteToUseForCurrentColor;
            //.. add to the list
            SpawnedLivesUIElements.Add(spawned);
        }
    }

    private void SetLivesSprite()
    {
        if (SpawnedLivesUIElements != null)
        {
            for (int i = 0; i < SpawnedLivesUIElements.Count; i++)
            {
                SpawnedLivesUIElements[i].sprite = _spriteToUseForCurrentColor;
            }
        }
    }
}
