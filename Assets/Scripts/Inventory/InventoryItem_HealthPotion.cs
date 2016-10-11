using UnityEngine;

public class InventoryItem_HealthPotion : InventoryItem
{
    public GameObject particles;

    [Range(1, 1000)]
    public float hp_amount;

    public override bool Use(HeroController hero)
    {
        bool used = false;

        if (hero.CurrentHealth() < hero.MaxHealth())
        {
            __General.InstantiatePrefab(particles, particles.name, hero.Position_BottomCenter(), particles.transform.rotation, hero.transform, false);
            hero.Heal(hp_amount);
            used = true;
        }

        return used;
    }
}
