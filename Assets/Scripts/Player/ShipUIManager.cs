using UnityEngine;
using UnityEngine.UI;

public class ShipUIManager : MonoBehaviour
{
    [Space]
    [Header("UI Elements")]
    public Text PlayerNameText;
    public Slider ShipBoosterSlider;
    public Image ShipBoosterSliderFillImage;
    public Image BoosterBackgroundImage;
    public UIPlayerLivesManager PlayerLivesUI;

    private ShipMotor _motor;
    private GameObject _shipToAttachTo;

    public void Initialize(PlayerGameSettingsHook _playerSettings, ShipMotor _shipMotor, ShipHealth _shipHealth)
    {
        if (_shipMotor != null)
        {
            _motor = _shipMotor;
            _shipToAttachTo = _motor.gameObject;
        }

        if (PlayerLivesUI && _shipHealth != null)
        {
            PlayerLivesUI.Initialize(_playerSettings.PlayerLivesSprite, _shipHealth);
        }

        if(PlayerNameText)
        {
            PlayerNameText.text = _playerSettings.PlayerName;
        }

        SetUiElementsColor(_playerSettings);
    }

    private void LateUpdate()
    {
        if (_shipToAttachTo)
        {
            Vector2 pos = new Vector2(_shipToAttachTo.transform.position.x, _shipToAttachTo.transform.position.y - 1);
            transform.position = pos;
        }

        if (_motor)
        {
            SetBoosterValue(_motor.GetCurrentBoosterReserves);
        }
    }

    private void SetUiElementsColor(PlayerGameSettingsHook _playerSettings)
    {
        if (ShipBoosterSliderFillImage)
        {
            ShipBoosterSliderFillImage.color = _playerSettings.PlayerBoosterBarColor;
        }
        if (BoosterBackgroundImage)
        {
            BoosterBackgroundImage.sprite = _playerSettings.BackgroundSprite;
        }
    }

    private void SetBoosterValue(float _value)
    {
        if (ShipBoosterSlider)
        {
            ShipBoosterSlider.value = _value;
        }
    }
}
