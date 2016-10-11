using System;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

public class PlayerCamera : MonoBehaviour
{
    const float TARGETTING_ACCURACY = 0.05f;

    #region Variables
    [Range(0.0f, 0.5f)]
    public float camera_offset_x;

    [Range(-0.5f, 0.5f)]
    public float camera_offset_y;

    private Camera _camera;

    private PlayerController player;

    private Blur _blur;

    private Twirl _twirl;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        _camera = transform.GetComponent<Camera>();
        _blur = GetComponent<Blur>();
        _twirl = GetComponent<Twirl>();
    }

    private void LateUpdate()
    {
        if (player == null)
        {
            return;
        }

        Vector2 hero_position = player.HeroPosition();
        Vector2 hero_velocity = player.HeroVelocity();
        Vector2 camera_position = transform.position;
        Vector2 hero_look = player.HeroLook();

        float screen_size = _camera.orthographic ? _camera.orthographicSize * 2 : Math.Abs(transform.position.z);

        Vector2 end_position = new Vector2
            (
            hero_position.x + (camera_offset_x * player.HeroForwardDirection().x + hero_look.x) * screen_size,
            hero_position.y + (camera_offset_y + hero_look.y) * screen_size
            );

        float delta_x = DeltaTranslation(end_position.x, camera_position.x, hero_velocity.x, camera_offset_x);
        float delta_y = DeltaTranslation(end_position.y, camera_position.y, hero_velocity.y, camera_offset_y);

        if (delta_x != 0 || delta_y != 0)
        {
            transform.Translate(delta_x, delta_y, 0.0f, Space.World);
        }
    }
    #endregion MonoBehaviour

    #region Red
    private float DeltaTranslation(float v_end_position, float v_camera_position, float v_hero_velocity, float v_camera_offset)
    {
        float delta = v_end_position - v_camera_position;

        if (Math.Abs(delta) > TARGETTING_ACCURACY)
        {
            float hero_velocity = Math.Abs(v_hero_velocity);

            if (hero_velocity < 1)
            {
                hero_velocity = 1;
            }

            float k = hero_velocity * (1 - v_camera_offset) * Time.deltaTime;

            if (k < 1)
            {
                delta *= k;
            }
        }

        return delta;
    }

    public void SetPlayerReference(PlayerController v_player)
    {
        player = v_player;
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

    public void BlurEffect(bool enable)
    {
        _blur.enabled = enable;
    }

    public System.Collections.IEnumerator TwirlEffect(float full_duration, float step_duration, float full_angle)
    {
        float delta_angle = step_duration * full_angle / full_duration;
        
        _twirl.angle = 0;

        _twirl.radius = new Vector2(0.75f, 0.75f);

        _twirl.enabled = true;

        while(_twirl.angle<full_angle)
        {
            _twirl.angle += delta_angle;

            yield return new WaitForSecondsRealtime(step_duration);
        }

        _twirl.enabled = false;
    }
    #endregion Red
}
