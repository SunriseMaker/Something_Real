using UnityEngine;

public class HealthItem : InventoryItem
{
    public float heal_effect;

    public HealthItem(bool v_use_once, string v_name, string v_description, float v_heal_effect) : base(v_use_once, v_name, v_description)
    {
        heal_effect = v_heal_effect;
    }

    public override void Use(HeroController hero)
    {
        base.Use(hero);

        hero.Heal(heal_effect);
    }
}
