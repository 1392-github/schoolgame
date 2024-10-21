using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BusStop 
{
    public string name;
    public string map;
    public int extra;
    public Vector3 loc;
    public int destDoorID = -1;
    public Direction2 direction;
}
