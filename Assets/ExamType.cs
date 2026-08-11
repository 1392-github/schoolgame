using System;
[Serializable]
public class ExamType
{
    public enum DateType
    {
        FIRST_WEDNESDAY,
        LAST_WEDNESDAY,
        LAST_WEEKDAY,
        SUNEUNG
    }
    public string name;
    public float logBase;
    public int logShift;
    public DateType dateType;
    public int grade;
    public int month;
}
