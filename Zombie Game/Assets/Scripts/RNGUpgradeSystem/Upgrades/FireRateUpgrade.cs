using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/FireRate Upgrade")]
public class FireRateUpgrade : Upgrade
{
    public override float GetRandomValueForRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Random.Range(5f, 10f);
            case Rarity.Rare: return Random.Range(10f, 20f);
            case Rarity.Epic: return Random.Range(20f, 30f);
            case Rarity.Legendary: return Random.Range(30f, 45f);
            default: return 10f;
        }
    }

    public override void Apply(PlayerMovement player, GunSystem1 gun)
    {
        float percent = GetRandomValueForRarity(rarity);
        float multiplier = 1f - (rolledValue / 100f);

        gun.baseFireRate *= multiplier;
        if (gun.baseFireRate < 0.05f)
            gun.baseFireRate = 0.05f; // safety clamp

        Debug.Log($"[UPGRADE] FIRERATE +{percent}% FASTER ({rarity}) → New FireRate = {gun.baseFireRate}");
    }
}





