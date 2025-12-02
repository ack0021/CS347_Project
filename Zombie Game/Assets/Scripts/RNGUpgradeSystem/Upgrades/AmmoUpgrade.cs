using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Ammo Upgrade")]
public class AmmoUpgrade : Upgrade
{
    public float minPercent = 10f;
    public float maxPercent = 50f;

    public override float GetRandomValueForRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Random.Range(10f, 20f);
            case Rarity.Uncommon: return Random.Range(20f, 30f);
            case Rarity.Rare: return Random.Range(30f, 40f);
            case Rarity.Epic: return Random.Range(40f, 50f);
            case Rarity.Legendary: return Random.Range(50f, 70f);
            default: return Random.Range(minPercent, maxPercent);
        }
    }

    public override void Apply(PlayerMovement player, GunSystem1 gun)
    {
        float multiplier = 1f + (rolledValue / 100f);

        gun.magazineSize = Mathf.RoundToInt(gun.magazineSize * multiplier);

        gun.SendMessage("ReloadFinished", SendMessageOptions.DontRequireReceiver);
    }
}

