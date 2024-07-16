using System.Collections;
using System.Collections.Generic;
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
        else
        {
            GameObject.Find("Canvas").transform.Find("UnsupportedError").gameObject.SetActive(true);
            GameObject.Find("Canvas").transform.Find("UnsupportedError").Find("Text (Legacy)").GetComponent<UnityEngine.UI.Text>().text = $"호환되지 않는 버전입니다\n{GetSaveFile<SaveFile0>().versionName} 이상의 버전으로 플레이해 주세요";
            return;
        }
        if (version <= 1) // 1 (1.0~1.03) => 2 (1.04~)
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
        SceneManager.LoadScene("GlobalScene");
    }
}
