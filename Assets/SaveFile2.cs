using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile2 : SaveFile0
{
    public string time;
    public string timeSpeed;
    public int money;
    public int exp;
    public int level;
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
    public bool duringClassPlacement;
    public string startClassPlacement;
    public string endClassPlacement;
    public List<string> busStopTime;
    public string nextBusStopTimeChange;
    public List<int> inventory;
    public int speed;
    public int repeatAll1;
    public int weeklyGoalSubject;
    public int weeklyGoalValue;
    public int weeklyGoalReward;
    public string weeklyGoalTime;
    public bool weeklyGoalCompleted;
    public int[] stat;
    public List<Experimental> experimental;
    public string startTime;
    public string totalPlayTime;
    public int length;
    public bool end;
}
