using UnityEngine;

public struct PositionOffsets
{
    public PositionOffsets(Vector2 v_center, Vector2 v_top_center, Vector2 v_bottom_center, Vector2 v_left_center, Vector2 v_right_center)
    {
        center = v_center;
        top_center = v_top_center;
        bottom_center = v_bottom_center;
        left_center = v_left_center;
        right_center = v_right_center;
    }

    public Vector2 center;
    public Vector2 top_center;
    public Vector2 bottom_center;
    public Vector2 left_center;
    public Vector2 right_center;
}

public struct ObjectSize
{
    public ObjectSize(float v_width, float v_height)
    {
        width = v_width;
        height = v_height;
    }

    public float width;
    public float height;
}
