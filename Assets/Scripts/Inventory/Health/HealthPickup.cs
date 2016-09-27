using UnityEngine;

public class HealthPickup : Pickup
{
    public float health_item_heal_effect;

    protected override void AddToInventory(Inventory inventory)
    {
        inventory.AddItem(new HealthItem(inventory_item_use_once, inventory_item_name, inventory_item_description, health_item_heal_effect));
    }
}
