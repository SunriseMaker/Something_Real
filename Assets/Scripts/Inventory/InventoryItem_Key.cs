using UnityEngine;

public class InventoryItem_Key : InventoryItem
{
    public override bool Use(HeroController hero)
    {
        bool used = false;

        RaycastHit2D[] raycasts = Physics2D.CircleCastAll(hero.Position(), hero.Size().height, Vector3.zero, 1, GameData.LayerMasks.IntaractableObjects);

        foreach(RaycastHit2D r in raycasts)
        {
            Door door = r.collider.GetComponent<Door>();

            if(door!=null)
            {
                if(door.Unlock(_name))
                {
                    used = true;
                }
            }
        }

        return used;
    }
}
