using UnityEngine;

public class Pickup : MonoBehaviour
{
    public GameObject inventory_item_prefab;

    private InventoryItem inventory_item_component;

    public int quantity;

    private void Awake()
    {
        inventory_item_component = inventory_item_prefab.GetComponent<InventoryItem>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        PlayerController _player = col.transform.root.GetComponent<PlayerController>();

        if (_player != null)
        {
            _player.AddInventoryItem(inventory_item_prefab, inventory_item_component, quantity);
            AudioSource.PlayClipAtPoint(GamePrefabs.SFX.pickup, transform.position);
            Destroy(gameObject);
        }
    }
}
