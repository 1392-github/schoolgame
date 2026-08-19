using System;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class ChatElement
{
    //public string character;
    [TextArea(10, 10)]
    public string value;
    public int next;
    public Func<object[]> chatEvent;
    public List<NameAndVal<int>> option;
    public bool disableNext;
}
