using UnityEngine;

public class FallingPlatform : MonoBehaviour
{
    public enum FallingPlatformBehaviour { BringBack, Deactivate }

    #region Variables
    public FallingPlatformBehaviour _behaviour;

    public float falling_time;
    public float trembling_time;

    private Animator _animator;

    int AP_Trembling;
    int AP_Falling;
    int AP_Idle;

    private bool is_falling;
    private float falling_timer;

    private bool is_trembling;
    private float trembling_timer;

    private Vector2 initial_position;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        _animator = GetComponent<Animator>();

        AP_Idle = Animator.StringToHash("Idle");
        AP_Trembling = Animator.StringToHash("Trembling");
        AP_Falling = Animator.StringToHash("Falling");

        initial_position = transform.position;

        is_falling = false;
        falling_timer = 0.0f;

        is_trembling = false;
        trembling_timer = 0.0f;
    }

    private void FixedUpdate()
    {
        if(is_trembling)
        {
            trembling_timer += Time.deltaTime;

            if(trembling_timer>=trembling_time)
            {
                SetTrembling(false);
                SetFalling(true);
            }
        }

        if (is_falling)
        {
            falling_timer += Time.deltaTime;

            if(falling_timer>=falling_time)
            {
                SetFalling(false);

                _animator.SetTrigger(AP_Idle);

                switch (_behaviour)
                {
                    case FallingPlatformBehaviour.Deactivate:
                        gameObject.SetActive(false);
                        break;
                    
                    case FallingPlatformBehaviour.BringBack:
                        transform.position = initial_position;
                        break;
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (is_falling || is_trembling)
        {
            return;
        }

        SetTrembling(true);
    }
    #endregion MonoBehaviour

    #region Red
    private void SetTrembling(bool trembling)
    {
        trembling_timer = 0.0f;

        is_trembling = trembling;

        if (is_trembling)
        {
            _animator.SetTrigger(AP_Trembling);
        }
    }

    private void SetFalling(bool falling)
    {
        falling_timer = 0.0f;

        is_falling = falling;

        if (is_falling)
        {
            _animator.SetTrigger(AP_Falling);
        }
    }
    #endregion Red
}
