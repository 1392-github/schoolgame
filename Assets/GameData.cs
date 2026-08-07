using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public static int grade;
    public static int semester;
    public static bool nextDayOnHome;
    public static HomeUIManager uiManager;
    public static QuestPreview questPreview;
    public static bool weekend;
    #endregion
    #region 스탯 정보 속성
    public static long needExpForLvUP => (long)(30 * Mathf.Pow(1.07f, stat[0]));
    public static float studyLvBonus => Mathf.Pow(1.03f, stat[0]);
    public static int LvIncome => (int)(10000 * Mathf.Pow(1.025f, stat[1]));
    public static int classPlacementChance => Mathf.Clamp(10 + stat[2] * 2, 10, 100);
    public static float problemTime => 60 * Mathf.Pow(0.99f, stat[3]);
    public static int maxQuest => stat[4] <= 0 ? 1 : (hiddenLevelMode ? stat[4] / 10 : stat[4]) + 1;
    public static int questTime => stat[5] <= 0 ? 1 : (hiddenLevelMode ? stat[5] / 10 : stat[5]) + 1;
    #endregion
    public static Player player;
    public static void Load(SaveFile8 save)
    {
        time = DateTime.ParseExact(save.time, "yyyy-MM-dd HH:mm:ss", null);
        timeSpeed = TimeSpan.Parse(save.timeSpeed);
        name = save.name;
        school = save.school;
        birth = save.birth;
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
        if (save.introCompleted) Load2();
    }
    public static void Load2()
    {
        startYear = birth + 16;
        firstDay = new DateTime(startYear, 3, 2, 8, 0, 0);
        if (firstDay.DayOfWeek == DayOfWeek.Saturday) firstDay = firstDay.AddDays(2);
        if (firstDay.DayOfWeek == DayOfWeek.Sunday) firstDay = firstDay.AddDays(1);
        if (time == default)
        {
            time = firstDay;
        }
        //suneungDay = DateTime.ParseExact(suneungDays.days[startYear - 1991], "yyyy-MM-dd", null);
        if (stat.Length < statTypes.Count)
        {
            stat = stat.Concat(new int[statTypes.Count - stat.Length]).ToArray();
        }
        grade = time.Year - startYear + 1;
        semester = time.Month >= 9 ? 2 : 1;
        DayOfWeek dayOfWeek = time.DayOfWeek;
        weekend = dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
    }
    public static void Save()
    {
        if (tutorial)
        {
            return;
        }
        SaveFile8 save = new SaveFile8();
        save.version = 8;
        save.versionName = "21";
        save.time = time.ToString("yyyy-MM-dd HH:mm:ss");
        save.timeSpeed = timeSpeed.ToString();
        save.name = name;
        save.school = school;
        save.birth = birth;
        save.exp = exp;
        save.money = money;
        save.studyExp = studyExp;
        save.map = currentScene;
        save.mapextra = mapArgs;
        save.scores = scores;
        if (currentScene != "Home")
        {
            Transform playerTransform = GameObject.Find("Player").transform;
            save.x = playerTransform.position.x;
            save.y = playerTransform.position.y;
        }
        save.schindex = schedule;
        save.inclass = inClass;
        save.inschool = inSchool;
        save.achCompleted = achCompleted;
        save.clas = clas;
        save.duringClassPlacement = duringClassPlacement;
        save.startClassPlacement = startClassPlacement.ToString("yyyy-MM-dd");
        save.endClassPlacement = endClassPlacement.ToString("yyyy-MM-dd");
        save.inventory = inventory;
        save.speed = speed;
        save.stat = stat;
        save.experimental = experimental;
        save.startTime = startTime.ToString("yyyy-MM-dd HH:mm:ss");
        save.totalPlayTime = totalPlayTime.ToString("d\\:hh\\:mm\\:ss");
        save.length = length;
        save.end = end;
        save.difficulty = difficulty;
        save.repeatGradeMax = repeatGradeMax;
        save.tutorial = tutorial;
        save.quest = quest;
        save.pendingQuest = pendingQuest;
        save.hiddenLevelMode = hiddenLevelMode;
        save.introCompleted = true;
        File.WriteAllText(Path.Combine(Application.persistentDataPath, "saves", saveName), JsonUtility.ToJson(save));
    }
    public static void giveStudyExp(int sub, int min, int max)
    {
        int amount = Random.Range((int)(min * studyLvBonus), (int)(max * studyLvBonus) + 1);
        studyExp[sub] += amount;
        if (player != null) player.SendMessage($"{Util.subjectName[sub]} 능력치가 {Mathf.Abs(amount)} {(amount >= 0 ? "증가" : "감소")}했습니다");
        for (int i = quest.Count - 1; i >= 0; i--)
        {
            Quest q = quest[i];
            bool complete = true;
            for (int j = 0; j < 5; j++)
            {
                if (studyExp[j] < q.req[j])
                {
                    complete = false;
                    break;
                }
            }
            if (complete)
            {
                if (player != null)
                {
                    player.SendMessage($"퀘스트를 성공하여 {q.reward} XP를 획득했습니다");
                }
                GiveExp(q.reward, false);
                quest.RemoveAt(i);
            }
        }
        if (player != null)
        {
            player.UpdateQuestList();
        }
        if (questPreview != null) questPreview.UpdatePreview();
    }
    public static void GiveExp(long amount, bool msg = true)
    {
        exp += amount;
        if (msg)
        {
            if (player != null) player.SendMessage($"{amount} 경험치를 획득했습니다");
        }
        if (uiManager != null) uiManager.UpdateXPDisplay();
        if (hiddenLevelMode)
        {
            while (exp >= needExpForLvUP)
            {
                exp -= needExpForLvUP;
                for (int i = 0; i < stat.Length; i++)
                {
                    stat[i]++;
                }
                if (player != null)
                {
                    player.SendMessage($"레벨 {stat[0] + 1}을 달성했습니다");
                    player.UpdateLv();
                }
            }
        }
    }
    public static bool ExperimentalCheck(Experimental e)
    {
        return experimental.Contains(e);
    }
}
