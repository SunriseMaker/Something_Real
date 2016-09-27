﻿using UnityEngine;

public class AnimationRotation : MonoBehaviour
{
    #region Variables
    public Vector3 rotation_angle;
    public bool destroy_component_on_collision;
    #endregion Variables

    #region MonoBehaviour
    void FixedUpdate ()
    {
        transform.Rotate(rotation_angle * Time.deltaTime);
	}

    protected virtual void OnCollisionEnter2D()
    {
        if(destroy_component_on_collision)
        {
            Destroy(this);
        }
    }
    #endregion MonoBehaviour
}
