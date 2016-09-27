using System;

public sealed class Mana : iMana
{
    #region Constructors
    public Mana(float v_max_mp, float v_current_mp=0)
    {
        UnityEngine.Debug.Assert(v_max_mp >= v_current_mp, "ASSERTION FAILED: Maximum mana is less than initial mana.");
        max_mp = v_max_mp;
        current_mp = v_current_mp;
    }
    #endregion Constructors

    #region Variables
    public float current_mp;
    public float max_mp;
    #endregion Variables

    #region iMana
    public float AddMana(float amount)
    {
        return AffectMana(Math.Abs(amount));
    }

    public float SubtractMana(float amount)
    {
        return AffectMana(-Math.Abs(amount));
    }

    public bool UseMana(float cost)
    {
        bool used = false;

        if(current_mp>=cost)
        {
            SubtractMana(cost);
            used = true;
        }

        return used;
    }

    public float CurrentMana()
    {
        return current_mp;
    }

    public float MaxMana()
    {
        return max_mp;
    }
    #endregion iMana

    #region Red
    private float AffectMana(float amount)
    {
        float old_mp = current_mp;

        current_mp += amount;

        current_mp = Math.Max(0, current_mp);

        current_mp = Math.Min(current_mp, max_mp);

        return current_mp - old_mp;
    }
    #endregion Red
}
