using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Stats", menuName = "ScriptableObject/Stats")]
public class Stats : ScriptableObject
{
    public List<StatType> stats;
}
