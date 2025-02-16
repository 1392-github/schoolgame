using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour
{
    SaveBuffer buf;
    T GetSaveFile<T>() where T : SaveFile0
    {
        return JsonUtility.FromJson<T>(System.IO.File.ReadAllText(Application.persistentDataPath + $"/{buf.name}"));
    }
    public void Play()
    {
        buf = GameObject.Find("SaveData").GetComponent<SaveBuffer>();
        buf.name = transform.parent.Find("Name").GetComponent<UnityEngine.UI.Text>().text;
        int version = GetSaveFile<SaveFile0>().version;
        if (version == 1)
        {
            buf.save = GetSaveFile<SaveFile1>();
        }
        else if (version == 2)
        {
            buf.save = GetSaveFile<SaveFile2>();
        }
        else if (version == 3)
        {
            buf.save = GetSaveFile<SaveFile3>();
        }
        else if (version == 4)
        {
            buf.save = GetSaveFile<SaveFile4>();
        }
        else if (version == 5)
        {
            buf.save = GetSaveFile<SaveFile5>();
        }
        else if (version == 6)
        {
            buf.save = GetSaveFile<SaveFile6>();
        }
        else
        {
            GameObject.Find("Canvas").transform.Find("UnsupportedError").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("UnsupportedError").Find("Text (Legacy)").GetComponent<UnityEngine.UI.Text>().text = $"호환되지 않는 버전입니다\n{GetSaveFile<SaveFile0>().versionName} 이상의 버전으로 플레이해 주세요";
            return;
        }
        if (version <= 1) // 1 (1.0~1.03) => 2 (1.04~1.08)
        {
            SaveFile1 current = (SaveFile1)buf.save;
            SaveFile2 next = new SaveFile2();
            next.time = current.time;
            next.timeSpeed = current.timeSpeed;
            next.money = current.money;
            next.exp = current.exp;
            next.level = current.level;
            next.studyExp = current.studyExp;
            next.scores = current.scores;
            next.map = current.map;
            next.mapextra = current.mapextra;
            next.x = current.x;
            next.y = current.y;
            next.schindex = current.schindex;
            next.inclass = current.inclass;
            next.inschool = current.inschool;
            next.achCompleted = current.achCompleted;
            next.clas = current.clas;
            next.duringClassPlacement = current.duringClassPlacement;
            next.startClassPlacement = current.startClassPlacement;
            next.endClassPlacement = current.endClassPlacement;
            next.busStopTime = current.busStopTime;
            next.nextBusStopTimeChange = current.nextBusStopTimeChange;
            next.inventory = current.inventory;
            next.speed = current.speed;
            next.repeatAll1 = current.repeatAll1;
            next.weeklyGoalSubject = current.weeklyGoalSubject;
            next.weeklyGoalValue = current.weeklyGoalValue;
            next.weeklyGoalReward = current.weeklyGoalReward;
            next.weeklyGoalTime = current.weeklyGoalTime;
            next.weeklyGoalCompleted = current.weeklyGoalCompleted;
            next.stat = current.stat;
            next.experimental = current.experimental;
            next.startTime = current.startTime;
            next.totalPlayTime = current.totalPlayTime;
            next.length = 0;
            next.end = false;
            buf.save = next;
        }
        if (version <= 2) // 2 (1.04~1.08) => 3 (1.09~1.10)
        {
            SaveFile2 current = (SaveFile2)buf.save;
            SaveFile3 next = new SaveFile3();
            next.time = current.time;
            next.timeSpeed = current.timeSpeed;
            next.money = current.money;
            next.exp = current.exp;
            next.level = current.level;
            next.studyExp = current.studyExp;
            next.scores = current.scores;
            next.map = current.map;
            next.mapextra = current.mapextra;
            next.x = current.x;
            next.y = current.y;
            next.schindex = current.schindex;
            next.inclass = current.inclass;
            next.inschool = current.inschool;
            next.achCompleted = current.achCompleted;
            next.clas = current.clas;
            next.duringClassPlacement = current.duringClassPlacement;
            next.startClassPlacement = current.startClassPlacement;
            next.endClassPlacement = current.endClassPlacement;
            next.busStopTime = current.busStopTime;
            next.nextBusStopTimeChange = current.nextBusStopTimeChange;
            next.inventory = current.inventory;
            next.speed = current.speed;
            next.repeatAll1 = current.repeatAll1;
            next.weeklyGoalSubject = current.weeklyGoalSubject;
            next.weeklyGoalValue = current.weeklyGoalValue;
            next.weeklyGoalReward = current.weeklyGoalReward;
            next.weeklyGoalTime = current.weeklyGoalTime;
            next.weeklyGoalCompleted = current.weeklyGoalCompleted;
            next.stat = current.stat;
            next.experimental = current.experimental;
            next.startTime = current.startTime;
            next.totalPlayTime = current.totalPlayTime;
            next.length = current.length;
            next.end = current.end;
            next.reqexpm = 1;
            buf.save = next;
        }
        if (version <= 3) // 3 (1.09~1.10) => 4 (1.11~1.13)
        {
            SaveFile3 current = (SaveFile3)buf.save;
            SaveFile4 next = new SaveFile4();
            next.time = current.time;
            next.timeSpeed = current.timeSpeed;
            next.money = current.money;
            next.exp = current.exp;
            next.studyExp = current.studyExp;
            next.scores = current.scores;
            next.map = current.map;
            next.mapextra = current.mapextra;
            next.x = current.x;
            next.y = current.y;
            next.schindex = current.schindex;
            next.inclass = current.inclass;
            next.inschool = current.inschool;
            next.achCompleted = current.achCompleted;
            next.clas = current.clas;
            next.duringClassPlacement = current.duringClassPlacement;
            next.startClassPlacement = current.startClassPlacement;
            next.endClassPlacement = current.endClassPlacement;
            next.busStopTime = current.busStopTime;
            next.nextBusStopTimeChange = current.nextBusStopTimeChange;
            next.inventory = current.inventory;
            next.speed = current.speed;
            next.goalSubject = current.weeklyGoalSubject;
            next.goalValue = current.weeklyGoalValue;
            next.goalReward = current.weeklyGoalReward;
            next.stat = current.stat;
            next.experimental = current.experimental;
            next.startTime = current.startTime;
            next.totalPlayTime = current.totalPlayTime;
            next.length = current.length;
            next.end = current.end;
            next.difficulty = current.reqexpm;
            next.repeatGradeMax = new int[8];
            next.costWeight = 100;
            next.costWeightStatus = Random.Range(0, 2) == 0;
            next.hash = "";
            next.stockAmount = new int[5];
            next.stockCost = Enumerable.Repeat(1000, 5).ToArray();
            next.stockCostChanged = new int[5];
            next.stockStatus = new bool[5];
            for (int i = 0; i < 5; i++)
            {
                next.stockStatus[i] = Random.Range(0, 2) == 0;
            }
            buf.save = next;
        }
        if (version <= 4) // 4 (1.11~1.13) => 5 (1.14~17)
        {
            SaveFile4 current = (SaveFile4)buf.save;
            SaveFile5 next = new SaveFile5();
            next.time = current.time;
            next.timeSpeed = current.timeSpeed;
            next.money = current.money;
            next.exp = current.exp;
            next.studyExp = current.studyExp.Select(c => (long)c).ToArray();
            next.scores = current.scores;
            next.map = current.map;
            next.mapextra = current.mapextra;
            next.x = current.x;
            next.y = current.y;
            next.schindex = current.schindex;
            next.inclass = current.inclass;
            next.inschool = current.inschool;
            next.achCompleted = current.achCompleted;
            next.clas = current.clas;
            next.duringClassPlacement = current.duringClassPlacement;
            next.startClassPlacement = current.startClassPlacement;
            next.endClassPlacement = current.endClassPlacement;
            next.busStopTime = current.busStopTime;
            next.nextBusStopTimeChange = current.nextBusStopTimeChange;
            next.inventory = current.inventory;
            next.speed = current.speed;
            next.goalSubject = current.goalSubject;
            next.goalValue = current.goalValue;
            next.goalReward = current.goalReward;
            next.stat = new int[6];
            if (current.stat.Length != 0)
            {
                next.stat[0] = current.stat[0];
                next.stat[1] = current.stat[1];
                next.stat[2] = current.stat[3];
                next.stat[3] = current.stat[4];
            }
            next.experimental = current.experimental;
            next.startTime = current.startTime;
            next.totalPlayTime = current.totalPlayTime;
            next.length = current.length;
            next.end = current.end;
            next.difficulty = current.difficulty;
            next.repeatGradeMax = current.repeatGradeMax;
            next.stockCost = current.stockCost;
            next.stockAmount = current.stockAmount;
            next.stockStatus = current.stockStatus;
            next.stockCostChanged = current.stockCostChanged;
            buf.save = next;
        }
        if (version <= 5) // 5 (14~17) => 6 (18~)
        {
            SaveFile5 current = (SaveFile5)buf.save;
            SaveFile6 next = new SaveFile6();
            next.time = current.time;
            next.timeSpeed = current.timeSpeed;
            next.money = current.money;
            next.exp = current.exp;
            next.studyExp = current.studyExp;
            next.scores = current.scores;
            next.map = current.map;
            next.mapextra = current.mapextra;
            next.x = current.x;
            next.y = current.y;
            next.schindex = current.schindex;
            next.inclass = current.inclass;
            next.inschool = current.inschool;
            next.achCompleted = current.achCompleted;
            next.clas = current.clas;
            next.duringClassPlacement = current.duringClassPlacement;
            next.startClassPlacement = current.startClassPlacement;
            next.endClassPlacement = current.endClassPlacement;
            next.busStopTime = current.busStopTime;
            next.nextBusStopTimeChange = current.nextBusStopTimeChange;
            next.inventory = current.inventory;
            next.speed = current.speed;
            next.stat = current.stat;
            next.experimental = current.experimental;
            next.startTime = current.startTime;
            next.totalPlayTime = current.totalPlayTime;
            next.length = current.length;
            next.end = current.end;
            next.difficulty = current.difficulty;
            next.repeatGradeMax = current.repeatGradeMax;
            next.stockCost = current.stockCost;
            next.stockAmount = current.stockAmount;
            next.stockStatus = current.stockStatus;
            next.stockCostChanged = current.stockCostChanged;
            next.quest = current.quest;
            next.pendingQuest = current.pendingQuest;
            next.tutorial = current.tutorial;
            next.hiddenLevelMode = current.hiddenLevelMode;
            if (next.experimental.Contains(Experimental.QUEST))
            {
                next.experimental.Remove(Experimental.QUEST);
            }
            else
            {
                long[] req = new long[5];
                req[current.goalSubject] = current.goalValue;
                next.quest = new List<Quest>();
                next.quest.Add(new Quest() { req = req, reward = current.goalReward, timeLimit = "9999-12-31" });
                next.pendingQuest = new Quest1[5];
                for (int i = 0; i < 5; i++)
                {
                    next.pendingQuest[i] = new Quest1();
                }
            }
            buf.save = next;
        }
        SceneManager.LoadScene("GlobalScene");
    }
}
