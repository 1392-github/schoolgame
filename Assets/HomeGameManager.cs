using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGameManager : MonoBehaviour
{
    [SerializeField] HomeUIManager uiManager;
    void Start()
    {
        if (GameData.nextDayOnHome)
        {
            StartDay();
            GameData.nextDayOnHome = false;
        }
    }
    public void StartDay()
    {
        DayOfWeek dayOfWeek = GameData.time.DayOfWeek;
        if (GameData.time.Hour < 8) GameData.weekend = dayOfWeek == DayOfWeek.Sunday || dayOfWeek == DayOfWeek.Monday;
        else GameData.weekend = dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
        if (GameData.weekend)
        {
            GameData.inSchool = false;
            uiManager.nextDayButton.interactable = true;
            GameData.timeSpeed = new TimeSpan(0, 0, 30);
        }
        else
        {
            GameData.inSchool = true;
            uiManager.nextDayButton.interactable = false;
            GameData.timeSpeed = new TimeSpan(0, 1, 0);
        }
        GameData.schedule = 0;
        if (GameData.time.Date == GameData.endClassPlacement)
        {
            GameData.duringClassPlacement = false;
        }
        for (int i = GameData.quest.Count - 1; i >= 0; i--)
        {
            Quest q = GameData.quest[i];
            if (DateTime.ParseExact(q.timeLimit, "yyyy-MM-dd", null) > GameData.time.Date)
            {
                continue;
            }
            bool fail = false;
            for (int j = 0; j < 5; j++)
            {
                if (GameData.studyExp[j] < q.req[j])
                {
                    fail = true;
                    break;
                }
            }
            if (fail)
            {
                //SendMessage($"퀘스트를 실패하여 {q.reward} XP를 잃었습니다");
                GameData.GiveExp(-q.reward, false);
                GameData.quest.RemoveAt(i);
            }
        }
    }
}
