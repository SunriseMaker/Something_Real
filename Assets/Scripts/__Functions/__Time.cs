using System;
using UnityEngine;

public static class __Time
{
    private static float saved_time_scale = 1.0f;

    #region Convertion
    const int SECONDS_IN_HOUR = 3600;

    public static DateTime Vector3_To_DateTime(DateTime ymd, Vector3 hms)
    {
        int hours = (int)hms.x;
        hours = Math.Max(0, hours);
        hours = Math.Min(23, hours);

        int minutes = (int)hms.y;
        minutes = Math.Max(0, minutes);
        minutes = Math.Min(59, minutes);

        int seconds = (int)hms.z;
        seconds = Math.Max(0, seconds);
        seconds = Math.Min(59, seconds);

        return new DateTime(ymd.Year, ymd.Month, ymd.Day, hours, minutes, seconds);
    }

    #region HMS_to_Seconds
    public static int HMS_to_Seconds(DateTime time)
    {
        return HMS_to_Seconds(time.Hour, time.Minute, time.Second);
    }

    public static int HMS_to_Seconds(Vector3 time)
    {
        return HMS_to_Seconds((int)time.x, (int)time.y, (int)time.z);
    }

    public static int HMS_to_Seconds(int h, int m, int s)
    {
        return h * 3600 + m * 60 + s;
    }

    public static string Time24(DateTime dt)
    {
        return dt.Hour.ToString().PadLeft(2, '0') + ":" + dt.Minute.ToString().PadLeft(2, '0');
    }

    public static float SecondsToHours(float seconds)
    {
        return seconds / SECONDS_IN_HOUR;
    }
    #endregion HMS_to_Seconds
    #endregion Convertion

    #region PauseGame
    public static void PauseGame()
    {
        saved_time_scale = Time.timeScale;
        Time.timeScale = 0.0f;
        GameData.GameTime.game_paused = true;
    }

    public static void UnpauseGame()
    {
        Time.timeScale = saved_time_scale;
        GameData.GameTime.game_paused = false;
    }

    public static System.Collections.IEnumerator SlowMotion(float slow_coefficient, float duration)
    {
        Time.timeScale = GameData.GameTime.normal_time_scale / slow_coefficient;
        Time.fixedDeltaTime = Time.timeScale * GameData.GameTime.normal_fixed_delta_time;

        yield return new WaitForSeconds(duration);

        Time.timeScale = GameData.GameTime.normal_time_scale;
        Time.fixedDeltaTime = GameData.GameTime.normal_fixed_delta_time;
    }
    #endregion PauseGame
}

public class Interval
{
    public DateTime start;
    public DateTime end;

    public Interval(DateTime v_start, DateTime v_end)
    {
        start = v_start;
        end = v_end;
    }

    public bool Inside(DateTime dt)
    {
        return dt >= start && dt <= end;
    }

    public override string ToString()
    {
        return __Time.Time24(start) + " - " + __Time.Time24(end);
    }
}