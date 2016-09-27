using UnityEngine;

public class HeroSkill_Invisibility : HeroSkill
{
    #region Variables
    public float base_duration;

    [Tooltip("Hero Transparancy coefficient.")]
    [Range(0.0f, 1.0f)]
    public float alpha;
    #endregion Variables

    #region Red
    public override void Effect()
    {
        base.Effect();

        float duration = base_duration * level;

        StartCoroutine(hero.Invisibility(duration, alpha));
    }
    #endregion Red
}