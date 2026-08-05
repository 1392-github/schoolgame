using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class GameData
{
    #region 저장 데이터
    public static string name;
    public static string school;
    public static int birth;
    public static long exp;
    public static int money;
    public static DateTime time;
    public static TimeSpan timeSpeed;
    public static bool inClass;
    public static bool inSchool;
    public static string currentScene;
    public static int mapArgs;
    public static long[] studyExp;
    public static List<TestScore> scores;
    public static int schedule;
    public static bool[] achCompleted;
    public static int[] clas;
    public static bool duringClassPlacement;
    public static DateTime startClassPlacement;
    public static DateTime endClassPlacement;
    public static List<int> inventory;
    public static int speed;
    public static int[] stat;
    public static List<Experimental> experimental;
    public static DateTime startTime;
    public static TimeSpan totalPlayTime;
    public static int length;
    public static bool end;
    public static int difficulty;
    public static int[] repeatGradeMax;
    public static List<Quest> quest;
    public static Quest1[] pendingQuest;
    public static bool tutorial;
    public static bool hiddenLevelMode;

    public static bool init;
    public static List<Item> items;
    public static List<StatType> statTypes;

    public static float x; // 이건 처음 넘겨줄 때만 씀
    public static float y; // 이것도 마찬가지
    #endregion
    #region 저장 데이터가 아닌 변수들
    public static int startYear;
    public static DateTime suneungDay;
    //public static SuneungDays suneungDays;
    public static DateTime firstDay;
    public static string saveName;
    #endregion
    public static void Load(SaveFile8 save)
    {
        time = DateTime.ParseExact(save.time, "yyyy-MM-dd HH:mm:ss", null);
        timeSpeed = TimeSpan.Parse(save.timeSpeed);
        name = save.name;
        school = save.school;
        birth = save.birth;
        startYear = birth + 16;
        firstDay = new DateTime(startYear, 3, 2, 8, 0, 0);
        if (firstDay.DayOfWeek == DayOfWeek.Saturday) firstDay = firstDay.AddDays(2);
        if (firstDay.DayOfWeek == DayOfWeek.Sunday) firstDay = firstDay.AddDays(1);
        //suneungDay = DateTime.ParseExact(suneungDays.days[startYear - 1991], "yyyy-MM-dd", null);
        exp = save.exp;
        money = save.money;
        studyExp = save.studyExp;
        scores = save.scores;
        schedule = save.schindex;
        inClass = save.inclass;
        inSchool = save.inschool;
        clas = save.clas;
        inventory = save.inventory;
        achCompleted = save.achCompleted;
        duringClassPlacement = save.duringClassPlacement;
        startClassPlacement = DateTime.ParseExact(save.startClassPlacement, "yyyy-MM-dd", null);
        endClassPlacement = DateTime.ParseExact(save.endClassPlacement, "yyyy-MM-dd", null);
        speed = save.speed;
        experimental = save.experimental;
        currentScene = save.map;
        mapArgs = save.mapextra;
        stat = save.stat;
        x = save.x;
        y = save.y;
        if (stat.Length < statTypes.Count)
        {
            stat = stat.Concat(new int[statTypes.Count - stat.Length]).ToArray();
        }
        startTime = DateTime.ParseExact(save.startTime, "yyyy-MM-dd HH:mm:ss", null);
        totalPlayTime = TimeSpan.ParseExact(save.totalPlayTime, "d\\:hh\\:mm\\:ss", null);
        length = save.length;
        end = save.end;
        difficulty = save.difficulty;
        repeatGradeMax = save.repeatGradeMax;
        quest = save.quest;
        pendingQuest = save.pendingQuest;
        tutorial = save.tutorial;
        hiddenLevelMode = save.hiddenLevelMode;
    }
}
