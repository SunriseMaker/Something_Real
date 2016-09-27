using UnityEngine;

public sealed class ProjectileAttackData : AttackData
{
    public enum ProjectileImpactBehaviour { penetrate, destroy, stick };

    public ProjectileImpactBehaviour behaviour;

    public GameObject projectile_prefab;

    public float life_time;

    [Tooltip("Speed of kinematic projectile\nForce applied to non-kinematic projectile")]
    public float speed_effect;

    public bool is_kinematic;

    [Tooltip("Override of Y coordinate of the launch direction vector.\nZero value means launch direction isn't overrided.")]
    [Range(-1,1)]
    public float override_y;
}
