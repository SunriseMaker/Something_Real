using UnityEngine;

public class HeroSkill : MonoBehaviour
{
    public enum SkillType { Offensive, Defensive }

    #region Variables
    public SkillType skill_type;

    [Tooltip("True: Skill effect starts immediately on use.\nFalse: Skill effect is supposed to be triggered on particular animation frame.")]
    public bool instant_effect;

    [Tooltip("Specify trigger name to play animation on skill use. Note that skill effect can be triggered on particular animation frame.")]
    public string animator_trigger_name;

    public float mana_cost;

    public Sprite HUD_sprite;

    [Tooltip("Delay in seconds between uses of skill.")]
    public float base_delay;

    protected bool can_use;

    protected int level;

    protected HeroController hero;
    #endregion Variables

    #region MonoBehaviour
    public void SetHeroReference(HeroController v_hero)
    {
        hero = v_hero;
    }

    public bool Use()
    {
        bool used = false;

        if (can_use)
        {
            if (hero.CurrentMana() >= mana_cost)
            {
                used = true;

                hero.SetSkill(this);

                if (animator_trigger_name != "")
                {
                    hero.SetAnimatorTrigger(animator_trigger_name);
                }

                if (instant_effect)
                {
                    Effect();
                }

                float delay_time = CalcDelay();

                if (delay_time > 0)
                {
                    StartCoroutine(Delay(delay_time));
                }
            }
        }

        return used;
    }

    public virtual void Effect()
    {
        hero.SubtractMana(mana_cost);
    }

    protected virtual void Awake()
    {
        can_use = true;
        level = 1;
    }
    #endregion MonoBehaviour

    #region Red
    protected virtual float CalcDelay()
    {
        return base_delay;
    }

    protected System.Collections.IEnumerator Delay(float delay_time)
    {
        can_use = false;

        yield return new WaitForSeconds(delay_time);

        can_use = true;
    }
    #endregion Red
}
