using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable, CreateAssetMenu(fileName = "Chat", menuName = "ScriptableObject/Chat")]
public class Chat : ScriptableObject
{
    public string name;
    public List<ChatElement> value;
    public Action endEvent;
}
