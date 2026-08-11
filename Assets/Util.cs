using System;
public static class Util
{
    public static readonly string[] subjectName = { "국어", "수학", "사회", "과학", "영어" };

    public static string DDay(DateTime from, DateTime to)
    {
        if (to > from)
        {
            return $"D-{(to - from).Days}";
        }
        else if (to < from)
        {
            return $"D+{(from - to).Days}";
        }
        else
        {
            return "D-Day";
        }
    }
    public static DateTime InGameDate(DateTime time)
    {
        if (time.Hour < 8)
        {
            return time.Date.AddDays(-1);
        }
        else
        {
            return time.Date;
        }
    }
}
