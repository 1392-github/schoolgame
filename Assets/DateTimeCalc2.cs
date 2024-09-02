using System;

public static class DateTimeCalc2
{
    public static readonly TimeSpan day = new TimeSpan(1, 0, 0, 0);
    public static TimeSpan Add(TimeSpan a, TimeSpan b)
    {
        TimeSpan c = a + b;
        if (c >= day)
        {
            return c - day;
        }
        else
        {
            return c;
        }
    }
    public static TimeSpan Sub(TimeSpan a, TimeSpan b)
    {
        TimeSpan c = a - b;
        if (c < TimeSpan.Zero)
        {
            return c + day;
        }
        else
        {
            return c;
        }
    }
    public static TimeSpan Add2(TimeSpan a, TimeSpan b, out bool o)
    {
        TimeSpan c = a + b;
        if (c >= day)
        {
            o = true;
            return c - day;
        }
        else
        {
            o = false;
            return c;
        }
    }
    public static bool Between(TimeSpan a, TimeSpan b, TimeSpan c)
    {
        if (b > c)
        {
            return a >= b || a <= c;
        }
        else
        {
            return a >= b && a <= c;
        }
    }
}
