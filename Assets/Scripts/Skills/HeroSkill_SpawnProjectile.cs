using UnityEngine;

public class HeroSkill_SpawnProjectile : HeroSkill
{
    #region Variables
    private ProjectileAttackData attack_data;
    #endregion Variables

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();

        attack_data = GetComponent<ProjectileAttackData>();
    }
    #endregion MonoBehaviour

    #region Red
    public override void Effect()
    {
        base.Effect();

        hero.SpawnProjectile(attack_data);
    }
    #endregion Red
}
