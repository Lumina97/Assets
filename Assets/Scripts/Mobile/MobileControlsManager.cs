using UnityEngine;

public class MobileControlsManager : MonoBehaviour
{
    public static MobileControlsManager Instance;

    void Awake()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer)
        {
            gameObject.SetActive(false);
        }
        else
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
