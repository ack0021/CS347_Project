using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Rarity
{
    Common,
    Uncommon,
    Rare,
    Epic,
    Legendary
}

public abstract class Upgrade : ScriptableObject
{
    public string upgradeName;
    public string description;
    public Sprite icon;
    public float rolledValue;

    public Rarity rarity;

    public abstract void Apply(PlayerMovement player, GunSystem1 gun);
    public abstract float GetRandomValueForRarity(Rarity rarity);

    public static readonly float[] rarityChances = new float[]
    {
        40f, // Common
        30f, // Uncommon
        20f, // Rare
        7f,  // Epic
        3f   // Legendary
    };

    public static Rarity RollRarity()
    {
        float roll = Random.Range(0f, 100f);
        float cumulative = 0f;

        for (int i = 0; i < rarityChances.Length; i++)
        {
            cumulative += rarityChances[i];
            if (roll <= cumulative)
                return (Rarity)i;
        }

        return Rarity.Common;
    }
}







