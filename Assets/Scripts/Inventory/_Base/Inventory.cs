using UnityEngine;
using System.Linq;

public class Inventory : MonoBehaviour
{
    #region Variables
    private PlayerController player;

    [HideInInspector]
    public InventoryItem _current_item;

    private System.Collections.Generic.List<InventoryItem> items;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        player = GetComponent<PlayerController>();
        items = new System.Collections.Generic.List<InventoryItem>();
    }
    #endregion MonoBehaviour

    #region Red
    public void AddItem(InventoryItem item)
    {
        if(item!=null)
        {
            items.Add(item);
        }
    }

    public void RemoveItem()
    {
        if(_current_item!=null)
        {
            items.Remove(_current_item);
        }
    }

    public void UseItem()
    {
        if(_current_item!=null)
        {
            _current_item.Use(player.current_hero);

            if (_current_item._use_once)
            {
                RemoveItem();
            }
        }
    }

    public void SelectItem()
    {
        _current_item = items.First();
    }

    public void ShowItems()
    {
        print("**********************");
        print("Inventory -> ShowItems");

        foreach (InventoryItem i in items)
        {
            print(string.Format("Name: {0}, Description: {1}", i._name, i._description));
        }

        print("**********************");
    }
    #endregion Red
}
