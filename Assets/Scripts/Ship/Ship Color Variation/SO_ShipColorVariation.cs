using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu( menuName = "Ship/ColorVariation")]
public class SO_ShipColorVariation : ScriptableObject
{
    public Sprite[] ShipSprites;
    public Sprite[] ShipLivesSprites;
    public Sprite BackGroundSprites;
    public Color BoosterColors;
    public GameObject DeathParticles;

    [HideInInspector] public int VariationPos;
}
