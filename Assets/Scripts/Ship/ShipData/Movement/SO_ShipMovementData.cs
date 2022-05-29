using UnityEngine;

[CreateAssetMenu(menuName = "Ship/Data/Movement")]
public class SO_ShipMovementData : ScriptableObject
{
    //movement
    public float AccellerationSpeed;
    public AudioClip AccelerationAudioClip;

    //Booster
    [Space]
    [Range(0, 1f)]
    public float BoosterDepletionSpeed;
    [Range(0,10)]
    public float BoosterRechargeDelay;
    [Range(0f, 1f)]
    public float BoosterRechargeSpeed;
}