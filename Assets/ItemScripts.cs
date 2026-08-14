using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemScripts
{
    public static HomeUIManager uiManager;
    public static object[] Item1Desc(int l)
    {
        return new object[] { (int)(l * GameData.studyLvBonus), (int)(l * 2 * GameData.studyLvBonus) };
    }
    public static bool UseItem1(int id)
    {
        if (GameData.weekend)
        {
            if (GameData.time.TimeOfDay >= new TimeSpan(7, 0, 0) && GameData.time.TimeOfDay <= new TimeSpan(8, 0, 0))
            {
                if (uiManager != null) uiManager.OpenDialog("이 아이템은 7시 ~ 8시까지 사용할 수 없습니다");
                return false;
            }
        }
        else
        {
            if (GameData.time.TimeOfDay >= new TimeSpan(7, 0, 0) && GameData.time.TimeOfDay <= new TimeSpan(14, 50, 0))
            {
                if (uiManager != null) uiManager.OpenDialog("이 아이템은 7시 ~ 14시 50분까지 사용할 수 없습니다");
                return false;
            }
        }
        /*
        임시로 문제 푸는 기능 없앰
        cntProblemItem = id;
        Problem p = data.problem[id].value[Random.Range(0, data.problem[id].value.Count)];
        problem.SetActive(true);
        problem.transform.SetAsLastSibling();
        if (p.imgContent == null)
        {
            problemText.gameObject.SetActive(true);
            problemImage.gameObject.SetActive(false);
            problemText.text = p.content;
        }
        else
        {
            problemText.gameObject.SetActive(false);
            problemImage.gameObject.SetActive(true);
            problemImage.sprite = p.imgContent;
        }
        problemAnswer = p.answer;
        problemTimer = new TimeSpan(0, 1, 0) * problemTime;
        timeSpeed = new TimeSpan(0, 0, 30) * (problemTime / 60);*/
        // v1.0에서는 문제 풀 필요 없음
        int l = id % 10 + 1;
        GameData.giveStudyExp(id / 10, l, l * 2);
        GameData.time += new TimeSpan(0, 60, 0);
        return true;
    }
}
