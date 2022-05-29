using UnityEngine;
using UnityEngine.SceneManagement;

public class UIGameOptionsManager : MonoBehaviour
{
    [Space]
    [Range(1,4)]
    public int players = 1;

    [Space]
    public UIShipSelectManager ShipSelectManager;

    public delegate void OnGameOptionsManagerClosedDelegate();
    public OnGameOptionsManagerClosedDelegate OnGameOptionsClosed;

    private void OnEnable()
    {
        if(ShipSelectManager)
        {
            ShipSelectManager.gameObject.SetActive(true);
            ShipSelectManager.InitializePlayers(players);
        }
    }

    public void BackToMainMenu()
    {
        if (ShipSelectManager) ShipSelectManager.CleanUpPlayers();

        if (OnGameOptionsClosed != null)
            OnGameOptionsClosed();

        gameObject.SetActive(false);
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Level_01");
    }
}
