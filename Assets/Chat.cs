using System.Collections.Generic;
[System.Serializable]
public class Chat
{
    public string name;
    public List<ChatElement> value;
    public UnityEngine.Events.UnityEvent endEvent;
}
