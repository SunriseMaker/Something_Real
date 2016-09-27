using System;
using UnityEngine;

public class CameraFollowingPlayer : MonoBehaviour
{
    const float TARGETTING_ACCURACY = 0.01f;

    #region Variables
    [Range(0.0f, 0.5f)]
    public float camera_offset_x;

    [Range(-0.5f, 0.5f)]
    public float camera_offset_y;

    private Camera _camera;

    private PlayerController player;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        _camera = transform.GetComponent<Camera>();
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector2 hero_position = player.HeroPosition();
        Vector2 camera_position = transform.position;
        Vector2 hero_look = player.HeroLook();

        float screen_size = _camera.orthographic ? _camera.orthographicSize * 2 : Math.Abs(transform.position.z);

        Vector2 end_position = new Vector2
            (
            hero_position.x + (camera_offset_x * player.HeroForwardDirection().x + hero_look.x) * screen_size,
            hero_position.y + (camera_offset_y + hero_look.y) * screen_size
            );

        #region delta_x
        float delta_x = end_position.x - camera_position.x;
        float absolute_delta_x = Math.Abs(delta_x);

        if (absolute_delta_x > TARGETTING_ACCURACY)
        {
            float hero_velocity = Math.Abs(player.HeroVelocity().x);

            if (hero_velocity < 1)
            {
                hero_velocity = 1;
            }

            float k = hero_velocity * (1 - camera_offset_x) * Time.deltaTime;

            if (k < 1)
            {
                delta_x *= k;
            }
        }
        #endregion delta_x

        #region delta_y
        float delta_y = end_position.y - camera_position.y;
        #endregion delta_y

        if (delta_x != 0 || delta_y != 0)
        {
            transform.Translate(delta_x, delta_y, 0.0f, Space.World);
        }
    }
    #endregion MonoBehaviour

    #region Red
    public void SetPlayerReference(PlayerController v_player)
    {
        player = v_player;
    }

    public void FocusOnHero()
    {
        if(player==null)
        {
            return;
        }

        Vector2 hero_position = player.HeroPosition();

        transform.position = new Vector3(hero_position.x, hero_position.y, transform.position.z);
    }

    public void Zoom(float f)
    {
        if (_camera.orthographic)
        {
            _camera.orthographicSize -= f;
        }
        else
        {
            transform.Translate(0, 0, -f, Space.World);
        }
    }
    #endregion Red
}

