using System;
using UnityEngine;

public class ExamScoresGenerate : MonoBehaviour
{
    public GameObject exam;
    public int examType;
    void Start()
    {
        ExamType[] exams = examType == 1 ? GameData.type1Exams : GameData.curriculum.type2Exam;
        DateTime[] examDate = examType == 1 ? GameData.type1ExamDate : GameData.type2ExamDate;
        DateTime inGameDate = Util.InGameDate(GameData.time);
        for (int i = 0; i < exams.Length; i++)
        {
            ExamScore s = Instantiate(exam, transform).GetComponent<ExamScore>();
            s.examName.text = examType == 2 ? exams[i].name.Replace("Y0", (GameData.startYear + exams[i].grade - 1).ToString()).Replace("Y1", (GameData.startYear + exams[i].grade).ToString()) : exams[i].name;
            s.examDay.text = $"{examDate[i]:yyyy-MM-dd} ({Util.DDay(inGameDate, examDate[i])})";
            DateTime revealDate;
            if (exams[i].grade == 3 && exams[i].month == 12)
            {
                revealDate = examDate[i]; // 3-2 기말의 경우 examDate[i].AddDays(examType == 1 ? 7 : 14);를 하면 졸업 이후가 되어서 3-2 기말 성적표를 평생 못 보는 문제가 생김
            }
            else
            {
                revealDate = examDate[i].AddDays(examType == 1 ? 7 : 14);
            }
            s.revealDay.text = $"{revealDate:yyyy-MM-dd} ({Util.DDay(inGameDate, revealDate)})";
            if (inGameDate >= revealDate)
            {
                s.rank.text = "11111"; // 임시
                s.button.interactable = true;
            }
            else
            {
                s.rank.text = "-";
                s.button.interactable = false;
            }
        }
    }
}
