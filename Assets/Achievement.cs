using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Achievement
{
    public string name;
    public string desc;
    public int exp;
    public int grade;
    public bool excludeEnding;
    public UnityEngine.Events.UnityEvent para;
}
