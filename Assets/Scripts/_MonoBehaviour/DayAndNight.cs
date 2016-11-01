using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class DayAndNight : MonoBehaviour
{
    #region Variables
    private Light _light;

    private List<LightInterval> intervals;

    private string intervals_info;
    #endregion Variables

    #region MonoBehaviour
    private void Awake()
    {
        _light = GetComponent<Light>();

        intervals = gameObject.GetComponents<LightInterval>().ToList();

        intervals_info = "";
    }

    private void Start()
    {
        intervals_info = IntervalsInfo();
    }

    private void FixedUpdate()
    {
        SetLighting();
    }
    #endregion MonoBehaviour

    #region Red
    private void SetLighting()
    {
        float light_intensity = 1.0f;
        Color light_color = Color.green;

        int seconds = __Time.HMS_to_Seconds(GameData.GameTime.now);

        foreach (LightInterval li in intervals)
        {
            if (seconds >= li.start && seconds <= li.end)
            {
                float k = (float)((seconds - li.start) / li.duration);
                light_intensity = li.intensity_start + k * li.intensity_delta;
                light_color = li.color_start + k * li.color_delta;
            }
        }

        _light.intensity = light_intensity;
        _light.color = light_color;
    }

    public string Info()
    {
        string now_info = __Time.Time24(GameData.GameTime.now);

        string speed_info = GameData.GameTime.time_speed.ToString();

        string info =
            "Now: " + now_info + "\n" +
            "Time Speed: " + speed_info + "\n" +
            "\n" +
            "Intervals: \n" + intervals_info;

        return info;
    }

    public string IntervalsInfo()
    {
        string intervals_info = "";

        int k = 0;

        foreach (LightInterval li in intervals)
        {
            k++;

            if (intervals_info != "")
            {
                intervals_info += "\n";
            }

            intervals_info += k.ToString() + ". " + li.interval_name + ": " + li.interval.ToString();
        }

        return intervals_info;
    }
}
    #endregion Red
