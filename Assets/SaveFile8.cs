using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveFile8 : SaveFile0
{
    public string time;
    public string timeSpeed;
    public bool introCompleted;
    public string name;
    public string school;
    public int birth;
    public int money;
    public long exp;
    public long[] studyExp;
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
    public List<int> inventory;
    public int speed;
    public int[] stat;
    public List<Experimental> experimental;
    public string startTime;
    public string totalPlayTime;
    public int length;
    public bool end;
    public int difficulty;
    public int[] repeatGradeMax;
    public List<Quest> quest;
    public Quest1[] pendingQuest;
    public bool tutorial;
    public bool hiddenLevelMode;
}
