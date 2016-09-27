using UnityEngine;

public class InventoryItem
{
    public bool _use_once;
    public string _name;
    public string _description;

    public InventoryItem(bool v_use_once, string v_name, string v_description)
    {
        _use_once = v_use_once;
        _name = v_name;
        _description = v_description;
    }

    public virtual void Use(HeroController hero)
    {
    }
}
