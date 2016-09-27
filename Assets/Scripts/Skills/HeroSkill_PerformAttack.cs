using UnityEngine;

public class HeroSkill_PerformAttack : HeroSkill
{
    #region Variables
    private AttackData attack_data;
    #endregion Variables

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();

        attack_data = GetComponent<AttackData>();
    }
    #endregion MonoBehaviour

    #region Red
    public override void Effect()
    {
        base.Effect();

        hero.PerformAttack(attack_data);
    }
    #endregion Red
}
