using System;
using UnityEngine;

public sealed class LightInterval : MonoBehaviour
{
    public string interval_name;

    public Vector3 time_start;

    public Vector3 time_end;

    public Color color_start;

    public Color color_end;

    public float intensity_start;

    public float intensity_end;

    [HideInInspector]
    public int start;

    [HideInInspector]
    public int end;

    [HideInInspector]
    public double duration;

    [HideInInspector]
    public float intensity_delta;

    [HideInInspector]
    public Color color_delta;

    [HideInInspector]
    public Interval interval;

    private void Awake()
    {
        DateTime dt = DateTime.MinValue;

        interval=new Interval(__Time.Vector3_To_DateTime(dt, time_start), __Time.Vector3_To_DateTime(dt, time_end));

        start = __Time.HMS_to_Seconds(interval.start);
        end = __Time.HMS_to_Seconds(interval.end);

        duration = end - start;
        intensity_delta = intensity_end - intensity_start;
        color_delta = color_end - color_start;
    }
}
