using UnityEngine;

public class Explosion : MonoBehaviour
{
    #region Variables
    private AttackData attack_data;

    private Collider2D _collider;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        attack_data = GetComponent<AttackData>();
        _collider = GetComponent<Collider2D>();
    }

    private void Start()
    {
        __Attacks.PerformAttack(attack_data, Vector2.zero, _collider.bounds.center, GetInstanceID());
    }
    #endregion MonoBehaviour
}
