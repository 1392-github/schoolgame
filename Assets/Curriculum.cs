using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Curriculum", menuName = "ScriptableObject/Curriculum")]
public class Curriculum : ScriptableObject
{
    public ExamType[] type2Exam;
}
