using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Damage Upgrade")]
public class DamageUpgrade : Upgrade
{
    public float minPercent = 5f;
    public float maxPercent = 30f;

    public override float GetRandomValueForRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Random.Range(5f, 10f);
            case Rarity.Rare: return Random.Range(10f, 18f);
            case Rarity.Epic: return Random.Range(18f, 25f);
            case Rarity.Legendary: return Random.Range(25f, 35f);
            default: return Random.Range(minPercent, maxPercent);
        }
    }

    public override void Apply(PlayerMovement player, GunSystem1 gun)
    {
        float rolledPercent = GetRandomValueForRarity(rarity);
        float multiplier = 1f + (rolledValue / 100f);

        gun.baseDamage = Mathf.RoundToInt(gun.baseDamage * multiplier);

        Debug.Log($"[UPGRADE] DMG +{rolledPercent}% ({rarity}) → {gun.baseDamage}");
    }
}






