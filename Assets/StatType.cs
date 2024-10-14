[System.Serializable]
public class StatType
{
    public string name;
    public int reqBase;
    public float reqExp;
    public int max;
    public string prop;
    public string prefix;
    public string suffix;
    public Experimental experimental = Experimental.NONE;
    public UnityEngine.Events.UnityEvent onUpgrade;
}
