using UnityEngine;

public class InventoryCell
{
    public InventoryItem inventory_item;

    public GameObject game_object;

    public int quantity;

    public InventoryCell(InventoryItem v_inventory_item, GameObject v_game_object, int v_quantity)
    {
        inventory_item = v_inventory_item;

        game_object = v_game_object;

        quantity = v_quantity;
    }

    public override string ToString()
    {
        return inventory_item._name + " (" + quantity.ToString() + ")";
    }
}
