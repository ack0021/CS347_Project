using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Health Upgrade")]
public class HealthUpgrade : Upgrade
{
    public override void Apply(PlayerMovement player, GunSystem1 gun)
    {
        player.maxHealth += rolledValue;
        player.currentHealth += rolledValue; // optional heal
    }

    public override float GetRandomValueForRarity(Rarity rarity)
    {
        return rarity switch
        {
            Rarity.Common => Random.Range(5f, 10f),
            Rarity.Uncommon => Random.Range(10f, 15f),
            Rarity.Rare => Random.Range(15f, 20f),
            Rarity.Epic => Random.Range(20f, 30f),
            Rarity.Legendary => Random.Range(30f, 40f),
            _ => 5f,
        };
    }
}

