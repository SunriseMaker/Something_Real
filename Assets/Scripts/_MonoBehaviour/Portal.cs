using UnityEngine;

public class Portal : MonoBehaviour
{
    #region Variables
    public enum SpawnSide { Up, Down, Left, Right }

    [SerializeField]
    private SpawnSide spawn_side;

    [SerializeField]
    private GameObject linked_portal;

    [HideInInspector]
    public Collider2D _collider;

    private Collider2D linked_portal_collider;

    private SpawnSide linked_portal_spawn_side;
    public bool works;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();

        Debug.Assert(_collider != null);

        if(linked_portal!=null)
        {
            linked_portal_collider = linked_portal.GetComponent<Collider2D>();
            linked_portal_spawn_side = linked_portal.GetComponent<Portal>().spawn_side;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(!works)
        {
            return;
        }

        // Teleport hero to linked portal
        if (linked_portal == null)
        {
            return;
        }

        iDimensions dimensions = col.GetComponent<iDimensions>();

        if(dimensions==null)
        {
            return;
        }

        ObjectSize size = dimensions.Size();

        float x = 0.0f;
        float y = 0.0f;

        switch (linked_portal_spawn_side)
        {
            case SpawnSide.Up:
                x = linked_portal_collider.bounds.center.x;
                y = linked_portal_collider.bounds.max.y+size.height;
                break;

            case SpawnSide.Down:
                x = linked_portal_collider.bounds.center.x;
                y = linked_portal_collider.bounds.min.y-size.height;
                break;

            case SpawnSide.Left:
                x = linked_portal_collider.bounds.min.x-size.width;
                y = linked_portal_collider.bounds.min.y;
                break;

            case SpawnSide.Right:
                x = linked_portal_collider.bounds.max.x+size.width;
                y = linked_portal_collider.bounds.min.y;
                break;
        }

        col.transform.position = new Vector3(x, y, 0);
    }
    #endregion MonoBehaviour
}
