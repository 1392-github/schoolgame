using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Item
{
    public string name;
    public string desc;
    public int cost;
    public Func<object[]> descExt;
    public Func<bool> use;
}
