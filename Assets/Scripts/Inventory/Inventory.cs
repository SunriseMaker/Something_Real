using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class Inventory
{
    #region Variables
    private List<InventoryCell> items;

    private Transform inventory_group;

    const string INVENTORY_GROUP = "Inventory";
    #endregion Variables

    public Inventory(Transform owner)
    {
        items = new List<InventoryCell>();

        inventory_group = new GameObject(INVENTORY_GROUP).transform;
        inventory_group.SetParent(owner);
    }

    #region Red
    public void AddItem(GameObject inventory_item_prefab, InventoryItem inventory_item_component, int quantity)
    {
        string inventory_item_name = inventory_item_component._name;

        if (quantity>0)
        {
            InventoryCell ic = FindCell(inventory_item_name);

            if(ic==null)
            {
                GameObject item_gameobject = __General.InstantiatePrefab(inventory_item_prefab, inventory_item_name, Vector3.zero, Quaternion.Euler(Vector3.zero), inventory_group.transform, false);
                InventoryItem item_component = item_gameobject.GetComponent<InventoryItem>();
                items.Add(new InventoryCell(item_component, item_gameobject, quantity));
            }
            else
            {
                ic.quantity += quantity;
            }
        }
    }

    public void RemoveItemInCell(ref InventoryCell cell)
    {
        Object.Destroy(cell.game_object);
        items.Remove(cell);
        cell = null;
    }

    public bool UseItemInCell(ref InventoryCell cell, HeroController hero)
    {
        bool used = false;

        if(cell.inventory_item.Use(hero))
        {
            used = true;

            if(cell.inventory_item._use_once)
            {
                cell.quantity--;
            }

            if(cell.quantity==0)
            {
                RemoveItemInCell(ref cell);
            }
        }

        return used;
    }

    public InventoryCell GetNextInventoryCell(InventoryCell current_cell, int forward)
    {
        InventoryCell next_cell = null;

        if (items.Count!=0)
        {
            bool get_first = false;

            if (current_cell == null)
            {
                get_first = true;
            }
            else
            {
                int index = IndexOfCell(current_cell);

                if (index == -1)
                {
                    get_first = true;
                }
                else
                {
                    int next_index = index + forward;
                    next_index = System.Math.Max(0, next_index);
                    next_index = System.Math.Min(next_index, items.Count - 1);

                    next_cell = items[next_index];
                }
            }

            if (get_first)
            {
                next_cell = items.First();
            }
        }

        return next_cell;
    }
    #endregion Red

    #region Find
    private InventoryCell FindCell(InventoryCell item)
    {
        return items.Find((x) => x == item);
    }

    private InventoryCell FindCell(string item_name)
    {
        return items.Find((x) => x.inventory_item._name == item_name);
    }

    private int IndexOfCell(InventoryCell item)
    {
        return items.IndexOf(FindCell(item));
    }
    #endregion Find
}
