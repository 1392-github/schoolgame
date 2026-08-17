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
    public static int schedule;
    public static bool[] achCompleted;
    public static int[] clas;
    public static bool duringClassPlacement;
    public static DateTime startClassPlacement;
    public static DateTime endClassPlacement;
    public static List<int> inventory;
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
    public static SuneungDays suneungDays;
    public static DateTime firstDay;
    public static string saveName;
    public static int grade;
    public static int semester;
    public static bool nextDayOnHome;
    public static HomeUIManager uiManager;
    public static QuestPreview questPreview;
    public static bool weekend;
    public static Curriculum curriculum;
    public static ExamType[] type1Exams;
    public static DateTime[] type1ExamDate;
    public static DateTime[] type2ExamDate;
    static readonly int[] firstWed = { 3, 2, 1, 0, 6, 5, 4 };
    static readonly int[] lastWed = { -4, -5, -6, 0, -1, -2, -3 };
    static readonly int[] thirdThu = { 18, 17, 16, 15, 14, 20, 19 };
    #endregion
    #region 스탯 정보 속성
    public static long needExpForLvUP => (long)(30 * Mathf.Pow(1.07f, stat[0]));
    public static float studyLvBonus => Mathf.Pow(1.03f, stat[0]);
    public static int LvIncome => (int)(10000 * Mathf.Pow(1.025f, stat[1]));
    public static int maxQuest => (hiddenLevelMode ? stat[3] / 10 : stat[2]) + 1;
    public static int questTime
    {
        get
        {
            if (hiddenLevelMode)
            {
                return stat[3] / 10;
            }
            else
            {
                if (stat[3] >= 20)
                {
                    return stat[3] * 3 - 27;
                }
                else if (stat[3] >= 10)
                {
                    return stat[3] * 2 - 8;
                }
                else
                {
                    return stat[3] + 1;
                }
            }
        }
    }
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
        schedule = save.schindex;
        inClass = save.inclass;
        inSchool = save.inschool;
        clas = save.clas;
        inventory = save.inventory;
        achCompleted = save.achCompleted;
        duringClassPlacement = save.duringClassPlacement;
        startClassPlacement = DateTime.ParseExact(save.startClassPlacement, "yyyy-MM-dd", null);
        endClassPlacement = DateTime.ParseExact(save.endClassPlacement, "yyyy-MM-dd", null);
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
        ExamManager.type1Exam = save.type1Exam;
        ExamManager.type2Exam = save.type2Exam;
        ExamManager.currentExamType = save.currentExamType;
        ExamManager.currentExam = save.currentExam;
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
        if (time.Month == 1) grade--; // 1월 1일 0~8시는 학년 안 오름
        semester = time.Month >= 9 || time.Month == 1 ? 2 : 1;
        DayOfWeek dayOfWeek = time.DayOfWeek;
        weekend = dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        type1ExamDate = new DateTime[type1Exams.Length];
        for (int i = 0; i < type1Exams.Length; i++)
        {
            type1ExamDate[i] = new DateTime(startYear + type1Exams[i].grade - 1, type1Exams[i].month, DateTime.DaysInMonth(1, type1Exams[i].month)); // DaysInMonth에서 month는 2가 아니기에(2월에 시험없고 방학임) year 값은 상관없어서 1 넣음
            if (type1ExamDate[i].DayOfWeek == DayOfWeek.Saturday) type1ExamDate[i] = type1ExamDate[i].AddDays(-1);
            if (type1ExamDate[i].DayOfWeek == DayOfWeek.Sunday) type1ExamDate[i] = type1ExamDate[i].AddDays(-2);
        }
        type2ExamDate = new DateTime[curriculum.type2Exam.Length];
        for (int i = 0; i < curriculum.type2Exam.Length; i++)
        {
            DateTime c;
            switch (curriculum.type2Exam[i].dateType)
            {
                case ExamType.DateType.FIRST_WEDNESDAY:
                    c = new DateTime(startYear + curriculum.type2Exam[i].grade - 1, curriculum.type2Exam[i].month, 1);
                    type2ExamDate[i] = c.AddDays(firstWed[(int)c.DayOfWeek]);
                    break;
                case ExamType.DateType.LAST_WEDNESDAY:
                    c = new DateTime(startYear + curriculum.type2Exam[i].grade - 1, curriculum.type2Exam[i].month, DateTime.DaysInMonth(1, curriculum.type2Exam[i].month));
                    type2ExamDate[i] = c.AddDays(lastWed[(int)c.DayOfWeek]);
                    break;
                case ExamType.DateType.LAST_WEEKDAY:
                    // Type 1에서만 쓰는 거라 구현안함
                    break;
                case ExamType.DateType.SUNEUNG:
                    if (curriculum.type2Exam[i].grade != 3 || curriculum.type2Exam[i].month != 11) throw new ArgumentException("ExamType.DateType.SUNEUNG only availble in 3rd grade november type 2 exam.");
                    if (startYear - 1991 < suneungDays.days.Length)
                    {
                        type2ExamDate[i] = DateTime.ParseExact(suneungDays.days[startYear - 1991], "yyyy-MM-dd", null);
                    }
                    else
                    {
                        c = new DateTime(startYear + 2, 11, 1);
                        type2ExamDate[i] = c.AddDays(thirdThu[(int)c.DayOfWeek]);
                    }
                    break;
            }
        }
        StudentCard.LoadPhoto();
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
        save.type1Exam = ExamManager.type1Exam;
        save.type2Exam = ExamManager.type2Exam;
        save.currentExamType = ExamManager.currentExamType;
        save.currentExam = ExamManager.currentExam;
        save.studentCardPatternColor = StudentCard.patternColor;
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
