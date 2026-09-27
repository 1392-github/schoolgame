using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable, CreateAssetMenu(fileName = "Chat", menuName = "ScriptableObject/Chat")]
public class Chat : ScriptableObject
{
    public string name;
    public bool skipable;
    public List<ChatElement> value;
    public bool pauseGame;
    public int endEvent = -1;
}
