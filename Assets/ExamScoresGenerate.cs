using System;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            ExamScoreDisplay s = Instantiate(exam, transform).GetComponent<ExamScoreDisplay>();
            int i2 = i;
            s.button.onClick.AddListener(() =>
            {
                ExamManager.openExamType = examType;
                ExamManager.openExam = i2;
                SceneManager.LoadScene("ExamScore");
            });
            s.examName.text = ExamManager.GetExamName(examType, i);
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
                StringBuilder stringBuilder = new StringBuilder();
                for (int j = 0; j < 5; j++)
                {
                    stringBuilder.Append(ExamManager.GetExamScore(examType, i).grade[j]);
                }
                s.rank.text = stringBuilder.ToString();
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
