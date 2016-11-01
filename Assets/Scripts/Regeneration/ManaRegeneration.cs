using UnityEngine;

public sealed class ManaRegeneration : Regeneration
{
    #region Variables
    [Tooltip("How much MP will be restored at a time.")]
    [SerializeField]
    private float mp_amount;

    private iMana _mana;
    #endregion Variables

    #region MonoBehaviour
    protected override void Awake()
    {
        base.Awake();

        _mana = GetComponent<iMana>();
    }
    #endregion MonoBehaviour

    #region Red
    protected override void Effect()
    {
        _mana.AddMana(mp_amount);
    }
    #endregion Red	
}
