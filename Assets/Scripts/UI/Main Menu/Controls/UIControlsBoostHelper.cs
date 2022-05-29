using UnityEngine;
using UnityEngine.UI;

public class UIControlsBoostHelper : MonoBehaviour
{
    public Slider BoostSlider;
    public float DepletionSpeed = 0.005f;
    
	void Update ()
    {
        BoostSlider.value = Mathf.Lerp(BoostSlider.value - DepletionSpeed, 0, DepletionSpeed * Time.deltaTime);
        if(BoostSlider.value <= BoostSlider.minValue)
        {
            BoostSlider.value = BoostSlider.maxValue;
        }
    }
}
