using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Data : MonoBehaviour
{
    public List<Schedule> schedule;
    public List<Schedule> tutorialSchedule;
    public List<int> NeedExpForGrade;
    public List<int> gradeRank;
    public List<string> subjectName;
    public List<Achievement> achievement;
    public List<string> achievementGrade;
    public List<int> friendableStudent;
    public List<BusStop> busStop;
    public List<Item> item;
    public List<Wrap<List<Problem>>> problem;
    public List<StatType> stat;
}
