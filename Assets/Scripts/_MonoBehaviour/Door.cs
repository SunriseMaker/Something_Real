using UnityEngine;

public class Door : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private string key_name;

    [SerializeField]
    private bool locked;

    private Portal _portal;

    private Animator _animator;

    private int AP_locked;
    #endregion Variables

    #region MonoBehaviour
    private void Start()
    {
        _animator = GetComponent<Animator>();
        AP_locked = Animator.StringToHash("Locked");
        _portal = GetComponent<Portal>();
        Refresh();
    }

    public bool Unlock(string key)
    {
        bool unlocked = false;

        if(locked)
        {
            if(key==key_name)
            {
                locked = false;
                unlocked = true;
                Refresh();
            }
        }

        return unlocked;
    }

    private void Refresh()
    {
        _animator.SetBool(AP_locked, locked);
        _portal.works = !locked;
    }
    #endregion MonoBehaviour
}
