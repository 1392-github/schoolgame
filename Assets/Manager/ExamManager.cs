using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExamManager
{
    public static ExamScore[] type1Exam;
    public static ExamScore[] type2Exam;
    public static int currentExamType;
    public static int currentExam;
    public static int openExamType;
    public static int openExam;

    public static readonly int[] gradeCutPercentile = {96, 89, 77, 60, 40, 23, 11, 4};
    static readonly float[] gradeCutStandard = {
        1.750686f,
        1.226528f,
        0.738846f,
        0.253347f,
        -0.253347f,
        -0.738846f,
        -1.226528f,
        -1.750686f,
    };

    public static void Exam()
    {
        if (currentExamType == 0)
        {
            return;
        }
        ExamType examType;
        if (currentExamType == 1)
        {
            examType = GameData.type1Exams[currentExam];
        }
        else
        {
            examType = GameData.curriculum.type2Exam[currentExam];
        }
        ExamScore score = new ExamScore();
        for (int i = 0; i < 5; i++)
        {
            score.rawScore[i] = Mathf.Clamp((int)(Mathf.Log(GameData.studyExp[i] + examType.logShift, examType.logBase) - Mathf.Log(examType.logShift, examType.logBase)) + Random.Range(-3, 4), 0, 100);
            score.average[i] = Random.Range(50, 61);
            score.deviation[i] = Random.Range(20, 26);
            float zScore = (score.rawScore[i] - score.average[i]) / (float)score.deviation[i];
            score.standardScore[i] = Mathf.FloorToInt(zScore * 20 + 100.5f);
            float percentile = Util.GetNormalCDF(zScore);
            score.percentile[i] = (int)(percentile * 100);
            if (score.rawScore[i] == 100)
            {
                // 100점인데 1등급 안 나오는 것 방지
                score.grade[i] = 1;
            }
            else
            {
                if (score.percentile[i] < gradeCutPercentile[7])
                {
                    score.grade[i] = 9;
                }
                else
                {
                    for (int g = 0; g < 8; g++)
                    {
                        if (score.percentile[i] >= gradeCutPercentile[g])
                        {
                            score.grade[i] = g + 1;
                            break;
                        }
                    }
                }
            }
            for (int g = 0; g < 8; g++)
            {
                score.gradeCut[i * 8 + g] = Mathf.Clamp(Mathf.CeilToInt(gradeCutStandard[g] * score.deviation[i] + score.average[i]), 0, 100);
            }
            score.rank[i] = Mathf.CeilToInt((1 - percentile) * 300);
        }
        if (currentExamType == 1)
        {
            type1Exam[currentExam] = score;
        }
        else
        {
            type2Exam[currentExam] = score;
        }
    }
    public static string GetExamName(int examType, int exam)
    {
        if (examType == 1)
        {
            return GameData.type1Exams[exam].name;
        }
        else
        {
            ExamType e = GameData.curriculum.type2Exam[exam];
            return e.name.Replace("Y0", (GameData.startYear + e.grade - 1).ToString()).Replace("Y1", (GameData.startYear + e.grade).ToString());
        }
    }
    public static ExamScore GetExamScore(int examType, int exam)
    {
        if (examType == 1)
        {
            return type1Exam[exam];
        }
        else
        {
            return type2Exam[exam];
        }
    }
}
