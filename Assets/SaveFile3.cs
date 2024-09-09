using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile3 : SaveFile0
{
    public string time;
    public string timeSpeed;
    public int money;
    public int exp;
    public int level; // deperated
    public int[] studyExp;
    public List<TestScore> scores;
    public string map;
    public int mapextra;
    public float x;
    public float y;
    public int schindex;
    public bool inclass;
    public bool inschool;
    public bool[] achCompleted;
    public int[] clas;
    public bool duringClassPlacement; // reversed
    public string startClassPlacement;
    public string endClassPlacement; // reversed
    public List<string> busStopTime;
    public string nextBusStopTimeChange;
    public List<int> inventory;
    public int speed;
    public int repeatAll1; // deperated
    public int weeklyGoalSubject; // rename to goalSubject
    public int weeklyGoalValue; // rename to goalValue
    public int weeklyGoalReward; // rename to goalReward
    public string weeklyGoalTime; // deperated
    public bool weeklyGoalCompleted; // deperated
    public int[] stat;
    public List<Experimental> experimental;
    public string startTime;
    public string totalPlayTime;
    public int length;
    public bool end;
    public int reqexpm; // rename to difficulty next version
    public string hash; // ranking version only
}
