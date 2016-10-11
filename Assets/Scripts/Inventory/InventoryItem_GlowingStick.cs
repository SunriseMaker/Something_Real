using UnityEngine;

public class InventoryItem_GlowingStick : InventoryItem
{
    ProjectileAttackData attack_data;

    private void Awake()
    {
        attack_data = GetComponent<ProjectileAttackData>();
    }

    public override bool Use(HeroController hero)
    {
        base.Use(hero);

        hero.SpawnProjectile(attack_data);

        return true;
    }
}
