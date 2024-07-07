using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
[System.Serializable]
public class ChatElement
{
    public string character;
    public Color characterColor;
    [TextArea]
    public string value;
    public int next;
    public UnityEvent chatEvent;
    public List<NameAndVal<int>> option;
}
