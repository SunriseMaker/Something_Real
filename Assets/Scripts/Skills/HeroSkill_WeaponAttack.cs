using UnityEngine;

public class HeroSkill_WeaponAttack : HeroSkill
{
    #region Red
    public override void Effect()
    {
        base.Effect();

        hero.WeaponAttack();
    }
    #endregion Red
}
