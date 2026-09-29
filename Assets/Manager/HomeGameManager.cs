using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeGameManager : MonoBehaviour
{
    [SerializeField] HomeUIManager uiManager;
    [SerializeField] QuestPreview questPreview;
    [SerializeField] Chat startChat;
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
        if (GameData.time == GameData.firstDay)
        {
            ChatManager.OpenChat(startChat);
        }
        if (GameData.weekendOrVacation)
        {
            GameData.inSchool = false;
            uiManager.nextDayButton.interactable = true;
            GameData.timeSpeed = new TimeSpan(0, 0, 30);
        }
        else
        {
            GameData.inSchool = true;
            uiManager.nextDayButton.interactable = false;
            GameData.timeSpeed = new TimeSpan(0, 0, 0);
        }
        GameData.schedule = 0;
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
        questPreview.UpdatePreview();
        ExamManager.currentExamType = 0;
        ExamManager.currentExam = 0;
        DateTime date = GameData.time.Date;
        for (int i = 0; i < GameData.type1ExamDate.Length; i++)
        {
            if (date == GameData.type1ExamDate[i])
            {
                ExamManager.currentExamType = 1;
                ExamManager.currentExam = i;
                break;
            }
        }
        if (ExamManager.currentExamType == 0)
        {
            for (int i = 0; i < GameData.type2ExamDate.Length; i++)
            {
                if (date == GameData.type2ExamDate[i])
                {
                    ExamManager.currentExamType = 2;
                    ExamManager.currentExam = i;
                    break;
                }
            }
        }
    }
}
