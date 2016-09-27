using UnityEngine;

public class AnimationMovement : MonoBehaviour
{
    public enum OnFinishMovement { Stop, TeleportBack, MoveBack }

    #region Variables
    public OnFinishMovement on_finish;

    public Vector2 distance;
    public float time;
    public float lifetime;

    private Vector2 start;
    private Vector2 end;
    private Vector2 destination;
    private Vector2 translocation;

    private bool forward;
    #endregion Variables

    #region MonoBehaviour
    private void Start()
    {
        if(time<=0)
        {
            time = 1.0f;
        }

        if(lifetime>0)
        {
            Destroy(this, lifetime);
        }

        translocation = distance / time;
        start = transform.position;
        end = start + distance;
        destination = end;
        forward = true;
    }

    private void FixedUpdate()
    {
        if (((Vector2)transform.position - destination).magnitude < 0.01)
        {
            switch (on_finish)
            {
                case OnFinishMovement.Stop:
                    translocation = Vector2.zero;
                    break;

                case OnFinishMovement.TeleportBack:
                    transform.position = start;
                    break;

                case OnFinishMovement.MoveBack:
                    Reverse();
                    break;
            }
        }
        else
        {
            transform.Translate(translocation * Time.deltaTime, Space.World);
        }
	}

    private void OnTriggerStay2D(Collider2D col)
    {
        col.gameObject.transform.Translate(translocation * Time.deltaTime, Space.World);
    }
    #endregion MonoBehaviour

    #region Red
    private void Reverse()
    {
        translocation = translocation * -1;

        forward = !forward;

        if (forward)
        {
            destination = end;
        }
        else
        {
            destination = start;
        }
    }
    #endregion Red
}
