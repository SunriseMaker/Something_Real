using UnityEngine;

public class Portal : MonoBehaviour
{
    public enum SpawnSide { Up, Down, Left, Right }

    #region Variables
    public SpawnSide spawn_side;

    public GameObject linked_portal;

    private CircleCollider2D linked_portal_collider;
    private SpawnSide linked_portal_spawn_side;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        if(linked_portal!=null)
        {
            linked_portal_collider = linked_portal.GetComponent<CircleCollider2D>();
            linked_portal_spawn_side = linked_portal.GetComponent<Portal>().spawn_side;
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        // Teleport hero to linked portal
        if (linked_portal != null)
        {
            float offset_x = 0.0f;
            float offset_y = 0.0f;

            switch (linked_portal_spawn_side)
            {
                case SpawnSide.Up:
                    offset_y = linked_portal_collider.radius;
                    break;

                case SpawnSide.Down:
                    offset_y = -linked_portal_collider.radius;
                    break;

                case SpawnSide.Left:
                    offset_x = -linked_portal_collider.radius;
                    break;

                case SpawnSide.Right:
                    offset_x = linked_portal_collider.radius;
                    break;
            }

            col.transform.position = new Vector3(linked_portal.transform.position.x + offset_x, linked_portal.transform.position.y + offset_y);

            if(col.tag == "Player")
            {
                GameData.Singletons.main_camera.FocusOnHero();
            }
        }
    }
    #endregion MonoBehaviour
}
