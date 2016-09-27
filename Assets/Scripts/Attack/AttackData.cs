using UnityEngine;

public class AttackData : MonoBehaviour
{
    [Range(0, 20)]
    public float range;

    [Range(0, 5)]
    public float radius;

    public float damage;

    public float impact_force_amount;

    public bool always_apply_impact_force;

    

    public bool penetrate_enemies;

    public bool penetrate_walls;

    public AudioClip sound;
}
