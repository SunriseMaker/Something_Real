using UnityEngine;

public class Pickup : MonoBehaviour
{
    public bool inventory_item_use_once;
    public string inventory_item_name;
    public string inventory_item_description;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.tag=="Player")
        {
            Inventory _inventory = col.transform.root.GetComponent<Inventory>();

            if (_inventory != null)
            {
                AddToInventory(_inventory);
                Destroy(gameObject);
            }
        }
    }

    protected virtual void AddToInventory(Inventory inventory)
    {
        inventory.AddItem(new InventoryItem(inventory_item_use_once, inventory_item_name, inventory_item_description));
    }
}
