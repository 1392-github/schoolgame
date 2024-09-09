using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile4 : SaveFile0
{
    public string time;
    public string timeSpeed;
    public int money;
    public int exp;
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
    public int goalSubject;
    public int goalValue;
    public int goalReward;
    public int[] stat;
    public List<Experimental> experimental;
    public string startTime;
    public string totalPlayTime;
    public int length;
    public bool end;
    public int difficulty;
    public int[] repeatGradeMax;
    public int costWeight;
    public bool costWeightStatus;
    public string hash; // ranking version only
    public int[] stockCost;
    public int[] stockAmount;
    public bool[] stockStatus;
    public int[] stockCostChanged;
}
