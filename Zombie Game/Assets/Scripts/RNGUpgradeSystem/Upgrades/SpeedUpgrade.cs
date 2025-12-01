using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Upgrades/Speed Upgrade")]
public class SpeedUpgrade : Upgrade
{
    public override float GetRandomValueForRarity(Rarity rarity)
    {
        switch (rarity)
        {
            case Rarity.Common: return Random.Range(5f, 10f);
            case Rarity.Rare: return Random.Range(10f, 18f);
            case Rarity.Epic: return Random.Range(18f, 25f);
            case Rarity.Legendary: return Random.Range(25f, 35f);
            default: return 10f;
        }
    }

    public override void Apply(PlayerMovement player, GunSystem1 gun)
    {
        float percent = GetRandomValueForRarity(rarity);
        float multiplier = 1f + (rolledValue / 100f);

        player.baseSpeed *= multiplier;

        Debug.Log($"[UPGRADE] SPEED +{percent}% ({rarity}) → New Speed = {player.baseSpeed}");
    }
}





