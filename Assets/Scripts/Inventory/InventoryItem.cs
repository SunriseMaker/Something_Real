using UnityEngine;

public class InventoryItem : MonoBehaviour
{
    #region Variables
    public bool _use_once;

    public string _name;

    public string _description;

    public Sprite hud_sprite;
    #endregion Variables

    #region Red
    public virtual bool Use(HeroController hero)
    {
        return true;
    }
    #endregion Red
}
